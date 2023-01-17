namespace Common
{
    public static class Constants
    {

        // Settings files
        #region Log and config
        
        public const string AppSettings = "appsettings.json";

        /// <summary>
        /// DateTime Format for Logs.
        /// </summary>
        public const string LogsDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// Holds the Service name
        /// </summary>
        public const string ServiceName = "ExpenseCollector";

        #endregion

        #region File Parser

        public const string ErrorFolder = "\\Error";
        public const string ProcessedFolder = "\\Processed";

        public const string FileNamePattern = "Expense_*";

        #region App Settings 

        public const string BasePath = "File:BasePath";

        #endregion

        #endregion

        #region ExpenseCollector

        public const string ApiDateFormat = "yyyy-MM-dd";
        public const string ApiRequest = "?requestJobDescription=";
        public const string TemplateFile = "Template.txt";
        public const string RequestType = "file";
        public const string BaseName = "Expense_";

        #region App Settings 

        public const string Duration = "Duration";
        public const string UserName = "Credentials:UserId";
        public const string UserSecret = "Credentials:UserSecret";
        public const string InputType = "Input:type";
        public const string ExpensifyUrl = "Url:Expensify";

        #endregion

        #endregion
    }
}
