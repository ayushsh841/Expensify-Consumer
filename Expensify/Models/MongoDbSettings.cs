namespace Models
{
    /// <summary>
    /// This class holds the mongo db settings 
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// Holds the Connection string name
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Holds the database name
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
