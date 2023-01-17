using Models.DbModels;
using Models.ServiceModels;
using MongoDB.Bson;
using MongoDB.Driver.Core.Operations;
using Services.Implementation;
using Services.Interfaces;
using Services.Tests.Utils;
using Xunit;

namespace Services.Tests
{
    public class ExpenseMapperTest
    {
        private IExpenseMapper expenseMapper;

        public ExpenseMapperTest()
        {
            expenseMapper = new ExpenseMapper();
        }

        /// <summary>
        /// Given: Expense details are provided
        /// When: Map method is called
        /// Then: Returns the Expense BSON model
        /// </summary>
        [Fact]
        public void Map_GivenDetails_ReturnsExpectedModel()
        {
            VendorExpenseDetails details = new VendorExpenseDetails()
            {
                Amount = 100.00,
                Category = "Test",
                ConvertedAmount = 100.00,
                Currency = "INR",
                Merchant = "Amazon",
                ModifiedAmount = 100.00,
                TaxAmount = 5.00,
                Type = "Cloud"
            };

            ObjectId id = new ObjectId();

            Expense expectedExpense = new Expense()
            {
                Category = "Test",
                Type = "Cloud",
                ConvertedAmount = 100.00,
                ModifiedAmount = 100.00,
                TaxAmount = 5.00,
                Currency = "INR",
                Cost = 100.00,
                VendorId = id
            };

            Expense expense = expenseMapper.Map(details, id);

            Assert.Equal(expectedExpense, expense, new GenericEqualityComparer<Expense>());
        }
    }
}
