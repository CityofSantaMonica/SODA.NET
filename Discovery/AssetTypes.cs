using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public enum AssetTypes
    {
        [DataMember(Name = "api")]
        Api,
        [DataMember(Name = "calendar")]
        Calendar,
        [DataMember(Name = "chart")]
        Chart,
        [DataMember(Name = "datalens")]
        Datalens,
        [DataMember(Name = "dataset")]
        Dataset,
        [DataMember(Name = "federated_href")]
        FederatedHref,
        [DataMember(Name = "file")]
        File,
        [DataMember(Name = "filter")]
        Filter,
        [DataMember(Name = "form")]
        Form,
        [DataMember(Name = "href")]
        Href,
        [DataMember(Name = "link")]
        Link,
        [DataMember(Name = "map")]
        Map,
        [DataMember(Name = "measure")]
        Measure,
        [DataMember(Name = "story")]
        Story,
        [DataMember(Name = "visualization")]
        Visualization
    }
}
