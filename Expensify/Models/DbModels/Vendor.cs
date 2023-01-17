using Models.CustomAttributes;

namespace Models.DbModels
{
    /// <summary>
    /// This class defines the properties of the 
    /// a vendor object
    /// </summary>
    [BsonCollection("Vendors")]
    public class Vendor : Document
    {
        /// <summary>
        /// Holds the name of the vendor
        /// </summary>
        public string Name { get; set; }
    }
}
