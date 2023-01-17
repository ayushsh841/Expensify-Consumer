using Common;
using System.Reflection.Metadata;

namespace Models.ServiceModels
{
    /// <summary>
    /// Holds the output settings for the job
    /// </summary>
    public class OutputSetting
    {
        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        public OutputSetting()
        {
            fileExtension = "json";
            fileBasename = Constants.BaseName;
        }

        /// <summary>
        /// Holds the base name for the file
        /// </summary>
        public string fileBasename { get; set; }

        /// <summary>
        /// Type of extension for the file
        /// </summary>
        public string fileExtension { get; set; }
    }
}
