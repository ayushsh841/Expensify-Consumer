using Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Services.Implementation
{
    /// <summary>
    /// This class holds the method for parsing the
    /// contents of the file into the required model
    /// </summary>
    public class FileProcessor : IFileProcessor
    {
        /// <summary>
        /// This method parses the content of the file 
        /// into required model object
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="file">File to parse</param>
        /// <returns>List of object</returns>
        public List<T> Process<T>(string file)
        {
            List<T> details = new List<T>();

            string json = File.ReadAllText(file);

            // Parse details into required models
            details = JsonSerializer.Deserialize<List<T>>(json);

            return details;
        }
    }
}
