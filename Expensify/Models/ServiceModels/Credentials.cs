namespace Models.ServiceModels
{
    /// <summary>
    /// Holds the auth credentals
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Holds the User id
        /// </summary>
        public string partnerUserID { get; set; }

        /// <summary>
        /// Holds the User Secret/Password
        /// </summary>
        public string partnerUserSecret { get; set; }
    }
}
