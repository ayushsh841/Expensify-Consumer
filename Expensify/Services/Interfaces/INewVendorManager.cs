using Microsoft.Extensions.Logging;
using Models.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    /// <summary>
    /// This interface defines the method to add 
    /// new vendors into db
    /// </summary>
    public interface INewVendorManager
    {
        /// <summary>
        /// This method manages the flow for identifying 
        /// new vendors and adding them to the list
        /// </summary>
        /// <param name="vendors">Vendors list</param>
        /// <param name="logger">logger object</param>
        /// <returns>Returns if new vendors were 
        /// added successfully or not</returns>
        Task<bool> AddVendors(List<VendorExpenseDetails> vendors, ILogger logger);
    }
}
