using System.Runtime.Serialization;

namespace SODA.Models
{
    [DataContract]
    public class WebsiteUrlColumn
    {
        [DataMember(Name="description")]
        public string Description { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }
    }
}
