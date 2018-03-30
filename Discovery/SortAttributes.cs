using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public enum SortAttributes
    {
        [DataMember(Name = "relevance")]
        Relevance,
        [DataMember(Name = "name")]
        Name,
        [DataMember(Name = "owner")]
        Owner,
        [DataMember(Name = "dataset_id")]
        DatasetId,
        [DataMember(Name = "datatype")]
        Datatype,
        [DataMember(Name = "domain_category")]
        DomainCategory,
        [DataMember(Name = "createdAt")]
        CreatedAt,
        [DataMember(Name = "updatedAt")]
        UpdatedAt,
        [DataMember(Name = "page_views_total")]
        PageViewsTotal,
        [DataMember(Name = "page_views_last_month")]
        PageViewsLastMonth,
        [DataMember(Name = "page_views_last_week")]
        PageViewsLastWeek,
    }
}
