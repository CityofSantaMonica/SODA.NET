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

        [DataMember(Name="url")]
        public string Url { get; set; }

        public WebsiteUrlColumn() { }

        public WebsiteUrlColumn(string url, string description)
        {
            Url = String.IsNullOrEmpty(url) ? String.Empty : Uri.EscapeUriString(url);
            Description = description;
        }

        public WebsiteUrlColumn(Uri uri, string description) : this(uri.ToString(), description) { }
    }
}
