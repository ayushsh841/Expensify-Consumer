using Microsoft.Extensions.Logging;
using Models.ServiceModels;
using MongoDB.Driver.Core.Operations;
using Moq;
using Services.Implementation;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Services.Tests
{
    public class ExpenseExtractionFlowTest
    {
        private Mock<IFileProcessor> mockFileProcessor;
        private Mock<INewVendorManager> mockNewVendorManager;
        private Mock<INewExpenseManager> mockNewExpenseManager;
        private Mock<ILogger> mockLogger;
        private IExpenseExtractionFlow expenseExtractionFlow;

        public ExpenseExtractionFlowTest()
        {
            mockFileProcessor = new Mock<IFileProcessor>();
            mockNewVendorManager = new Mock<INewVendorManager>();
            mockNewExpenseManager = new Mock<INewExpenseManager>();
            mockLogger = new Mock<ILogger>();

            expenseExtractionFlow = new ExpenseExtractionFlow(mockFileProcessor.Object, 
                mockNewVendorManager.Object, mockNewExpenseManager.Object);
        }

        /// <summary>
        /// Given: A valid file name
        /// When: Extract Method is called
        /// Then: Returns True
        /// And: All methods are successfully called
        /// </summary>
        [Fact]
        public async Task Extract_GivenFileName_ReturnsTrue()
        {
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>()
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

            mockFileProcessor.Setup(x => x.Process<VendorExpenseDetails>("file")).Returns(vendorExpenseDetails);
            mockNewVendorManager.Setup(x => x.AddVendors(vendorExpenseDetails, mockLogger.Object)).ReturnsAsync(true);
            mockNewExpenseManager.Setup(x => x.AddExpenses(vendorExpenseDetails, mockLogger.Object)).ReturnsAsync(true);

            bool result = await expenseExtractionFlow.Extract("file", mockLogger.Object);

            Assert.True(result);
            mockFileProcessor.Verify(x => x.Process<VendorExpenseDetails>("file"), Times.Once);
            mockNewVendorManager.Verify(x => x.AddVendors(vendorExpenseDetails, mockLogger.Object), Times.Once);
            mockNewExpenseManager.Verify(x => x.AddExpenses(vendorExpenseDetails, mockLogger.Object), Times.Once);
        }

        /// <summary>
        /// Given: A valid file name
        /// And: File contains no details 
        /// When: Extract Method is called
        /// Then: Returns False
        /// And: No methods are called 
        /// </summary>
        [Fact]
        public async Task Extract_GivenFileName_FileContainsNoDetails_ReturnsFalse()
        {
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>();

            mockFileProcessor.Setup(x => x.Process<VendorExpenseDetails>("file")).Returns(vendorExpenseDetails);

            bool result = await expenseExtractionFlow.Extract("file", mockLogger.Object);

            Assert.False(result);
            mockFileProcessor.Verify(x => x.Process<VendorExpenseDetails>("file"), Times.Once);
            mockNewVendorManager.Verify(x => x.AddVendors(vendorExpenseDetails, mockLogger.Object), Times.Never);
            mockNewExpenseManager.Verify(x => x.AddExpenses(vendorExpenseDetails, mockLogger.Object), Times.Never);
        }

        /// <summary>
        /// Given: A valid file name
        /// When: Extract Method is called
        /// And: Vendor updation fails
        /// Then: Returns False
        /// And: AddVendors method is called
        /// And: AddExpenses method is not called
        /// </summary>
        [Fact]
        public async Task Extract_GivenFileName_UnableToUpdateVendors_ReturnsFalse()
        {
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>()
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

            mockFileProcessor.Setup(x => x.Process<VendorExpenseDetails>("file")).Returns(vendorExpenseDetails);
            mockNewVendorManager.Setup(x => x.AddVendors(vendorExpenseDetails, mockLogger.Object)).ReturnsAsync(false);

            bool result = await expenseExtractionFlow.Extract("file", mockLogger.Object);

            Assert.False(result);
            mockFileProcessor.Verify(x => x.Process<VendorExpenseDetails>("file"), Times.Once);
            mockNewVendorManager.Verify(x => x.AddVendors(vendorExpenseDetails, mockLogger.Object), Times.Once);
            mockNewExpenseManager.Verify(x => x.AddExpenses(vendorExpenseDetails, mockLogger.Object), Times.Never);
        }
    }
}
