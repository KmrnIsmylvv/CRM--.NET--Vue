using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Elfo.Round.Localization;
using Microsoft.AspNetCore.Http;
using Elfo.Round.Identity;
using System.Linq;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class LocalizationExtensions
    {
        public static void AddRoundLocalization(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRoundLocalization(o =>
                {
                    o.Assembly = Assembly.Load("Elfo.Contoso.LearningRoundKamran.Infrastructure");
                    o.SetDefaultCulture("en")
                     .AddSupportedCultures("en", "it");
                    o.HowToObtainCurrentCulture = sp =>
                    {
                        var context = sp.GetRequiredService<IHttpContextAccessor>();
                        if (!context.HttpContext.User.HasClaim(RoundIdentityClaimType.UserLanguageCode))
                            return null;

                        var languageCodeClaim = context.HttpContext.User.Claims.First(x => x.Type == RoundIdentityClaimType.UserLanguageCode);
                        return languageCodeClaim.Value;
                    };
                });
        }
    }
}
