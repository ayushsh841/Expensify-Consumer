using Models.DbModels;
using Models.ServiceModels;
using MongoDB.Bson;
using Services.Interfaces;

namespace Services.Implementation
{   
    /// <summary>
    /// This class implements the method for 
    /// building the Expense model
    /// </summary>
    public class ExpenseMapper : IExpenseMapper
    {
        /// <summary>
        /// This method is used to map the given vendor id and 
        /// VendorExpenseDetails into Expense BSON Model
        /// </summary>
        /// <param name="vendorExpenseDetails">Expense details from file</param>
        /// <param name="vendorId">Associated vendor id</param>
        /// <returns>Expense Model</returns>
        public Expense Map(VendorExpenseDetails vendorExpenseDetails, ObjectId vendorId)
        {
            Expense expense = new Expense()
            {
                VendorId = vendorId,
                Category = vendorExpenseDetails.Category,
                Cost = vendorExpenseDetails.Amount,
                ConvertedAmount = vendorExpenseDetails.ConvertedAmount,
                Currency = vendorExpenseDetails.Currency,
                ModifiedAmount = vendorExpenseDetails.ModifiedAmount,
                TaxAmount = vendorExpenseDetails.TaxAmount,
                Type = vendorExpenseDetails.Type
            };

            return expense;
        }
    }
}
