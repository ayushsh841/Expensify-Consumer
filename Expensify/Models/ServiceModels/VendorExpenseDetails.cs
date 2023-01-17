using System.Text.Json.Serialization;

namespace Models.ServiceModels
{
    /// <summary>
    /// This class defines the properties 
    /// for the file returned from the expensify jobs
    /// </summary>
    public class VendorExpenseDetails
    {
        /// <summary>
        /// Holds the merchant name
        /// </summary>
        [JsonPropertyName("Merchant")]
        public string Merchant { get; set; }

        /// <summary>
        /// Holds the amount 
        /// </summary>
        [JsonPropertyName("Amount")]
        public double Amount { get; set; }

        /// <summary>
        /// Holds the amount 
        /// </summary>
        [JsonPropertyName("ConvertedAmount")]
        public double ConvertedAmount { get; set; }

        /// <summary>
        /// Holds the curreny 
        /// </summary>
        [JsonPropertyName("Currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Holds the curreny 
        /// </summary>
        [JsonPropertyName("ModifiedAmount")]
        public double ModifiedAmount { get; set; }

        /// <summary>
        /// Holds the curreny 
        /// </summary>
        [JsonPropertyName("TaxAmount")]
        public double TaxAmount { get; set; }

        /// <summary>
        /// Holds the Category
        /// </summary>
        [JsonPropertyName("Category")]
        public string Category { get; set; }

        /// <summary>
        /// Holds the type of expense
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; }
    }
}
