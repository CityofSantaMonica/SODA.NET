using Microsoft.Exchange.WebServices.Data;
using System;

namespace SODA.Utilities
{
    /// <summary>
    /// EwsClient implementation for Exchange Server 2007 SP1
    /// </summary>
    public class Ews2007Sp1Client : EwsClient
    {
        /// <summary>
        /// Initialize a new EwsClient targeting Exchange Web Services 2007 SP1.
        /// </summary>
        /// <param name="username">A user with login rights on the specified <paramref name="domain"/>.</param>
        /// <param name="password">The password for the specified <paramref name="username"/>.</param>
        /// <param name="domain">The Exchange domain.</param>
        public Ews2007Sp1Client(string username, string password, string domain)
            : base(username, password, domain, ExchangeVersion.Exchange2007_SP1)
        {
        }
    }
}
