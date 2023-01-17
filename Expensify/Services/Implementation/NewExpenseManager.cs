using Microsoft.Extensions.Logging;
using Models.DbModels;
using Models.ServiceModels;
using MongoDB.Bson;
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
    public class NewExpenseManager : INewExpenseManager
    {
        /// <summary>
        /// Holds the Vendor repository object
        /// </summary>
        private readonly IMongoRepository<Vendor> _vendorRepository;

        /// <summary>
        /// Holds the Expense repository object
        /// </summary>
        private readonly IMongoRepository<Expense> _expenseRepository;

        /// <summary>
        /// Holds the Expense mapper object
        /// </summary>
        private readonly IExpenseMapper _expenseMapper;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="vendorRepository">Instance of Vendor repository object</param>
        /// <param name="expenseRepository">Instance of Expense repository object</param>
        /// <param name="expenseMapper">Instance of Expense mapper object</param>
        public NewExpenseManager(
            IMongoRepository<Vendor> vendorRepository,
            IMongoRepository<Expense> expenseRepository,
            IExpenseMapper expenseMapper)
        {
            _vendorRepository = vendorRepository;
            _expenseRepository = expenseRepository;
            _expenseMapper = expenseMapper;
        }

        /// <summary>
        /// This method manages the flow adding new 
        /// expenses to the db
        /// </summary>
        /// <param name="expenses">expense list</param>
        /// <param name="logger">logger object</param>
        /// <returns>Returns if new expenses were 
        /// added successfully or not</returns>
        public async Task<bool> AddExpenses(List<VendorExpenseDetails> expenseDetails, ILogger logger)
        {
            List<Expense> expenses = new List<Expense>();
            Dictionary<string, ObjectId> vendors = new Dictionary<string, ObjectId>();

            logger.LogInformation("Getting the current list of Vendors");
            vendors = _vendorRepository.AsQueryable()
                                       .ToDictionary(x => x.Name, x => x.Id);

            logger.LogInformation("Building the expense List");
            foreach (var details in expenseDetails)
            {
                Expense expense = _expenseMapper.Map(details, vendors[details.Merchant]);

                expenses.Add(expense);
            }

            logger.LogInformation("Inserting the new Expenses");
            return await _expenseRepository.InsertManyAsync(expenses);
        }
    }
}
