using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// Enumeration of possible sort orders for use with SoQL queries.
    /// </summary>
    public enum SortOrder
    {
        ASC,
        DESC        
    }

    /// <summary>
    /// A class representing a query against a Socrata resource.
    /// </summary>
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

        /// <summary>
        /// Sets this SoqlQuery's "select" clause using the specified columns.
        /// </summary>
        /// <param name="columns">A list of column names to select during execution of this SoqlQuery.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Select(params string[] columns)
        {
            select = getNonEmptyValues(columns);
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's "where" clause using the specified predicate.
        /// </summary>
        /// <param name="predicate">A filter to be applied to the columns selected by this SoqlQuery.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Where(string predicate)
        {
            where = predicate;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's "order" clause using the specified columns and the default SortOrder.
        /// </summary>
        /// <param name="columns">A list of column names that define the order in which the records selected by this SoqlQuery are returned.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Order(params string[] columns)
        {
            return Order(SortOrder.ASC, columns);
        }

        /// <summary>
        /// Sets this SoqlQuery's "order" clause using the specified columns and the specified SortOrder.
        /// </summary>
        /// <param name="sortOrder">The direction to sort the records selected by this SoqlQuery.</param>
        /// <param name="columns">A list of column names that define the order in which the records selected by this SoqlQuery are returned.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Order(SortOrder sortOrder, params string[] columns)
        {
            this.sortOrder = sortOrder;
            orderBy = getNonEmptyValues(columns);
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's "group" clause using the specified columns.
        /// </summary>
        /// <param name="columns">A list of column names that define how records are grouped during execution of this SoqlQuery.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Group(params string[] columns)
        {
            groupBy = getNonEmptyValues(columns);
            return this;
        }
        
        /// <summary>
        /// Sets this SoqlQuery's "limit" clause using the specified integral limit.
        /// </summary>
        /// <param name="limit">A number representing the maximum number of records this SoqlQuery should return.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Limit(int limit)
        {
            //limit < 0 makes no sense, take the absolute value
            limit = Math.Abs(limit);

            //limit > 1000 will return a 400 Bad Request
            //http://dev.socrata.com/docs/queries.html#the_limit_parameter
            limit = Math.Min(limit, 1000);

            this.limit = limit;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's "offset" clause using the specified integral offset.
        /// </summary>
        /// <param name="offset">A number representing the starting offset into the total records that this SoqlQuery returns.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Offset(int offset)
        {
            //offset < 0 makes no sense, take the absolute value
            offset = Math.Abs(offset);

            this.offset = offset;

            return this;
        }
        
        /// <summary>
        /// Converts this SoqlQuery into a string format suitable for use in a SODA call.
        /// </summary>
        /// <returns>The string representation of this SoqlQuery.</returns>
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

        /// <summary>
        /// Restricts the input to only the non-empty values
        /// </summary>
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