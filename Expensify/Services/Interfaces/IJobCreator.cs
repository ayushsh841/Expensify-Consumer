using System.Threading.Tasks;

namespace Services.Interfaces
{
    /// <summary>
    /// This class implements the method for creating 
    /// a job request
    /// </summary>
    public interface IJobCreator
    {
        /// <summary>
        /// This method is responsible for creating a job
        /// </summary>
        /// <returns>Returns the file name</returns>
        public Task<string> Create();
    }
}
