using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SODA
{
    /// <summary>
    /// A class representing a query against a Socrata resource using a series of SoQL clauses.
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
        public static readonly string SearchKey = "$q";

        //the default is to select all columns
        //http://dev.socrata.com/docs/queries.html
        public static readonly string[] DefaultSelect = new[] { "*" };
        
        //the default sort direction is ascending
        //http://dev.socrata.com/docs/queries.html#the_order_parameter
        public static readonly SoqlOrderDirection DefaultOrderDirection = SoqlOrderDirection.ASC;
        
        //there is no implicit order of results of a query
        //so at a minimum provide $order=:id to guarantee that the order of results will be stable for paging
        //http://dev.socrata.com/docs/queries.html#the_order_parameter
        public static readonly string[] DefaultOrder = new[] { ":id" };

        //the maximum that can be requested with limit is 1000
        //http://dev.socrata.com/docs/queries.html#the_limit_parameter
        public static readonly int MaximumLimit = 1000;

        public string[] SelectColumns { get; private set; }
        public string[] SelectColumnAliases { get; private set; }
        public string WhereClause { get; private set; }
        public SoqlOrderDirection OrderDirection { get; private set; }
        public string[] OrderByColumns { get; private set; }
        public string[] GroupByColumns { get; private set; }
        public int LimitValue { get; private set; }
        public int OffsetValue { get; private set; }
        public string SearchText { get; private set; }

        /// <summary>
        /// Construct a new SoqlQuery using the defaults.
        /// </summary>
        public SoqlQuery()
        {
            SelectColumns = DefaultSelect;
            SelectColumnAliases = new string[0];
            OrderByColumns = DefaultOrder;
            OrderDirection = DefaultOrderDirection;
        }
        
        /// <summary>
        /// Converts this SoqlQuery into a string format suitable for use in a SODA call.
        /// </summary>
        /// <returns>The string representation of this SoqlQuery.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0}=", SelectKey);

            if (SelectColumns.Length == 1 && SelectColumns[0] == "*")
            {
                sb.Append(SelectColumns[0]);
            }
            else 
            {
                //evaluate the provided aliases
                var finalColumns = SelectColumns.Zip(SelectColumnAliases, (c, a) => String.Format("{0} AS {1},", c, a)).ToList();

                //if some columns were left un-aliased
                if (SelectColumns.Length > SelectColumnAliases.Length)
                {
                    finalColumns.AddRange(SelectColumns.Skip(SelectColumnAliases.Length));
                }

                //form the select clause
                sb.Append(String.Join(Delimiter, finalColumns));
            }

            sb.AppendFormat("&{0}={1} {2}", OrderKey, String.Join(Delimiter, OrderByColumns), OrderDirection);
                        
            if (!String.IsNullOrEmpty(WhereClause))
                sb.AppendFormat("&{0}={1}", WhereKey, WhereClause);

            if (GroupByColumns != null && GroupByColumns.Any())
                sb.AppendFormat("&{0}={1}", GroupKey, String.Join(Delimiter, GroupByColumns));
            
            if (OffsetValue > 0)
                sb.AppendFormat("&{0}={1}", OffsetKey, OffsetValue);

            if (LimitValue > 0)
                sb.AppendFormat("&{0}={1}", LimitKey, LimitValue);

            if (!String.IsNullOrEmpty(SearchText))
                sb.AppendFormat("&{0}={1}", SearchKey, SearchText);

            return sb.ToString();
        }

        /// <summary>
        /// Sets this SoqlQuery's select clause using the specified columns.
        /// </summary>
        /// <param name="columns">A list of column names to select during execution of this SoqlQuery.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Select(params string[] columns)
        {
            SelectColumns = getNonEmptyValues(columns) ?? DefaultSelect;
            return this;
        }

        /// <summary>
        /// Uses the specified column aliases for this SoqlQuery's select clause.
        /// </summary>
        /// <param name="columnAliases">A list of column aliases to be applied, in the specified order. Aliases beyond the available select columns are ignored.</param>
        /// <returns>This SoqlQuery.</returns>
        /// <remarks>
        /// SODA calls ignore text casing in aliases and return all aliased column names in lowercase.
        /// </remarks>
        public SoqlQuery As(params string[] columnAliases)
        {
            SelectColumnAliases = (getNonEmptyValues(columnAliases) ?? new string[0]).Select(a => a.ToLower()).ToArray();
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's where clause using the specified predicate.
        /// </summary>
        /// <param name="predicate">A filter to be applied to the columns selected by this SoqlQuery.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Where(string predicate)
        {
            WhereClause = predicate;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's where clause using the specified format string and substitution arguments.
        /// </summary>
        /// <param name="format">A composite format string, suitable for use with String.Format()</param>
        /// <param name="arguments">An array of objects to format.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Where(string format, params object[] args)
        {
            return Where(String.Format(format, args));
        }

        /// <summary>
        /// Sets this SoqlQuery's order clause using the specified columns and the DefaultOrderDirection.
        /// </summary>
        /// <param name="columns">A list of column names that define the order in which the rows selected by this SoqlQuery are returned.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Order(params string[] columns)
        {
            return Order(DefaultOrderDirection, columns);
        }

        /// <summary>
        /// Sets this SoqlQuery's order clause using the specified columns and the specified SoqlOrderDirection.
        /// </summary>
        /// <param name="direction">The direction to sort the rows selected by this SoqlQuery.</param>
        /// <param name="columns">A list of column names that define the order in which the rows selected by this SoqlQuery are returned.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Order(SoqlOrderDirection direction, params string[] columns)
        {
            OrderDirection = direction;
            OrderByColumns = getNonEmptyValues(columns) ?? DefaultOrder;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's group clause using the specified columns.
        /// </summary>
        /// <param name="columns">A list of column names that define how rows are grouped during execution of this SoqlQuery.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Group(params string[] columns)
        {
            GroupByColumns = getNonEmptyValues(columns);
            return this;
        }
        
        /// <summary>
        /// Sets this SoqlQuery's limit clause using the specified integral limit.
        /// </summary>
        /// <param name="limit">A number representing the maximum number of rows this SoqlQuery should return.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Limit(int limit)
        {
            limit = Math.Max(limit, 0);
            limit = Math.Min(limit, MaximumLimit);
            this.LimitValue = limit;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's "offset" clause using the specified integral offset.
        /// </summary>
        /// <param name="offset">A number representing the starting offset into the total rows that this SoqlQuery returns.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Offset(int offset)
        {
            //offset < 0 makes no sense, take the absolute value
            offset = Math.Abs(offset);
            this.OffsetValue = offset;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's full text search clause to the specified input.
        /// </summary>
        /// <param name="searchText">The input to a full text search.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery FullTextSearch(string searchText)
        {
            this.SearchText = searchText;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's full text search clause using the specified format string and substitution arguments.
        /// </summary>
        /// <param name="format">A composite format string, suitable for use with String.Format()</param>
        /// <param name="arguments">An array of objects to format.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery FullTextSearch(string format, params object[] args)
        {
            return FullTextSearch(String.Format(format, args));
        }
        
        /// <summary>
        /// Restricts the input to only the non-empty values
        /// </summary>
        private static string[] getNonEmptyValues(IEnumerable<string> source)
        {
            if (source != null && source.Any(s => !String.IsNullOrEmpty(s)))
            {
                return source.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            }

            return null;
        }
    }
}