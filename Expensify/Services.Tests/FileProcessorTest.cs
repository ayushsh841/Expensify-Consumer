using Models.ServiceModels;
using Services.Implementation;
using Services.Interfaces;
using Services.Tests.Utils;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Services.Tests
{
    public class FileProcessorTest
    {
        private IFileProcessor fileProcessor;

        public FileProcessorTest()
        {
            fileProcessor = new FileProcessor();
        }

        /// <summary>
        /// Given: The file name 
        /// When: Process method is called
        /// Then: Returns string list 
        /// </summary>
        [Fact]
        public void Process_GivenFile_ReturnsExpectedStringList()
        {
            string path = Directory.GetCurrentDirectory() + "\\MockData\\StringData.txt";
            List<string> expectedResult = new List<string>()
            {
                "dummy_data1", "dummy_data2", "dummy_data3"
            };

            List<string> result = fileProcessor.Process<string>(path);

            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Given: The file name 
        /// When: Process method is called
        /// Then: Returns VendorExpenseDetails list 
        /// </summary>
        [Fact]
        public void Process_GivenFile_ReturnsExpectedVendorExpenseDetails()
        {
            string path = Directory.GetCurrentDirectory() + "\\MockData\\ExpenseData.txt";
            List<VendorExpenseDetails> expectedResult = new List<VendorExpenseDetails>()
            {
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 1",
                    Amount = 150.00,
                    Currency = "INR",
                },
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 2",
                    Amount = 10.00,
                    Currency = "INR",
                }
            };

            List<VendorExpenseDetails> result = fileProcessor.Process<VendorExpenseDetails>(path);

            Assert.Equal(expectedResult, result, new GenericEqualityComparer<VendorExpenseDetails>());
        }
    }
}
