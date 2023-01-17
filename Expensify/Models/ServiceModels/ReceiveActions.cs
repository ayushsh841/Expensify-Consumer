using System.Collections.Generic;

namespace Models.ServiceModels
{
    /// <summary>
    /// This class defines the actions to be done
    /// on recieving the request
    /// </summary>
    public class ReceiveActions
    {
        public ReceiveActions()
        {
            immediateResponse = new List<string>() { "returnRandomFileName" };
        }

        /// <summary>
        /// Holds the list of items to be done
        /// </summary>
        public List<string> immediateResponse { get; set; }
    }
}
