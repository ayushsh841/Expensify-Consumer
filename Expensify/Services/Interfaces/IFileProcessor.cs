using Models.ServiceModels;
using System.Collections.Generic;

namespace Services.Interfaces
{
    /// <summary>
    /// This interface holds the method for parsing the
    /// contents of the file into the required model
    /// </summary>
    public interface IFileProcessor
    {
        /// <summary>
        /// This method parses the content of the file 
        /// into required model object
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="file">File to parse</param>
        /// <returns>List of object</returns>
        List<T> Process<T>(string file);
    }
}
