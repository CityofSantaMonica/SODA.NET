using System;
using System.Text.RegularExpressions;

namespace SODA.Utilities
{
    /// <summary>
    /// Interface definition for an Exchange Web Services Client
    /// </summary>
    public interface IEwsClient
    {
        /// <summary>
        /// The Uri at which the target Exchange Web Services are hosted.
        /// </summary>
        Uri ServiceEndpoint { get; }

        /// <summary>
        /// Search the Inbox for the first unread email with an attachment matching the specified regular expression, and if found, download the attachment to the specified directory.
        /// </summary>
        /// <param name="attachmentNamePattern">The attachment filename pattern to search for.</param>
        /// <param name="targetDirectory">The (writable) directory where a found attachment will be saved.</param>
        /// <returns>True if a matching attachment was found and downloaded. False otherwise.</returns>
        bool DownloadAttachment(Regex attachmentNamePattern, string targetDirectory);

        /// <summary>
        /// Send an email message with the specified subject and body to the specified list of recipient email addresses.
        /// </summary>
        /// <param name="messageSubject">The subject line of the email message.</param>
        /// <param name="messageBody">The plain-text content of the email message.</param>
        /// <param name="recipients">One or more email addresses that will be recipients of the email message.</param>
        void SendMessage(string messageSubject, string messageBody, params string[] recipients);
    }
}
