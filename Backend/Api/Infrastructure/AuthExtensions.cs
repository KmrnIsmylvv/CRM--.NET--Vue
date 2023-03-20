using Elfo.Round.Identity;
using Elfo.Round.Identity.Impersonation;
using Elfo.Round.Identity.JWT;
using Elfo.Round.Profiler;
using Elfo.Contoso.LearningRoundKamran.Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class AuthExtensions
    {
        public static void AddAuthentication(this IServiceCollection service, IConfiguration configuration)
        {
            switch (configuration["Authentication:Type"])
            {
                case "JWT":
                    service.AddJWTAuthentication(configuration);
                    break;
                case "Windows":
                    service.AddWindowsAuthentication(configuration);
                    break;
                default:
                    break;
            }
        }

        private static void AddJWTAuthentication(this IServiceCollection service, IConfiguration configuration)
        {
            service
                .AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["Authentication:AuthSecret"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            string token = "";

                            if (context.Request.Headers.ContainsKey("Authorization"))
                            {
                                var parts = context.Request.Headers["Authorization"].ToString().Split(" ");
                                token = parts.Count() > 1 ? parts[1] : string.Empty;
                            }

                            if (context.Request.Query.ContainsKey("access_token"))
                            {
                                token = context.Request.Query["access_token"];
                            }

                            if (context.Request.Cookies.ContainsKey("access_token"))
                            {
                                token = context.Request.Cookies["access_token"];
                            }

                            context.Token = token;

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                context.Response.Headers.Add("Token-Expired", "true");

                            return Task.CompletedTask;
                        }
                    };
                })
                .AddRoundIdentity(authOptions =>
                {
                    authOptions.HowToObtainUsernameFromIdentityName = identityName => identityName;
                })
                .WithJWTGeneration(jwtCreation =>
                {
                    jwtCreation.Secret = configuration["Authentication:AuthSecret"];
                })
                .WithImpersonation(o =>
                {
                    o.Secret = configuration["Authentication:AuthSecret"];
                    o.KeepTheseClaimsDuringImpersonation = (claims) =>
                    {
                        return claims.ToList().Where(t => t.Type == RoundIdentityClaimType.Permission &&
                                                          t.Value == ProfilerPermissionClaim.CanProfile);
                    };
                })
                .WithMsSqlServer<UserInformationModel>(msSqlOption =>
                {
                    msSqlOption.ConnectionString = configuration.GetConnectionString("Db");
                    msSqlOption.ProfileConnection = true;
                });

            service.AddAuthorization(options =>
            {
                options.AddPolicy("HangfirePolicy", builder => builder
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireClaim(RoundIdentityClaimType.Permission, Permissions.HangfireDashboard_Reader));
            });
        }

        private static void AddWindowsAuthentication(this IServiceCollection service, IConfiguration configuration)
        {
            service
                .AddAuthentication(IISDefaults.AuthenticationScheme)
                .AddRoundIdentity(authOptions =>
                {
                    authOptions.HowToObtainUsernameFromIdentityName = identityName => identityName;
                })
                .WithImpersonation(o =>
                {
                    o.Secret = configuration["Authentication:AuthSecret"];
                    o.KeepTheseClaimsDuringImpersonation = (claims) =>
                    {
                        return claims.ToList().Where(t => t.Type == RoundIdentityClaimType.Permission &&
                                                          t.Value == ProfilerPermissionClaim.CanProfile);
                    };
                })
                .WithMsSqlServer<UserInformationModel>(msSqlOption =>
                {
                    msSqlOption.ConnectionString = configuration.GetConnectionString("Db");
                    msSqlOption.ProfileConnection = true;
                });

            service.AddAuthorization(options =>
            {
                options.AddPolicy("HangfirePolicy", builder => builder
                    .AddAuthenticationSchemes(IISDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireClaim(RoundIdentityClaimType.Permission, Permissions.HangfireDashboard_Reader));
            });
        }

        public static void AddSwaggerAuthentication(this SwaggerGenOptions swaggerGenOptions, IConfiguration configuration)
        {
            switch (configuration["Authentication:Type"])
            {
                case "JWT":
                    swaggerGenOptions.AddSwaggerJWTAuthentication();
                    break;
                default:
                    break;
            }
        }

        private static void AddSwaggerJWTAuthentication(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }
    }
}
