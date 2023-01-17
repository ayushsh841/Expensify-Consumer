using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudEagle.ExpenseCollector
{
    /// <summary>
    /// This class handles the process of creating a job 
    /// through expensify API
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// Holds the logger object
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Holds the Job Creator object
        /// </summary>
        private readonly IJobCreator _jobCreator;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="logger">Instance of logger object</param>
        /// <param name="jobCreator">Instance of JobCreator object</param>
        public Worker(ILogger logger, IJobCreator jobCreator)
        {
            _logger = logger;
            _jobCreator = jobCreator;
        }

        /// <summary>
        /// This method starts the process of 
        /// creating a job
        /// </summary>
        /// <param name="stoppingToken">Cancellation token</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            string fileName = await _jobCreator.Create();

            _logger.LogInformation("The job generated the file: {name}", fileName);

            _logger.LogInformation("Worker stopped running at: {time}", DateTimeOffset.Now);

            Environment.Exit(0);
        }
    }
}
