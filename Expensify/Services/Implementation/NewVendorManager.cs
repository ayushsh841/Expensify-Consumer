using Common;
using Microsoft.Extensions.Logging;
using Models.DbModels;
using Models.ServiceModels;
using Repository.Base;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementation
{
    /// <summary>
    /// This interface defines the method to add 
    /// new vendors into db
    /// </summary>
    public class NewVendorManager : INewVendorManager
    {
        /// <summary>
        /// Holds the Vendor repository object
        /// </summary>
        private readonly IMongoRepository<Vendor> _vendorRepository;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="vendorRepository">Vendor repository object</param>
        public NewVendorManager(IMongoRepository<Vendor> vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        /// <summary>
        /// This method manages the flow for identifying 
        /// new vendors and adding them to the list
        /// </summary>
        /// <param name="vendors">Vendors list</param>
        /// <param name="logger">logger object</param>
        /// <returns>Returns if new vendors were 
        /// added successfully or not</returns>
        public async Task<bool> AddVendors(List<VendorExpenseDetails> vendorExpenseDetails, ILogger logger)
        {
            List<Vendor> newVendors = new List<Vendor>();

            logger.LogInformation("Indentifying the vendors");
            List<Vendor> vendors = vendorExpenseDetails
                .Select(x => new Vendor()
                {
                    Name = x.Merchant
                })
                .Distinct()
                .ToList();

            logger.LogInformation("Fetching the existing vendors");
            List<Vendor> existingVendors = _vendorRepository
                .AsQueryable()
                .ToList();

            if (existingVendors.Count() > 0)
            {
                logger.LogInformation("Filtering the new vendors");

                newVendors = vendors.Where(l1 => existingVendors.All(l2 => l2.Name != l1.Name))
                                    .ToList();

                logger.LogInformation("Total New Vendors: {count}", newVendors.Count());
            }
            else
            {
                newVendors = vendors;
            }

            if (newVendors.Count() > 0)
            {
                logger.LogInformation("Inserting the new Vendors");
                return await _vendorRepository.InsertManyAsync(newVendors);
            }

            return true;
        }
    }
}
