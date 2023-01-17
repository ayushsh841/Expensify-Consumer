using Microsoft.Extensions.Logging;
using Models.ServiceModels;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementation
{
    /// <summary>
    /// Manages the flow for adding vendors and 
    /// expenses from files
    /// </summary>
    public class ExpenseExtractionFlow : IExpenseExtractionFlow
    {
        /// <summary>
        /// This object returns the details from a file
        /// </summary>
        private readonly IFileProcessor _fileProcessor;

        /// <summary>
        /// This object manages the new vendors
        /// </summary>
        private readonly INewVendorManager _newVendorManager;

        /// <summary>
        /// This object manages the expenses
        /// </summary>
        private readonly INewExpenseManager _newExpenseManager;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="fileProcessor">Instance of File Processor object</param>
        /// <param name="newVendorManager">Instance of Vendor Manager object</param>
        /// <param name="newExpenseManager">Instance of Expense Manager object</param>
        public ExpenseExtractionFlow(
            IFileProcessor fileProcessor,
            INewVendorManager newVendorManager,
            INewExpenseManager newExpenseManager)
        {
            _fileProcessor = fileProcessor;
            _newVendorManager = newVendorManager;
            _newExpenseManager = newExpenseManager;
        }

        /// <summary>
        /// This methods manages the complete flow of adding new expense 
        /// and vendors from a given file
        /// </summary>
        /// <param name="fileName">Complete path to the file</param>
        /// <param name="logger">File Logger object</param>
        /// <returns>Returns if the process was successful or not.</returns>
        public async Task<bool> Extract(string fileName, ILogger logger)
        {
            bool vendorTask = false, expenseTask = false;
            List<VendorExpenseDetails> vendorExpenseDetails = new List<VendorExpenseDetails>();

            logger.LogInformation("Beginning to process the file");

            vendorExpenseDetails = _fileProcessor.Process<VendorExpenseDetails>(fileName);

            logger.LogInformation("File has been processed");

            if (vendorExpenseDetails.Count() > 0)
            {
                logger.LogInformation("Updating the Vendors");
                vendorTask = await _newVendorManager.AddVendors(vendorExpenseDetails, logger);

                if (vendorTask)
                {
                    logger.LogInformation("New Vendors successfully updated");
                    logger.LogInformation("Updating the Expenses");
                    expenseTask = await _newExpenseManager.AddExpenses(vendorExpenseDetails, logger);
                }
            }

            return vendorTask && expenseTask;
        }
    }
}
