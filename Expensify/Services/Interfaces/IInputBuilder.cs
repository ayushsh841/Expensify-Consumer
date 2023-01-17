using Models.ServiceModels;
using System.Collections.Generic;

namespace Services.Interfaces
{
    /// <summary>
    /// This interface defines the method for 
    /// building the JobRequest object
    /// </summary>
    public interface IInputBuilder
    {
        /// <summary>
        /// This method is responsible for building the 
        /// JobRequest model
        /// </summary>
        /// <param name="filters">Filters to be added</param>
        /// <returns>Job Request model</returns>
        JobRequest Build(Dictionary<string, string> filters);
    }
}
