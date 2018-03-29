using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class PageViews
    {
        [DataMember(Name = "page_views_last_week")]
        public long LastWeek { get; internal set; }

        [DataMember(Name = "page_views_last_month")]
        public long LastMonth { get; internal set; }

        [DataMember(Name = "page_views_total")]
        public long Total { get; internal set; }

        [DataMember(Name = "page_views_last_week_log")]
        public decimal LastWeekLog { get; internal set; }

        [DataMember(Name = "page_views_last_month_log")]
        public decimal LastMonthLog { get; internal set; }

        [DataMember(Name = "page_views_total_log")]
        public decimal TotalLog { get; internal set; }
    }
}
