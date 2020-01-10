using Microsoft.Extensions.Configuration;

namespace Infi.DojoEventSourcing.Configuration
{
    public static class ConfigurationFactory
    {
        public static IConfigurationRoot Create()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        }
    }
}