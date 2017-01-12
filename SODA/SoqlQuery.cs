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
        /// <summary>
        /// The delimiter used for lists of parameters (e.g. a list of columns in Select)
        /// </summary>
        public static readonly string Delimiter = ",";

        /// <summary>
        /// The querystring key for the SoQL Select clause.
        /// </summary>
        public static readonly string SelectKey = "$select";

        /// <summary>
        /// The querystring key for the SoQL Where clause.
        /// </summary>
        public static readonly string WhereKey = "$where";

        /// <summary>
        /// The querystring key for the SoQL Order clause.
        /// </summary>
        public static readonly string OrderKey = "$order";

        /// <summary>
        /// The querystring key for the SoQL Group clause.
        /// </summary>
        public static readonly string GroupKey = "$group";

        /// <summary>
        /// The querystring key for the SoQL Having clause.
        /// </summary>
        public static readonly string HavingKey = "$having";

        /// <summary>
        /// The querystring key for the SoQL Limit clause.
        /// </summary>
        public static readonly string LimitKey = "$limit";

        /// <summary>
        /// The querystring key for the SoQL Offset clause.
        /// </summary>
        public static readonly string OffsetKey = "$offset";

        /// <summary>
        /// The querystring key for the SoQL full-text search clause.
        /// </summary>
        public static readonly string SearchKey = "$q";

        /// <summary>
        /// The querystring key for a raw SoQL query.
        /// </summary>
        public static readonly string QueryKey = "$query";

        /// <summary>
        /// The default values for a Select clause.
        /// </summary>
        /// <remarks>
        /// The default is to select all columns (http://dev.socrata.com/docs/queries.html)
        /// </remarks>
        [Obsolete("Socrata provides $select = * by default, so this is field is no longer needed and will be removed in the next release.")]
        public static readonly string[] DefaultSelect = new[] { "*" };

        /// <summary>
        /// The default sort direction for an Order clause.
        /// </summary>
        /// <remarks>
        /// the default sort direction is ascending (http://dev.socrata.com/docs/queries.html#the_order_parameter)
        /// </remarks>
        public static readonly SoqlOrderDirection DefaultOrderDirection = SoqlOrderDirection.ASC;
        
        /// <summary>
        /// The default values for an Order clause.
        /// </summary>
        /// <remarks>
        /// There is no implicit order of results of a query,
        /// so at a minimum provide $order=:id to guarantee that the order of results will be stable for paging.
        /// (http://dev.socrata.com/docs/queries.html#the_order_parameter)
        /// </remarks>
        public static readonly string[] DefaultOrder = new[] { ":id" };

        /// <summary>
        /// The maximum number of results a query may return.
        /// </summary>
        /// <remarks>
        /// The maximum that can be requested with limit is 50,000 (http://dev.socrata.com/docs/paging.html)
        /// </remarks>
        public static readonly int MaximumLimit = 50000;

        /// <summary>
        /// Gets the columns that this SoqlQuery will select.
        /// </summary>
        public string[] SelectColumns { get; private set; }

        /// <summary>
        /// Gets the aliases for the columns that this SoqlQuery will select.
        /// </summary>
        public string[] SelectColumnAliases { get; private set; }

        /// <summary>
        /// Gets the predicate that this SoqlQuery will use for results filtering.
        /// </summary>
        public string WhereClause { get; private set; }

        /// <summary>
        /// Gets the sort direction that results from this SoqlQuery will be ordered on.
        /// </summary>
        public SoqlOrderDirection OrderDirection { get; private set; }

        /// <summary>
        /// Gets the columns that define the ordering for the results of this SoqlQuery.
        /// </summary>
        public string[] OrderByColumns { get; private set; }

        /// <summary>
        /// Gets the columns that define grouping for the results of this SoqlQuery.
        /// </summary>
        public string[] GroupByColumns { get; private set; }

        /// <summary>
        /// Gets the predicate that this SoqlQuery will use for aggregate filtering.
        /// </summary>
        public string HavingClause { get; private set; }

        /// <summary>
        /// Gets the maximum number of results that this SoqlQuery will return.
        /// </summary>
        public int LimitValue { get; private set; }

        /// <summary>
        /// Gets the offset into the full resultset that this SoqlQuery will begin from.
        /// </summary>
        public int OffsetValue { get; private set; }

        /// <summary>
        /// Gets the input to a full-text search that this SoqlQuery will perform.
        /// </summary>
        public string SearchText { get; private set; }

        /// <summary>
        /// Gets the raw SoQL query, combining one or more SoQL clauses and/or sub-queries, that this SoqlQuery will execute.
        /// </summary>
        public string RawQuery { get; private set; }

        /// <summary>
        /// Initialize a new SoqlQuery object.
        /// </summary>
        public SoqlQuery()
        {
            SelectColumns = new string[0];
            SelectColumnAliases = new string[0];
            OrderDirection = DefaultOrderDirection;
        }

        /// <summary>
        /// Initialize a new SoqlQuery object with the given query string. Individual SoQL clauses cannot be overridden using the fluent syntax.
        /// </summary>
        /// <param name="query">One or more SoQL clauses and/or sub-queries.</param>
        public SoqlQuery(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
                throw new ArgumentOutOfRangeException("query", "A SoQL query is required");

            RawQuery = query;
        }

        /// <summary>
        /// Converts this SoqlQuery into a string format suitable for use in a SODA call.
        /// </summary>
        /// <returns>The string representation of this SoqlQuery.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!String.IsNullOrEmpty(RawQuery))
            {
                sb.AppendFormat("{0}={1}", QueryKey, RawQuery);
            }
            else
            {
                if (SelectColumns.Length > 0)
                {
                    sb.AppendFormat("{0}=", SelectKey);

                    //evaluate the provided aliases
                    var finalColumns =
                        SelectColumns.Zip(SelectColumnAliases, (c, a) => String.Format("{0} AS {1}", c, a)).ToList();

                    if (SelectColumns.Length > SelectColumnAliases.Length)
                    {
                        //some columns were left un-aliased
                        finalColumns.AddRange(SelectColumns.Skip(SelectColumnAliases.Length));
                    }
                    //form the select clause
                    sb.Append(String.Join(Delimiter, finalColumns));
                }

                if (OrderByColumns != null)
                    sb.AppendFormat("&{0}={1} {2}", OrderKey, String.Join(Delimiter, OrderByColumns), OrderDirection);

                if (!String.IsNullOrEmpty(WhereClause))
                    sb.AppendFormat("&{0}={1}", WhereKey, WhereClause);

                if (GroupByColumns != null && GroupByColumns.Any())
                    sb.AppendFormat("&{0}={1}", GroupKey, String.Join(Delimiter, GroupByColumns));

                if (!String.IsNullOrEmpty(HavingClause))
                    sb.AppendFormat("&{0}={1}", HavingKey, HavingClause);

                if (OffsetValue > 0)
                    sb.AppendFormat("&{0}={1}", OffsetKey, OffsetValue);

                if (LimitValue > 0)
                    sb.AppendFormat("&{0}={1}", LimitKey, LimitValue);

                if (!String.IsNullOrEmpty(SearchText))
                    sb.AppendFormat("&{0}={1}", SearchKey, SearchText);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Sets this SoqlQuery's select clause using the specified columns.
        /// </summary>
        /// <param name="columns">A list of column names to select during execution of this SoqlQuery.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Select(params string[] columns)
        {
            SelectColumns = getNonEmptyValues(columns) ?? new string[0];
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
        /// <param name="args">An array of objects to format.</param>
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
        /// Sets this SoqlQuery's having clause using the specified predicate.
        /// </summary>
        /// <param name="predicate">A filter to be applied to the results of an aggregation using <see cref="Group(string[])"/>.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Having(string predicate)
        {
            HavingClause = predicate;
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's having clause using the specified format string and substitution arguments.
        /// </summary>
        /// <param name="format">A composite format string, suitable for use with String.Format()</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Having(string format, params object[] args)
        {
            return Having(String.Format(format, args));
        }

        /// <summary>
        /// Sets this SoqlQuery's limit clause using the specified integral limit.
        /// </summary>
        /// <param name="limit">A number representing the maximum number of rows this SoqlQuery should return.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Limit(int limit)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException("limit");

            this.LimitValue = Math.Min(limit, MaximumLimit);
            return this;
        }

        /// <summary>
        /// Sets this SoqlQuery's "offset" clause using the specified integral offset.
        /// </summary>
        /// <param name="offset">A number representing the starting offset into the total rows that this SoqlQuery returns.</param>
        /// <returns>This SoqlQuery.</returns>
        public SoqlQuery Offset(int offset)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

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
        /// <param name="args">An array of objects to format.</param>
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