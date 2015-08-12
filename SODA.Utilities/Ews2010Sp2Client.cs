using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Exchange.WebServices.Data;

namespace SODA.Utilities
{
    /// <summary>
    /// EwsClient implementation for Exchange Server 2010 SP2
    /// </summary>
    public class Ews2010Sp2Client : EwsClient
    {
        /// <summary>
        /// Initialize a new EwsClient targeting Exchange Web Services 2007 SP1.
        /// </summary>
        /// <param name="username">A user with login rights on the specified <paramref name="domain"/>.</param>
        /// <param name="password">The password for the specified <paramref name="username"/>.</param>
        /// <param name="domain">The Exchange domain.</param>
        public Ews2010Sp2Client(string username, string password, string domain)
            : base(username, password, domain, ExchangeVersion.Exchange2010_SP2)
        {
        }

        /// <summary>
        /// Search the Inbox for the first unread email with an attachment matching the specified regular expression, and if found, download the attachment to the specified directory.
        /// </summary>
        /// <param name="attachmentNamePattern">The attachment filename pattern to search for.</param>
        /// <param name="targetDirectory">The (writable) directory where a found attachment will be saved.</param>
        /// <returns>True if a matching attachment was found and downloaded. False otherwise.</returns>
        /// <remarks>
        /// If the <paramref name="targetDirectory"/> does not exist, it will be created before searching for an attachment to download.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="attachmentNamePattern"/> regular expression is null.</exception>
        public override bool DownloadAttachment(Regex attachmentNamePattern, string targetDirectory)
        {
            if (attachmentNamePattern == null)
                throw new ArgumentNullException("attachmentNamePattern");

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            return base.DownloadAttachment(attachmentNamePattern, targetDirectory);
        }

        /// <summary>
        /// Send an email message with the specified subject and body to the specified list of recipient email addresses.
        /// </summary>
        /// <param name="messageSubject">The subject line of the email message.</param>
        /// <param name="messageBody">The plain-text content of the email message.</param>
        /// <param name="recipients">One or more email addresses that will be recipients of the email message.</param>
        /// <exception cref="System.ArgumentException">Thrown if the specified list of recipients is null or empty.</exception>
        public override void SendMessage(string messageSubject, string messageBody, params string[] recipients)
        {
            if (recipients == null || !recipients.Any())
                throw new ArgumentException("Must specify at least 1 email recipient.", "recipients");

            base.SendMessage(messageSubject, messageBody, recipients);
        }
    }
}
