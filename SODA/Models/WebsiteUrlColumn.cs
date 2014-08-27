using System;
using System.Runtime.Serialization;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata Website URL column type.
    /// </summary>
    [DataContract]
    public class WebsiteUrlColumn
    {
        /// <summary>
        /// Gets or sets the link text of this WebsiteUrlColumn.
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the link url of this WebsiteUrlColumn.
        /// </summary>
        private string url;
        [DataMember(Name="url")]
        public string Url
        {
            get { return url; }
            set { url = String.IsNullOrEmpty(value) ? String.Empty : Uri.EscapeUriString(value); }
        }

        /// <summary>
        /// Initialize a new WebsiteUrlColumn.
        /// </summary>
        public WebsiteUrlColumn() { }

        /// <summary>
        /// Initialize a new WebsiteUrlColumn with the specified url and description.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="description"></param>
        public WebsiteUrlColumn(string url, string description)
        {
            Url = url;
            Description = description;
        }

        /// <summary>
        /// Initialize a new WebsiteUrlColumn with the specified uri and description.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="description"></param>
        public WebsiteUrlColumn(Uri uri, string description) : this(uri.ToString(), description) { }
    }
}
