using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;

namespace Elfo.Contoso.LearningRoundKamran.Api
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var host = BuildHost(args);
            await host.RunAsync();

            return 0;
        }

        public static IHost BuildHost(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                           .ConfigureWebHostDefaults(webBuilder =>
                           {
                               webBuilder.UseStartup<Startup>();

                               if (args.Contains("--httpsyswinauth"))
                               {
                                   webBuilder.UseHttpSys(options =>
                                   {
                                       options.Authentication.Schemes =
                                           AuthenticationSchemes.NTLM | AuthenticationSchemes.Negotiate;
                                       options.Authentication.AllowAnonymous = true;
                                   });
                               }
                           })
                           .UseSerilog();

            return host.Build();
        }
    }
}
