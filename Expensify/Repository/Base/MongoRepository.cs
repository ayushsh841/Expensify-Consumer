using Microsoft.Extensions.Options;
using Models;
using Models.CustomAttributes;
using Models.DbModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Base
{
    /// <summary>
    /// This class implements the generic methods for 
    /// the Mongo Db
    /// </summary>
    /// <typeparam name="TDocument">Document Type</typeparam>
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
    where TDocument : IDocument
    {
        /// <summary>
        /// Holds the collection instance
        /// </summary>
        private readonly IMongoCollection<TDocument> _collection;

        /// <summary>
        /// Holds the mongo client
        /// </summary>
        private readonly IMongoClient _client;

        /// <summary>
        /// Holds the database object
        /// </summary>
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Holds the mongo db settings object
        /// </summary>
        private readonly MongoDbSettings _mongoDbSettings;
        
        /// <summary>
        /// Cosntructor to initialize dependencies
        /// </summary>
        /// <param name="mongoDbSettings">Options class for reading 
        /// db settings</param>
        public MongoRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _client = new MongoClient(_mongoDbSettings.ConnectionString);
            _database = _client.GetDatabase(_mongoDbSettings.DatabaseName);
            _collection = _database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        /// <summary>
        /// This method get the collection name
        /// </summary>
        /// <param name="documentType">Generic document</param>
        /// <returns>Name of the collection the document type
        /// belongs to </returns>
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        /// <summary>
        /// Returns all documents from a collection
        /// </summary>
        /// <returns>ICollection object of the documents</returns>
        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        /// <summary>
        /// This method inserts a list of objects into DB
        /// </summary>
        /// <param name="documents">Generic document object</param>
        /// <returns>Returns boolean type of whether the operation 
        /// was successful or not</returns>
        public virtual async Task<bool> InsertManyAsync(ICollection<TDocument> documents)
        {
            //using (var session = await _client.StartSessionAsync())
            //{
            //    // Begin transaction
            //    session.StartTransaction();

            try
            {
                // Create some sample data
                await _collection.InsertManyAsync(documents);

                // Made it here without error? Let's commit the transaction
                //await session.CommitTransactionAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to MongoDB: " + e.Message);
                //await session.AbortTransactionAsync();
                return false;
            }
        }
    }
}