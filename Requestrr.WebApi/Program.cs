using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Requestrr.WebApi.RequestrrBot;

namespace Requestrr.WebApi
{
    public class Program
    {
        public static int Port = 4545;
        
        public static void Main(string[] args)
        {
            if (!File.Exists(SettingsFile.FilePath))
            {
                File.WriteAllText(SettingsFile.FilePath, File.ReadAllText("SettingsTemplate.json").Replace("[PRIVATEKEY]", Guid.NewGuid().ToString()));
            }
            else
            {
                SettingsFileUpgrader.Upgrade();
            }

            if (!File.Exists(NotificationsFile.FilePath))
            {
                File.WriteAllText(NotificationsFile.FilePath, File.ReadAllText("NotificationsTemplate.json"));
            }

            Port = (int)SettingsFile.Read().Port;

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://*:{Port}")
                .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile(SettingsFile.FilePath, optional: false, reloadOnChange: true);
            });
    }
}
