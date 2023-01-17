using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Implementation;
using Services.Interfaces;
using System.Collections.Generic;
using System;
using Xunit;
using Models.ServiceModels;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Services.Tests
{
    public class JobCreatorTest
    {
        private Mock<ILogger> mockLogger;
        private IConfiguration config;
        private Mock<IInputBuilder> mockInputBuilder;
        private Mock<IApiHelper> mockApiHelper;
        private IJobCreator jobCreator;

        public JobCreatorTest()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Duration", "3"},
            };

            config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            mockLogger = new Mock<ILogger>();
            mockInputBuilder = new Mock<IInputBuilder>();
            mockApiHelper = new Mock<IApiHelper>();
            jobCreator = new JobCreator(mockLogger.Object,
                                        config,
                                        mockInputBuilder.Object,
                                        mockApiHelper.Object);
        }

        /// <summary>
        /// When: Create method is called
        /// Then: Returns the file name 
        /// </summary>
        [Fact]
        public async Task Create_WhenCalled_ReturnsJobFileAsync()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"startDate", DateTime.Now.AddDays(3 * -1).ToString("yyyy-MM-dd") },
                {"endDate", DateTime.Now.ToString("yyyy-MM-dd") },
            };
            JobRequest request = new JobRequest();
            string function = "?requestJobDescription=" + JsonConvert.SerializeObject(request);
            string postBody = File.ReadAllText("Template.txt");

            mockInputBuilder.Setup(x => x.Build(dict)).Returns(request);
            mockApiHelper.Setup(x => x.PostResultAsync<string>(function, postBody)).ReturnsAsync("test_file");

            string file = await jobCreator.Create();

            Assert.Equal("test_file", file);
        }
    }
}
