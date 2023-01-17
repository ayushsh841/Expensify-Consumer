using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models.ServiceModels;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.Implementation
{
    /// <summary>
    /// This class implements the method for creating 
    /// a job request
    /// </summary>
    public class JobCreator : IJobCreator
    {
        /// <summary>
        /// Holds the logger object
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Holds the Configuration object
        /// </summary>
        private IConfiguration _config;

        /// <summary>
        /// Holds the InputBuilder object
        /// </summary>
        private IInputBuilder _inputBuilder;

        /// <summary>
        /// Holds the ApiHelper object
        /// </summary>
        private IApiHelper _apiHelper;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="logger">Instance of logger object<param>
        /// <param name="config">Instance of configuration object<param>
        /// <param name="inputBuilder">Instance of input builder object</param>
        /// <param name="apiHelper">Instance of ApiHelper object</param>
        public JobCreator(
            ILogger logger,
            IConfiguration config,
            IInputBuilder inputBuilder,
            IApiHelper apiHelper)
        {
            _logger = logger;
            _config = config;
            _inputBuilder = inputBuilder;
            _apiHelper = apiHelper;
        }

        /// <summary>
        /// This method is responsible for creating a job
        /// </summary>
        /// <returns>FileName for the job result. </returns>
        public async Task<string> Create()
        {
            _logger.LogInformation("Creating the filter");

            int duration = _config.GetValue<int>(Constants.Duration);
            // Set the duration 
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"startDate", DateTime.Now.AddDays(duration * -1).ToString(Constants.ApiDateFormat) },
                {"endDate", DateTime.Now.ToString(Constants.ApiDateFormat) },
            };

            _logger.LogInformation("Building the request");

            // This should include the details for SMTP server
            JobRequest request = _inputBuilder.Build(dict);

            _logger.LogInformation("Building the api request");
            string function = Constants.ApiRequest + JsonConvert.SerializeObject(request);

            string postBody = File.ReadAllText(Constants.TemplateFile);

            _logger.LogInformation("Calling the API");
            var fileName = await _apiHelper.PostResultAsync<string>(function, postBody);

            _logger.LogInformation($"API POST successful for {0}", function);

            return fileName;
        }
    }
}
