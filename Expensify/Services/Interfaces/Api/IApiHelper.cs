using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IApiHelper
    {
        /// <summary>
        /// Does a GET request
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="function">function name to call</param>
        /// <param name="_logger">_logger</param>
        /// <returns>Task</returns>
        Task<T> GetResultAsync<T>(string function);

        /// <summary>
        /// Does a HTTP POST response
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="function">Function name</param>
        /// <param name="postBody">List of post params</param>
        /// <returns>Task with result</returns>
        Task<T> PostResultAsync<T>(string function, string postBody);
    }
}
