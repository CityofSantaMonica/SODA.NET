using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SODA
{
    public enum SortOrder
    {
        ASC,
        DESC        
    }

    public class SoqlQuery
    {
        public static readonly string Delimiter = ",";
        public static readonly string SelectKey = "$select";
        public static readonly string WhereKey = "$where";
        public static readonly string OrderKey = "$order";
        public static readonly string GroupKey = "$group";
        public static readonly string LimitKey = "$limit";
        public static readonly string OffsetKey = "$offset";
        
        private IEnumerable<string> select { get; set; }
        private string where { get; set; }
        private SortOrder sortOrder { get; set; }
        private IEnumerable<string> orderBy { get; set; }
        private IEnumerable<string> groupBy { get; set; }
        private int limit { get; set; }
        private int offset { get; set; }                

        public SoqlQuery Select(params string[] columns)
        {
            select = getNonEmptyValues(columns);
            return this;
        }

        public SoqlQuery Where(string predicate)
        {
            where = predicate;
            return this;
        }

        public SoqlQuery Order(params string[] columns)
        {
            return Order(SortOrder.ASC, columns);
        }

        public SoqlQuery Order(SortOrder sortOrder, params string[] columns)
        {
            this.sortOrder = sortOrder;
            orderBy = getNonEmptyValues(columns);
            return this;
        }

        public SoqlQuery Group(params string[] columns)
        {
            groupBy = getNonEmptyValues(columns);
            return this;
        }
        
        public SoqlQuery Limit(int limit)
        {
            //limit < 0 makes no sense, take the absolute value
            limit = Math.Abs(limit);

            //limit > 1000 will return a 400 Bad Request
            //http://dev.socrata.com/docs/queries.html#the_limit_parameter
            limit = limit > 1000 ? 1000 : limit;

            this.limit = limit;
            return this;
        }

        public SoqlQuery Offset(int offset)
        {
            //offset < 0 makes no sense, take the absolute value
            offset = Math.Abs(offset);

            this.offset = offset;

            return this;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder(SelectKey + "=");

            if(select != null && select.Any())
                sb.Append(String.Join(Delimiter, select));
            else
                sb.Append("*");

            if (!String.IsNullOrEmpty(where))
                sb.AppendFormat("&{0}={1}", WhereKey, where);

            if (groupBy != null && groupBy.Any())
                sb.AppendFormat("&{0}={1}", GroupKey, String.Join(Delimiter, groupBy));

            if (orderBy != null && orderBy.Any())
                sb.AppendFormat("&{0}={1} {2}", OrderKey, String.Join(Delimiter, orderBy), sortOrder);

            if (offset > 0)
                sb.AppendFormat("&{0}={1}", OffsetKey, offset);

            if (limit > 0)
                sb.AppendFormat("&{0}={1}", LimitKey, limit);

            return sb.ToString();
        }

        private static IEnumerable<string> getNonEmptyValues(IEnumerable<string> source)
        {
            if (source != null && source.Any())
            {
                return source.Where(s => !String.IsNullOrEmpty(s));
            }

            return null;
        }
    }
}