using Models.DbModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Base
{
    /// <summary>
    /// This interface defines the generic methods for 
    /// the Mongo Db
    /// </summary>
    /// <typeparam name="TDocument">Document Type</typeparam>
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        /// <summary>
        /// Returns all documents from a collection
        /// </summary>
        /// <returns>ICollection object of the documents</returns>
        IQueryable<TDocument> AsQueryable();

        /// <summary>
        /// This method inserts a list of objects into DB
        /// </summary>
        /// <param name="documents">Generic document object</param>
        /// <returns>Returns boolean type of whether the operation 
        /// was successful or not</returns>
        Task<bool> InsertManyAsync(ICollection<TDocument> documents);
    }
}
