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
        [DataMember(Name="description")]
        public string Description { get; set; }

        private string url;
        [DataMember(Name="url")]
        public string Url
        {
            get { return url; }
            set { url = String.IsNullOrEmpty(value) ? String.Empty : Uri.EscapeUriString(value); }
        }

        public WebsiteUrlColumn() { }

        public WebsiteUrlColumn(string url, string description)
        {
            Url = url;
            Description = description;
        }

        public WebsiteUrlColumn(Uri uri, string description) : this(uri.ToString(), description) { }
    }
}
