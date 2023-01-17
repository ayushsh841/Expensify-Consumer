using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace CloudEagle.ExpenseCollector
{
    public partial class ExpenseCollectorEntryPoint
    {
        /// <summary>
        /// Defines the logger factory.
        /// </summary>
        private static ILoggerFactory loggerFactory;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// Defines the app config.
        /// </summary>
        private static IConfiguration appConfig;

        /// <summary>
        /// Defines host.
        /// </summary>
        private static IHost host;

        /// <summary>
        /// Gets or sets RetryAttempt value.
        /// </summary>
        private static int retryAttempt = 5;

        /// <summary>
        /// Configure host with retries.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void ConfigureHostWithDependencies(string[] args)
        {
            try
            {
                ConfigureHost(args);
            }
            catch (Exception ex)
            {
                logger.LogError($"FATAL Error: Exception: {ex}");

                throw ex;
            }

            logger.LogInformation("Host configuration attempt was successful.");
        }

        /// <summary>
        /// The Main method.
        /// </summary>
        /// <param name="args">The args passed by the user.</param>
        public static void Main(string[] args)
        {
            try
            {
                // Load host settings.
                LoadHostConfiguration(args);

                // Configure logging.
                ConfigureLogging(args);

                logger.LogInformation("Configuring host and app configs.");

                // Configure host and logging options.
                ConfigureHostWithDependencies(args);

                logger.LogInformation("Configuring host and app configs done.");

                host.Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical($"ResultCollection encountered an unknown error: {ex}");
            }
        }
    }
}
