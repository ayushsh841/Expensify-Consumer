using Microsoft.Extensions.Configuration;
using Models.ServiceModels;
using Services.Implementation;
using Services.Interfaces;
using Services.Tests.Utils;
using System;
using System.Collections.Generic;
using Xunit;

namespace Services.Tests
{
    public class InputBuilderTest
    {
        private IConfiguration configuration;
        private IInputBuilder builder;

        public InputBuilderTest()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Credentials:UserId", "TestUser"},
                {"Credentials:UserSecret", "TestSecret"},
                {"Input:type", "TestType"},
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            builder = new InputBuilder(configuration);
        }

        /// <summary>
        /// Given: Data filters are provided
        /// When: Build method is called
        /// Then: Returns the expected model
        /// </summary>
        [Fact]
        public void Build_GivenFilters_ReturnsExpectedModel()
        {
            Dictionary<string, string> filters = new Dictionary<string, string>()
            {
                {"startDate", DateTime.Now.AddDays(3 * -1).ToString("yyyy-MM-dd") },
                {"endDate", DateTime.Now.ToString("yyyy-MM-dd") },
            };
            JobRequest expectedRequest = new JobRequest()
            {
                type = "file",
                credentials = new Credentials()
                {
                    partnerUserID = "TestUser",
                    partnerUserSecret = "TestSecret"
                },
                onReceive = new ReceiveActions(),
                inputSettings = new InputSettings()
                {
                    type = "TestType",
                    filters = filters,
                },
                outputSettings = new OutputSetting()
            };

            JobRequest request = builder.Build(filters);

            Assert.Equal(expectedRequest, request, new GenericEqualityComparer<JobRequest>("type"));
            Assert.Equal(expectedRequest.credentials, request.credentials, new GenericEqualityComparer<Credentials>());
            Assert.Equal(expectedRequest.onReceive.immediateResponse[0], request.onReceive.immediateResponse[0]);
            Assert.Equal(expectedRequest.inputSettings, request.inputSettings, new GenericEqualityComparer<InputSettings>());
            Assert.Equal(expectedRequest.outputSettings, request.outputSettings, new GenericEqualityComparer<OutputSetting>());
        }
    }
}
