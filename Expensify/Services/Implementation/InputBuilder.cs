using Common;
using Microsoft.Extensions.Configuration;
using Models.ServiceModels;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services.Implementation
{
    /// <summary>
    /// This class implements the method for 
    /// building the JobRequest object
    /// </summary>
    public class InputBuilder : IInputBuilder
    {
        private IConfiguration config;

        public InputBuilder(IConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// This method is responsible for building the 
        /// JobRequest model
        /// </summary>
        /// <param name="filters">Filters to be added</param>
        /// <returns>Job Request model</returns>
        public JobRequest Build(Dictionary<string, string> filters)
        {
            JobRequest jobRequest = new JobRequest();
            jobRequest.type = Constants.RequestType;
            jobRequest.credentials = new Credentials()
            {
                partnerUserID = config.GetValue<string>(Constants.UserName),
                partnerUserSecret = config.GetValue<string>(Constants.UserSecret)
            };
            jobRequest.onReceive = new ReceiveActions();
            jobRequest.inputSettings = new InputSettings()
            {
                type = config.GetValue<string>(Constants.InputType),
                filters = filters,
            };
            jobRequest.outputSettings = new OutputSetting();

            return jobRequest;
        }
    }
}
