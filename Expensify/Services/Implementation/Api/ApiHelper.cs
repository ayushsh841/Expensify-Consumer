using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ApiHelper : IApiHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHelper"/> class.
        /// </summary>
        /// <param name="_config">_configuration</param>
        public ApiHelper(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Gets or sets API _config
        /// </summary>
        private IConfiguration _config;

        /// <summary>
        /// Gets or sets API _config
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Does a GET request
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="function">function name to call</param>
        /// <param name="_logger">_logger</param>
        /// <returns>Task</returns>
        public async Task<T> GetResultAsync<T>(string function)
        {
            try
            {
                using (var response = await GetHttpClient().GetAsync(_config.GetValue<string>("url:Expensify") + function))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(response.ReasonPhrase);
                    }

                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(apiResponse);

                    _logger.LogInformation($"API GET Call successful for {0}", function);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($" {0}: API GET Call failed for {1}, Reason: {2}", ex.Message, function, ex.StackTrace);

                return default(T);
            }
        }

        /// <summary>
        /// Does a HTTP POST response
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="function">Function name</param>
        /// <param name="postBody">List of post params</param>
        /// <returns>Task with result</returns>
        public async Task<T> PostResultAsync<T>(string function, string postBody)
        {
            try
            {
                HttpContent content = new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded");

                using (var response = await GetHttpClient().PostAsync(_config.GetValue<string>(Constants.ExpensifyUrl) + function, content).ConfigureAwait(false))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (typeof(T).IsValueType || typeof(T) == typeof(string)) 
                    {
                        return (T)Convert.ChangeType(apiResponse, typeof(T));
                    }

                    var result = JsonConvert.DeserializeObject<T>(apiResponse);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($" {0}: API POST Call failed for {1}, Reason: {2}", ex.Message, function, ex.StackTrace);

                return default(T);
            }
        }

        /// <summary>
        /// Returns a new Client
        /// </summary>
        /// <returns>HttpClient object</returns>
        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();

            return httpClient;
        }
    }
}
