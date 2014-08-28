﻿using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Exchange.WebServices.Data;

namespace SODA.Utilities
{
    /// <summary>
    /// Base implementation of the IEwsClient interface
    /// </summary>
    public abstract class EwsClient : IEwsClient
    {
        /// <summary>
        /// The underlying <see cref="Microsoft.Exchange.WebServices.Data.ExchangeService"/> that this client uses to talk to EWS.
        /// </summary>
        protected readonly ExchangeService exchangeService;
        
        //lazy-load the inbox folder
        private Folder inbox = null;

        /// <summary>
        /// Gets a <see cref="Microsoft.Exchange.WebServices.Data.Folder"/> object representing the Inbox that this client connects to.
        /// </summary>
        protected Folder Inbox
        {
            get
            {
                if(inbox == null)
                    inbox = Folder.Bind(exchangeService, WellKnownFolderName.Inbox);
                return inbox;
            }
        }

        /// <summary>
        /// The Uri at which the target Exchange Web Services are hosted.
        /// </summary>
        public Uri ServiceEndpoint
        {
            get { return exchangeService.Url; }
        }

        /// <summary>
        /// Initialize a new EwsClient using the specified username and password to connect to the specified version of EWS. An attempt is made to autodiscovered the EWS endpoint.
        /// </summary>
        /// <param name="username">A user with login rights on the specified <paramref name="domain"/>.</param>
        /// <param name="password">The password for the specified <paramref name="username"/>.</param>
        /// <param name="domain">The Exchange domain.</param>
        /// <param name="exchangeVersion">The version of Exchange.</param>
        /// <remarks>
        /// In order for autodiscovery of an EWS endpoint to work, there may be additional Exchange configuration required.
        /// See http://msdn.microsoft.com/en-us/library/office/jj900169(v=exchg.150).aspx.
        /// </remarks>
        public EwsClient(string username, string password, string domain, ExchangeVersion exchangeVersion)
        {
            exchangeService = new ExchangeService(exchangeVersion)
            {
                Credentials = new WebCredentials(username, password, domain),
            };
            
            exchangeService.AutodiscoverUrl(String.Format("{0}@{1}", username, domain));
        }

        /// <summary>
        /// Search the Inbox for the first unread email with an attachment matching the specified regular expression, and if found, download the attachment to the specified directory.
        /// </summary>
        /// <param name="attachmentNamePattern">The attachment filename pattern to search for.</param>
        /// <param name="targetDirectory">The (writable) directory where a found attachment will be saved.</param>
        /// <returns>True if a matching attachment was found and downloaded. False otherwise.</returns>
        public virtual bool DownloadAttachment(Regex attachmentNamePattern, string targetDirectory)
        {
            //we don't know how many items we'll have to search (likely nowhere near int.MaxValue)
            ItemView view = new ItemView(int.MaxValue);

            //we want the most recently received item
            view.OrderBy.Add(ItemSchema.DateTimeReceived, SortDirection.Descending);

            //load the properties required to perform a search for the attachment
            view.PropertySet = new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.IsRead, EmailMessageSchema.HasAttachments);

            //build the search filter
            SearchFilter filter = new SearchFilter.SearchFilterCollection(
                LogicalOperator.And,
                new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false),
                new SearchFilter.IsEqualTo(EmailMessageSchema.HasAttachments, true)
            );

            //perform the search and return the results as EmailMessage objects
            var results = Inbox.FindItems(filter, view).Cast<EmailMessage>();
            bool foundAttachment = false;

            foreach (var result in results)
            {
                //another call to EWS to actually load in this email's attachment collection
                var email = EmailMessage.Bind(exchangeService, result.Id, new PropertySet(EmailMessageSchema.Attachments));

                foreach (var attachment in email.Attachments)
                {
                    if (attachmentNamePattern.IsMatch(attachment.Name))
                    {
                        FileAttachment fileAttachment = attachment as FileAttachment;
                        if (fileAttachment != null)
                        {
                            //save the attachment to the target directory
                            fileAttachment.Load(Path.Combine(targetDirectory, fileAttachment.Name));
                            //mark the email as read
                            email.IsRead = true;
                            email.Update(ConflictResolutionMode.AlwaysOverwrite);
                            //get outta here!
                            foundAttachment = true;
                            break;
                        }
                    }
                }

                if (foundAttachment) break;
            }

            return foundAttachment;
        }

        /// <summary>
        /// Send an email message with the specified subject and body to the specified list of recipient email addresses.
        /// </summary>
        /// <param name="messageSubject">The subject line of the email message.</param>
        /// <param name="messageBody">The plain-text content of the email message.</param>
        /// <param name="recipients">One or more email addresses that will be recipients of the email message.</param>
        public virtual void SendMessage(string messageSubject, string messageBody, params string[] recipients)
        {
            var email = new EmailMessage(exchangeService);

            email.Subject = messageSubject;
            email.Body = messageBody;
            email.ToRecipients.AddRange(recipients);

            email.SendAndSaveCopy();
        }
    }
}