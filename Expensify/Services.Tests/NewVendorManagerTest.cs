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
    public class NewVendorManagerTest
    {
        private Mock<IMongoRepository<Vendor>> mockMongoRepository;
        private Mock<ILogger> mockLogger;
        private INewVendorManager newVendorManager;

        public NewVendorManagerTest()
        {
            mockMongoRepository = new Mock<IMongoRepository<Vendor>>();
            mockLogger = new Mock<ILogger>();

            newVendorManager = new NewVendorManager(mockMongoRepository.Object);
        }

        /// <summary>
        /// Given: All vendors are new 
        /// When: Add Vendors method is called
        /// Then: AddVendors method returns true
        /// And: All the new Vendors are inserted in DB
        /// And: All methods are called as expected
        /// </summary>
        [Fact]
        public async void AddVendors_GivenAllNewVendors_NewVendorsAreInserted()
        {
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>() {
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 1",
                },
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 2",
                },
            };
            string[] vendors = new string[] { "Merchant 1", "Merchant 2" };
            List<Vendor> vendorsToInsert = new List<Vendor>()
            {
                new Vendor() { Name = "Merchant 1" },
                new Vendor() { Name = "Merchant 2" }
            };

            mockMongoRepository.Setup(x => x.AsQueryable()).Returns(new List<Vendor>().AsQueryable());
            mockMongoRepository.Setup(x => x.InsertManyAsync(It.Is<List<Vendor>>(x =>
                x.All(y => vendors.Contains(y.Name))
            ))).ReturnsAsync(true);

            bool result = await newVendorManager.AddVendors(vendorExpenseDetails, mockLogger.Object);

            Assert.True(result);
            mockMongoRepository.Verify(x => x.AsQueryable(), Times.Once);
            mockMongoRepository.Verify(x => x.InsertManyAsync(It.Is<List<Vendor>>(x =>
                x.All(y => vendors.Contains(y.Name))
            )), Times.Once);
        }

        /// <summary>
        /// Given: All vendors already exist in DB
        /// When: Add Vendors method is called
        /// Then: AddVendors method returns true
        /// And: No Vendors are inserted in DB
        /// And: AsQueryable method is called once 
        /// And: InsertManyAsync is not called
        /// </summary>
        [Fact]
        public async void AddVendors_GivenNoNewVendors_NoVendorsAreInserted()
        {
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>() {
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 1",
                },
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 2",
                },
            };
            List<Vendor> existingVendors = new List<Vendor>() {
                new Vendor() { Name = "Merchant 1" },
                new Vendor() { Name = "Merchant 2" }
            };

            mockMongoRepository.Setup(x => x.AsQueryable()).Returns(existingVendors.AsQueryable());

            bool result = await newVendorManager.AddVendors(vendorExpenseDetails, mockLogger.Object);

            Assert.True(result);
            mockMongoRepository.Verify(x => x.AsQueryable(), Times.Once);
            mockMongoRepository.Verify(x => x.InsertManyAsync(It.IsAny<List<Vendor>>()), Times.Never);
        }

        /// <summary>
        /// Given: Some vendors are new 
        /// When: Add Vendors method is called
        /// Then: AddVendors method returns true
        /// And: Only the new Vendors are inserted in DB
        /// And: All methods are called as expected
        /// </summary>
        [Fact]
        public async void AddVendors_GivenPartialNewVendors_OnlyNewVendorsAreInserted()
        {
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>() {
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 1",
                },
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 2",
                },
            };
            List<Vendor> existingVendors = new List<Vendor>() {
                new Vendor() { Name = "Merchant 1" },
            };
            List<Vendor> newVendors = new List<Vendor>() {
                new Vendor() { Name = "Merchant 2" },
            };

            mockMongoRepository.Setup(x => x.AsQueryable()).Returns(existingVendors.AsQueryable());
            mockMongoRepository.Setup(x => x.InsertManyAsync(It.Is<List<Vendor>>(x =>
                x[0].Name == "Merchant 2"
            ))).ReturnsAsync(true);

            bool result = await newVendorManager.AddVendors(vendorExpenseDetails, mockLogger.Object);

            Assert.True(result);
            mockMongoRepository.Verify(x => x.AsQueryable(), Times.Once);
            mockMongoRepository.Verify(x => x.InsertManyAsync(It.Is<List<Vendor>>(x =>
                x[0].Name == "Merchant 2"
            )), Times.Once);
        }

        /// <summary>
        /// Given: Some vendors are new 
        /// When: Add Vendors method is called
        /// Then: AddVendors method returns false
        /// And: All methods are called as expected
        /// </summary>
        [Fact]
        public async void AddVendors_GivenVendors_DbExceptionOccurs_ReturnFalse()
        {
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>() {
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 1",
                },
                new VendorExpenseDetails()
                {
                    Merchant = "Merchant 2",
                },
            };
            List<Vendor> existingVendors = new List<Vendor>() {
                new Vendor() { Name = "Merchant 1" },
            };
            List<Vendor> newVendors = new List<Vendor>() {
                new Vendor() { Name = "Merchant 2" },
            };

            mockMongoRepository.Setup(x => x.AsQueryable()).Returns(existingVendors.AsQueryable());
            mockMongoRepository.Setup(x => x.InsertManyAsync(It.Is<List<Vendor>>(x =>
                x[0].Name == "Merchant 2"
            ))).ReturnsAsync(false);

            bool result = await newVendorManager.AddVendors(vendorExpenseDetails, mockLogger.Object);

            Assert.False(result);
        }
    }
}
