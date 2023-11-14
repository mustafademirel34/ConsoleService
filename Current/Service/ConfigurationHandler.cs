
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;

namespace ConsoleService.Service
{

    public class ConfigurationHandler
    {
        private IConfigurationRoot _configuration;

        public ConfigurationHandler(string configFile = "appsettings.json")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath("C:/ConsoleService")
                .Add(new JsonConfigurationSource { Path = configFile, ReloadOnChange = true });

            _configuration = builder.Build();
        }

        public string GetSetting(string key)
        {
            return _configuration[key];
        }

        

        public void AddOrUpdateSetting(string key, string value)
        {
            // Yapılandırma dosyasını yeniden yükleme işlemi
            var builder = new ConfigurationBuilder()
                .SetBasePath("C:/ConsoleService")
                .AddJsonFile("appsettings.json");

            var newConfig = builder.Build();

            var settings = newConfig.GetSection("AppSettings");
            settings[key] = value;

            // Yeni yapılandırma dosyasını kaydetme
            File.WriteAllText("appsettings.json", newConfig.GetDebugView());
        }

        public void RemoveSetting(string key)
        {
            // Yapılandırma dosyasını yeniden yükleme işlemi
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var newConfig = builder.Build();

            var settings = newConfig.GetSection("AppSettings");
            settings[key] = null;

            // Yeni yapılandırma dosyasını kaydetme
            File.WriteAllText("appsettings.json", newConfig.GetDebugView());
        }



    }

}