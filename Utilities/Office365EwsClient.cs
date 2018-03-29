using System;

namespace SODA.Utilities
{
    /// <summary>
    /// EwsClient implementation for Office 365
    /// </summary>
    public class Office365EwsClient : EwsClient
    {
        static readonly Uri O365EwsUrl = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

        /// <summary>
        /// Initialize a new Office365EwsClient using the specified email address and password to connect to the latest version of Office 365 EWS.
        /// </summary>
        /// <param name="emailAddress">An email address with login rights on Office 365.</param>
        /// <param name="password">The password for the specified <paramref name="emailAddress"/>.</param>
        public Office365EwsClient(string emailAddress, string password) : base(emailAddress, password, String.Empty, O365EwsUrl)
        {
        }
    }
}
