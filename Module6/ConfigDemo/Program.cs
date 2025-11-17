using System;
using Microsoft.Extensions.Configuration;

namespace ConfigDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Read provider and connection string from appsettings.json
            var (provider, connectionString) = GetProviderFromConfiguration();

            Console.WriteLine($"Provider: {provider}");
            Console.WriteLine($"Connection String: {connectionString}");
        }

        // Helper method to load config values
        static (string Provider, string ConnectionString) GetProviderFromConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string provider = config["ProviderName"]!;
            string connString = config["SqlServer:ConnectionString"]!;
            return (provider, connString);
        }
    }
}