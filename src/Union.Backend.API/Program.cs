using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Union.Backend.API
{
    public class Program
    {
        public readonly static string DB_CONTEXT_ARG = "DbContext";
        private static readonly Dictionary<string, string> argsMapping = new Dictionary<string, string>
        {
            { "-db", DB_CONTEXT_ARG }
        };

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCommandLine(args, argsMapping);
                })
                .UseStartup<Startup>();
    }
}
