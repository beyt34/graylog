using System;
using System.Reflection;
using Gelf.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace Graylog.SampleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((context, builder) => builder.AddGelf(options =>
                {
                    // Optional customization applied on top of settings in Logging:GELF configuration section.
                    options.LogSource = context.HostingEnvironment.ApplicationName;
                    options.AdditionalFields["machine_name"] = Environment.MachineName;
                    options.AdditionalFields["app_version"] = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
                }))
                .UseNLog(); // NLog: Setup NLog for Dependency injection;
        }
    }
}