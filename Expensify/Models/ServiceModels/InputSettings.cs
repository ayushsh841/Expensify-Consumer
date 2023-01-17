using System.Collections.Generic;

namespace Models.ServiceModels
{
    /// <summary>
    /// This class holds the the filters 
    /// for the API
    /// </summary>
    public class InputSettings
    {
        /// <summary>
        /// Holds the type of report to generate
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Holds the filters to apply
        /// </summary>
        public Dictionary<string, string> filters { get; set; }
    }
}
