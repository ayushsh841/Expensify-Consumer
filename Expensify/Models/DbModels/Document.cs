using MongoDB.Bson;
using System;

namespace Models.DbModels
{
    /// <summary>
    /// This class defines the base properties of the 
    /// a document object
    /// </summary>
    public class Document : IDocument
    {
        /// <summary>
        /// Unique id of the document
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Date the document was created
        /// </summary>
        public DateTime CreatedAt => Id.CreationTime;
    }
}
