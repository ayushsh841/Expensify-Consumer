namespace Models.ServiceModels
{
    /// <summary>
    /// Holds the request model for creating a 
    /// job to expensify 
    /// </summary>
    public class JobRequest
    {
        /// <summary>
        /// Holds the type of the job
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Holds the credentials for validating the request
        /// </summary>
        public Credentials credentials { get; set; }

        /// <summary>
        /// Holds the actions to be performed by the job
        /// upon recieving the request
        /// </summary>
        public ReceiveActions onReceive { get; set; }

        /// <summary>
        /// Holds the Input Settings for the job
        /// </summary>
        public InputSettings inputSettings { get; set; }

        /// <summary>
        /// Holds the output settings for the job
        /// </summary>
        public OutputSetting outputSettings { get; set; }
    }
}
