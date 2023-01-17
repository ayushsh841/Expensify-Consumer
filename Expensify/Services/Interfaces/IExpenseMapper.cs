using Models.DbModels;
using Models.ServiceModels;
using MongoDB.Bson;

namespace Services.Interfaces
{
    /// <summary>
    /// This interface defines the method for 
    /// building the Expense model
    /// </summary>
    public interface IExpenseMapper
    {
        /// <summary>
        /// This method is used to map the given vendor id and 
        /// VendorExpenseDetails into Expense BSON Model
        /// </summary>
        /// <param name="vendorExpenseDetails">Expense details from file</param>
        /// <param name="vendorId">Associated vendor id</param>
        /// <returns>Expense Model</returns>
        public Expense Map(VendorExpenseDetails vendorExpenseDetails, ObjectId vendorId);
    }
}
