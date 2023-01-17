﻿using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Implementation;
using Services.Interfaces;
using System.IO;

namespace CloudEagle.ExpenseCollector
{
    public partial class ExpenseCollectorEntryPoint
    {
        /// <summary>
        /// Load initial configuration so that values from initial app settings can be used in other configurations.
        /// </summary>
        /// <param name="args">The args passed by the user.</param>
        private static void LoadHostConfiguration(string[] args)
        {
            // Load initial configuration so that values from initial app settings can be used in other configurations.
            IHost h = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile(Constants.AppSettings, optional: false);
                    if (args != null)
                    {
                        configHost.AddCommandLine(args);
                    }
                })
                .Build();

            appConfig = h.Services.GetService<IConfiguration>();
        }

        /// <summary>
        /// Configure HostBuilder with appConfig and logging.
        /// </summary>
        private static void ConfigureHost(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder();

            // Configure host settings.
            hostBuilder.ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
            });

            // Configure app settings.
            hostBuilder.ConfigureAppConfiguration((hostContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddConfiguration(appConfig);
            });

            // Configure command line.
            hostBuilder.ConfigureAppConfiguration((hostContext, config) =>
            {
                if (args != null && args.Length > 0)
                {
                    config.AddCommandLine(args);
                }
            });

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();

                services.AddTransient<IFileProcessor, FileProcessor>();
                services.AddTransient<IJobCreator, JobCreator>();
                services.AddTransient<IInputBuilder, InputBuilder>();
                services.AddTransient<IApiHelper, ApiHelper>();

                services.AddSingleton(logger);
            });

            // Build hostbuilder.
            host = hostBuilder.Build();

            // Get configuration.
            appConfig = host.Services.GetService<IConfiguration>();
        }

        /// <summary>
        /// Configure app logging.
        /// </summary>
        /// <param name="args">The args passed by the user.</param>
        private static void ConfigureLogging(string[] args)
        {
            loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConfiguration(appConfig.GetSection("Logging"));
                logging.AddConsole(configure =>
                {
                    configure.TimestampFormat = Constants.LogsDateTimeFormat;
                });
                logging.AddDebug();
                string logFileName = string.Format("Logs/{0}-{1}.log", Constants.ServiceName, "{Date}");
                logging.AddFile(logFileName, isJson: true);
            });

            logger = loggerFactory.CreateLogger(Constants.ServiceName);
        }
    }
}
