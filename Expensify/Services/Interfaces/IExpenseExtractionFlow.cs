using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    /// <summary>
    /// Manages the flow for adding vendors and 
    /// expenses from files
    /// </summary>
    public interface IExpenseExtractionFlow
    {
        /// <summary>
        /// This methods manages the complete flow of adding new expense 
        /// and vendors from a given file
        /// </summary>
        /// <param name="fileName">Complete path to the file</param>
        /// <param name="logger">File Logger object</param>
        /// <returns>Returns if the process was successful or not.</returns>
        Task<bool> Extract(string fileName, ILogger logger);
    }
}
