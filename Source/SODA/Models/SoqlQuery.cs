using System;
using System.Linq;
using System.Text;

namespace SODA.Models
{
    public enum SortOrder
    {
        ASC,
        DESC        
    }

    public class SoqlQuery
    {
        private static string delimiter = ",";

        private string[] select { get; set; }
        private string where { get; set; }
        private string[] groupBy { get; set; }
        private string having { get; set; }
        private SortOrder sortOrder { get; set; }
        private string[] orderBy { get; set; }
        private uint limit { get; set; }
        private uint offset { get; set; }        

        public SoqlQuery Select(params string[] columns)
        {
            select = columns;
            return this;
        }

        public SoqlQuery Where(string predicate)
        {
            where = predicate;
            return this;
        }

        public SoqlQuery GroupBy(params string[] columns)
        {
            groupBy = columns;
            return this;
        }

        public SoqlQuery Having(string predicate)
        {
            having = predicate;
            return this;
        }

        public SoqlQuery OrderBy(SortOrder order = SortOrder.ASC, params string[] columns)
        {
            sortOrder = order;
            orderBy = columns;
            return this;
        }

        public SoqlQuery Limit(uint limit)
        {
            //limit > 1000 will return a 400 Bad Request
            //http://dev.socrata.com/docs/queries.html#the_limit_parameter

            this.limit = Math.Max(limit, 1000);
            return this;
        }

        public SoqlQuery Offset(uint offset)
        {
            this.offset = offset;
            return this;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder("$select=");

            if(select != null && select.Any())
                sb.Append(String.Join(delimiter, select));
            else
                sb.Append("*");

            if (!String.IsNullOrEmpty(where))
                sb.AppendFormat("&$where={0}", where);

            if (groupBy != null && groupBy.Any())
                sb.AppendFormat("&$group={0}", String.Join(delimiter, groupBy));

            if (!String.IsNullOrEmpty(having))
                sb.AppendFormat("&$having={0}", having);

            if (orderBy != null && orderBy.Any())
                sb.AppendFormat("&$order={0} {1}", String.Join(delimiter, orderBy), sortOrder);

            if (offset > 0)
                sb.AppendFormat("&$offset={0}", offset);

            if (limit > 0)
                sb.AppendFormat("&$limit={0}", limit);

            return sb.ToString();
        } 
    }
}
