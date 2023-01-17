using Models.CustomAttributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Models.DbModels
{
    /// <summary>
    /// This class defines the properties
    /// for a Expense document
    /// </summary>
    [BsonCollection("Expenses")]
    public class Expense : Document
    {
        /// <summary>
        /// Holds the Unique Id of the Vendor document
        /// </summary>
        public ObjectId VendorId { get; set; }

        /// <summary>
        /// Holds the cost incurred
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Holds the amount 
        /// </summary>
        public double ConvertedAmount { get; set; }

        /// <summary>
        /// Holds the curreny 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Holds the curreny 
        /// </summary>
        public double ModifiedAmount { get; set; }

        /// <summary>
        /// Holds the curreny 
        /// </summary>
        public double TaxAmount { get; set; }

        /// <summary>
        /// Holds the category of the expense
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Holds the type of the expense
        /// </summary>
        public string Type { get; set; }
    }
}
