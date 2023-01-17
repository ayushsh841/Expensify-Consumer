using Microsoft.Extensions.Logging;
using Models.DbModels;
using Models.ServiceModels;
using Moq;
using Repository.Base;
using Services.Implementation;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Services.Tests
{
    public class NewExpenseManagerTest
    {
        private Mock<IMongoRepository<Vendor>> mockVendorRepository;
        private Mock<IMongoRepository<Expense>> mockExpenseRepository;
        private Mock<IExpenseMapper> mockExpenseMapper;
        private Mock<ILogger> mockLogger;
        private INewExpenseManager newExpenseManager;

        public NewExpenseManagerTest()
        {
            mockVendorRepository = new Mock<IMongoRepository<Vendor>>();
            mockExpenseRepository = new Mock<IMongoRepository<Expense>>();
            mockExpenseMapper = new Mock<IExpenseMapper>();
            mockLogger = new Mock<ILogger>();

            newExpenseManager = new NewExpenseManager(mockVendorRepository.Object,
                mockExpenseRepository.Object, mockExpenseMapper.Object);
        }

        /// <summary>
        /// Given: Expense details are provided
        /// When: AddExpenses method is called
        /// Then: AddExpenses method returns true
        /// And: All methods are called as expected
        /// </summary>
        [Fact]
        public async void AddExpenses_GivenExpenseDetails_ExpensesAreSaved()
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

            List<Vendor> vendors = new List<Vendor>()
            {
                new Vendor() { Name = "Merchant 1" },
                new Vendor() { Name = "Merchant 2" }
            };

            List<Expense> expenses = new List<Expense>()
            {
                new Expense() { VendorId = vendors[0].Id, Cost = 150.00, Currency = "INR" },
                new Expense() { VendorId = vendors[1].Id, Cost = 10.00, Currency = "INR" }
            };

            mockVendorRepository.Setup(x => x.AsQueryable()).Returns(vendors.AsQueryable());
            mockExpenseMapper.Setup(x => x.Map(vendorExpenseDetails[0], vendors[0].Id)).Returns(expenses[0]);
            mockExpenseMapper.Setup(x => x.Map(vendorExpenseDetails[1], vendors[1].Id)).Returns(expenses[1]);
            mockExpenseRepository.Setup(x => x.InsertManyAsync(It.Is<List<Expense>>(x =>
                x.All(y => y.VendorId == expenses[0].VendorId || y.VendorId == expenses[1].VendorId &&
                           y.Cost == expenses[0].Cost || y.Cost == expenses[1].Cost &&
                           y.Currency == expenses[0].Currency || y.Currency == expenses[1].Currency)
            ))).ReturnsAsync(true);

            bool result = await newExpenseManager.AddExpenses(vendorExpenseDetails, mockLogger.Object);

            Assert.True(result);
            mockVendorRepository.Verify(x => x.AsQueryable());
            mockExpenseMapper.Verify(x => x.Map(vendorExpenseDetails[0], vendors[0].Id), Times.Once);
            mockExpenseMapper.Verify(x => x.Map(vendorExpenseDetails[1], vendors[1].Id), Times.Once);
            mockExpenseRepository.Verify(x => x.InsertManyAsync(It.Is<List<Expense>>(x =>
                x.All(y => y.VendorId == expenses[0].VendorId || y.VendorId == expenses[1].VendorId &&
                           y.Cost == expenses[0].Cost || y.Cost == expenses[1].Cost &&
                           y.Currency == expenses[0].Currency || y.Currency == expenses[1].Currency)
            )), Times.Once);
        }

        /// <summary>
        /// Given: Expense details are provided
        /// When: AddExpenses method is called
        /// Then: AddExpenses method returns false
        /// And: All methods are called as expected
        /// </summary>
        [Fact]
        public async void AddExpenses_GivenExpenseDetails_DbExceptionOccurs_ReturnsFalse()
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

            List<Vendor> vendors = new List<Vendor>()
            {
                new Vendor() { Name = "Merchant 1" },
                new Vendor() { Name = "Merchant 2" }
            };

            List<Expense> expenses = new List<Expense>()
            {
                new Expense() { VendorId = vendors[0].Id, Cost = 150.00, Currency = "INR" },
                new Expense() { VendorId = vendors[1].Id, Cost = 10.00, Currency = "INR" }
            };

            mockVendorRepository.Setup(x => x.AsQueryable()).Returns(vendors.AsQueryable());
            mockExpenseMapper.Setup(x => x.Map(vendorExpenseDetails[0], vendors[0].Id)).Returns(expenses[0]);
            mockExpenseMapper.Setup(x => x.Map(vendorExpenseDetails[1], vendors[1].Id)).Returns(expenses[1]);
            mockExpenseRepository.Setup(x => x.InsertManyAsync(It.Is<List<Expense>>(x =>
                x.All(y => y.VendorId == expenses[0].VendorId || y.VendorId == expenses[1].VendorId &&
                           y.Cost == expenses[0].Cost || y.Cost == expenses[1].Cost &&
                           y.Currency == expenses[0].Currency || y.Currency == expenses[1].Currency)
            ))).ReturnsAsync(false);

            bool result = await newExpenseManager.AddExpenses(vendorExpenseDetails, mockLogger.Object);

            Assert.False(result);
            mockVendorRepository.Verify(x => x.AsQueryable());
            mockExpenseMapper.Verify(x => x.Map(vendorExpenseDetails[0], vendors[0].Id), Times.Once);
            mockExpenseMapper.Verify(x => x.Map(vendorExpenseDetails[1], vendors[1].Id), Times.Once);
            mockExpenseRepository.Verify(x => x.InsertManyAsync(It.Is<List<Expense>>(x =>
                x.All(y => y.VendorId == expenses[0].VendorId || y.VendorId == expenses[1].VendorId &&
                           y.Cost == expenses[0].Cost || y.Cost == expenses[1].Cost &&
                           y.Currency == expenses[0].Currency || y.Currency == expenses[1].Currency)
            )), Times.Once);
        }
    }
}
