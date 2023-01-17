using Microsoft.Extensions.Logging;
using Models.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    /// <summary>
    /// This interface defines the method to add 
    /// new expenses into db
    /// </summary>
    public interface INewExpenseManager
    {
        /// <summary>
        /// This method manages the flow adding new 
        /// expenses to the db
        /// </summary>
        /// <param name="expenses">expense list</param>
        /// <param name="logger">logger object</param>
        /// <returns>Returns if new expenses were 
        /// added successfully or not</returns>
        Task<bool> AddExpenses(List<VendorExpenseDetails> expenses, ILogger logger);
    }
}
