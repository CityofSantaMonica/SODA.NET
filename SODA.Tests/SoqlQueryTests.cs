using System;
using System.Linq;
using NUnit.Framework;

namespace SODA.Tests
{
    [TestFixture]
    public class SoqlQueryTests
    {
        [Test]
        [Category("SoqlQuery")]
        public void Default_Ctor_Selects_Default()
        {
            string defaultSelectClause = String.Format("{0}={1}", SoqlQuery.SelectKey, String.Join(SoqlQuery.Delimiter, SoqlQuery.DefaultSelect));

            string soql = new SoqlQuery().ToString();

            StringAssert.Contains(defaultSelectClause, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Default_Ctor_Orders_By_DefaultOrder_In_DefaultOrderDirection()
        {
            string defaultOrderClause = String.Format("{0}={1} {2}", SoqlQuery.OrderKey, String.Join(SoqlQuery.Delimiter, SoqlQuery.DefaultOrder), SoqlQuery.DefaultOrderDirection);

            string soql = new SoqlQuery().ToString();

            StringAssert.Contains(defaultOrderClause, soql);
        }
                
        [Test]
        [Category("SoqlQuery")]
        public void Empty_Select_Selects_Default()
        {
            string defaultSelectClause = String.Format("{0}={1}", SoqlQuery.SelectKey, String.Join(SoqlQuery.Delimiter, SoqlQuery.DefaultSelect));

            string emptySelect = new SoqlQuery().Select("").ToString();
            string manyEmptySelect = new SoqlQuery().Select("", "", "").ToString();
            string nullSelect = new SoqlQuery().Select(null).ToString();

            StringAssert.Contains(defaultSelectClause, emptySelect);
            StringAssert.Contains(defaultSelectClause, manyEmptySelect);
            StringAssert.Contains(defaultSelectClause, nullSelect);
        }

        [TestCase("column1", "")]
        [TestCase("column1", "", "column2")]
        [Category("SoqlQuery")]
        public void Select_Clause_Only_Gets_Valid_Columns(params string[] columns)
        {
            string expected = String.Format("{0}={1}", SoqlQuery.SelectKey, String.Join(SoqlQuery.Delimiter, columns.Where(c => !String.IsNullOrEmpty(c))));

            string soql = new SoqlQuery().Select(columns).ToString();

            StringAssert.Contains(expected, soql);
        }
        
        [Test]
        [Category("SoqlQuery")]
        public void Last_Select_Overwrites_All_Previous()
        {
            string[] first =  { "first" };
            string[] second = { "first", "second" };
            string[] last =  { "first", "second", "last" };
            
            string expected = String.Format("{0}={1}", SoqlQuery.SelectKey, String.Join(SoqlQuery.Delimiter, last));
            
            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Select(first)
                     .Select(second)
                     .Select(last);

            StringAssert.Contains(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Aliases_Are_Ignored()
        {
            string column = "column1";
            string startOfAlias = String.Format("{0} AS ", column);

            string emptyAlias = new SoqlQuery().Select(column).As("").ToString();
            string nullAlias = new SoqlQuery().Select(column).As(null).ToString();

            StringAssert.DoesNotContain(startOfAlias, emptyAlias);
            StringAssert.DoesNotContain(startOfAlias, nullAlias);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Select_Clause_Gets_Valid_Aliases_When_All_Columns_Are_Aliased()
        {
            string[] columns = new[] { "column1", "column2", "column3" };
            string[] aliases = new[] { "column_a", "column_b", "column_c" };

            string soql = new SoqlQuery().Select(columns).As(aliases).ToString();

            for (int i = 0; i < columns.Length; i++)
            {
                StringAssert.Contains(String.Format("{0} AS {1}", columns[i], aliases[i]), soql);
            }
        }

        [Test]
        [Category("SoqlQuery")]
        public void Select_Clause_Gets_Valid_Aliases_When_Some_Columns_Are_Aliased()
        {
            string[] columns = new[] { "column1", "column2", "column3" };
            string[] aliases = new[] { "column_a", "column_b", };

            string soql = new SoqlQuery().Select(columns).As(aliases).ToString();

            for (int i = 0; i < aliases.Length; i++)
            {
                StringAssert.Contains(String.Format("{0} AS {1}", columns[i], aliases[i]), soql);
            }
        }

        [Test]
        [Category("SoqlQuery")]
        public void Select_Clause_Selects_Unaliased_Columns_When_Some_Columns_Are_Aliased()
        {
            string[] columns = new[] { "column1", "column2", "column3", "column4", "column5" };
            string[] aliases = new[] { "column_a", "column_b" };

            string soql = new SoqlQuery().Select(columns).As(aliases).ToString();

            StringAssert.Contains(String.Join(SoqlQuery.Delimiter, columns.Skip(aliases.Length)), soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Select_Clause_Gets_Valid_Aliases_And_Ignores_Extra_Aliases()
        {
            string[] columns = new[] { "column1", "column2" };
            string[] aliases = new[] { "column_a", "column_b", "column_c", "column_d" };

            string soql = new SoqlQuery().Select(columns).As(aliases).ToString();

            for (int i = 0; i < columns.Length; i++)
            {
                StringAssert.Contains(String.Format("{0} AS {1}", columns[i], aliases[i]), soql);
            }

            string[] extraAliases = aliases.Skip(columns.Length).ToArray();

            for (int j = 0; j < extraAliases.Length; j++)
            {
                StringAssert.DoesNotContain(String.Format("AS {0}", extraAliases[j]), soql);
            }
        }

        [Test]
        [Category("SoqlQuery")]
        public void Select_Clause_Gets_Aliases_As_Lowercase()
        {
            string[] columns = new[] { "column1", "column2", "column3" };
            string[] aliases = new[] { "Column_A", "COLUMN_B", "CoLuMn_C" };

            string soql = new SoqlQuery().Select(columns).As(aliases).ToString();

            for (int i = 0; i < columns.Length; i++)
            {
                StringAssert.Contains(String.Format("{0} AS {1}", columns[i], aliases[i].ToLower()), soql);
            }
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Where_Ignores_Where_Clause()
        {
            string startOfWhereClause = String.Format("{0}=", SoqlQuery.WhereKey);

            string emptyWhere = new SoqlQuery().Where("").ToString();
            string nullWhere = new SoqlQuery().Where(null).ToString();

            StringAssert.DoesNotContain(startOfWhereClause, emptyWhere);
            StringAssert.DoesNotContain(startOfWhereClause, nullWhere);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Where_Clause_Gets_Valid_Predicate()
        {
            string predicate = "something > nothing";

            string expected = String.Format("{0}={1}", SoqlQuery.WhereKey, predicate);

            string soql = new SoqlQuery().Where(predicate).ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Where_Clause_Gets_Formatted_Input()
        {
            string format = "something > {0}";

            string expected = String.Format("{0}={1}", SoqlQuery.WhereKey, String.Format(format, "nothing"));

            string soql = new SoqlQuery().Where(format, "nothing").ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Where_Overwrites_All_Previous()
        {
            string first = "first > 0";
            string second = "second > first";
            string last = "last > anything";

            string expected = String.Format("{0}={1}", SoqlQuery.WhereKey, last);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Where(first)
                     .Where(second)
                     .Where(last);

            StringAssert.Contains(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Order_Orders_By_DefaultOrder_In_DefaultOrderDirection()
        {
            string defaultOrderClause = String.Format("{0}={1} {2}", SoqlQuery.OrderKey, String.Join(SoqlQuery.Delimiter, SoqlQuery.DefaultOrder), SoqlQuery.DefaultOrderDirection);

            string emptyGroup = new SoqlQuery().Order("").ToString();
            string manyEmptyGroup = new SoqlQuery().Order("", "", "").ToString();
            string nullGroup = new SoqlQuery().Order(null).ToString();

            StringAssert.Contains(defaultOrderClause, emptyGroup);
            StringAssert.Contains(defaultOrderClause, manyEmptyGroup);
            StringAssert.Contains(defaultOrderClause, nullGroup);
        }
        
        [TestCase(SoqlOrderDirection.DESC, "column1", "")]
        [TestCase(SoqlOrderDirection.ASC, "column1", "", "column2")]
        [Category("SoqlQuery")]
        public void Order_Clause_Gets_Order_Direction_And_Valid_Columns(SoqlOrderDirection direction, params string[] columns)
        {
            string expected = String.Format("{0}={1} {2}", SoqlQuery.OrderKey, String.Join(SoqlQuery.Delimiter, columns.Where(c => !String.IsNullOrEmpty(c))), direction);

            string soql = new SoqlQuery().Order(direction, columns).ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Order_Overwrites_All_Previous()
        {
            string[] first = { "first" };
            string[] second = { "first", "second" };
            string[] last = { "first", "second", "last" };

            string expected = String.Format("{0}={1}", SoqlQuery.OrderKey, String.Join(SoqlQuery.Delimiter, last));

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Order(first)
                     .Order(second)
                     .Order(last);

            StringAssert.Contains(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Group_Ignores_Group_Clause()
        {
            string startOfGroupClause = String.Format("{0}=", SoqlQuery.GroupKey);

            string emptyGroup = new SoqlQuery().Group("").ToString();
            string manyEmptyGroup = new SoqlQuery().Group("", "", "").ToString();
            string nullGroup = new SoqlQuery().Group(null).ToString();

            StringAssert.DoesNotContain(startOfGroupClause, emptyGroup);
            StringAssert.DoesNotContain(startOfGroupClause, manyEmptyGroup);
            StringAssert.DoesNotContain(startOfGroupClause, nullGroup);
        }

        [TestCase("column1", "")]
        [TestCase("column1", "column2", "")]
        [Category("SoqlQuery")]
        public void Group_Clause_Only_Gets_Valid_Columns(params string[] columns)
        {
            string expected = String.Format("{0}={1}", SoqlQuery.GroupKey, String.Join(SoqlQuery.Delimiter, columns.Where(c => !String.IsNullOrEmpty(c))));

            string soql = new SoqlQuery().Group(columns).ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Group_Overwrites_All_Previous()
        {
            string[] first = { "first" };
            string[] second = { "first", "second" };
            string[] last = { "first", "second", "last" };

            string expected = String.Format("{0}={1}", SoqlQuery.GroupKey, String.Join(SoqlQuery.Delimiter, last));

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Group(first)
                     .Group(second)
                     .Group(last);

            StringAssert.Contains(expected, soqlQuery.ToString());
        }

        [TestCase(-1)]
        [TestCase(0)]
        [Category("SoqlQuery")]
        public void Limit_Clause_Ignores_Limits_Below_One(int limit)
        {
            string startOfLimitClause = String.Format("{0}=", SoqlQuery.LimitKey);
            
            string soql = new SoqlQuery().Limit(limit).ToString();

            StringAssert.DoesNotContain(startOfLimitClause, soql);
        }

        [TestCase(1001)]
        [TestCase(9999)]
        [Category("SoqlQuery")]
        public void Limit_Clause_Has_A_Ceiling_At_MaximumLimit(int limit)
        {
            string expected = String.Format("{0}={1}", SoqlQuery.LimitKey, Math.Min(limit, SoqlQuery.MaximumLimit));

            string soql = new SoqlQuery().Limit(limit).ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Limit_Overwrites_All_Previous()
        {
            int first = 1;
            int second = 2;
            int last = 3;

            string expected = String.Format("{0}={1}", SoqlQuery.LimitKey, last);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Limit(first)
                     .Limit(second)
                     .Limit(last);

            StringAssert.Contains(expected, soqlQuery.ToString());
        }

        [TestCase(-1)]
        [TestCase(-999)]
        [TestCase(1)]
        [TestCase(999)]
        [Category("SoqlQuery")]
        public void Offset_Clause_Gets_Absolute_Offset(int offset)
        {
            string expected = String.Format("{0}={1}", SoqlQuery.OffsetKey, Math.Abs(offset));

            string soql = new SoqlQuery().Offset(offset).ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Offset_Overwrites_All_Previous()
        {
            int first = 1;
            int second = 2;
            int last = 3;

            string expected = String.Format("{0}={1}", SoqlQuery.OffsetKey, last);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Offset(first)
                     .Offset(second)
                     .Offset(last);

            StringAssert.Contains(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_FullTextSearch_Ignores_Search_Clause()
        {
            string startOfSearchClause = String.Format("{0}=", SoqlQuery.SearchKey);

            string emptySearch = new SoqlQuery().FullTextSearch("").ToString();
            string nullSearch = new SoqlQuery().FullTextSearch(null).ToString();

            StringAssert.DoesNotContain(startOfSearchClause, emptySearch);
            StringAssert.DoesNotContain(startOfSearchClause, nullSearch);
        }

        [TestCase("search text")]
        [TestCase("query")]
        [TestCase("find something")]
        [Category("SoqlQuery")]
        public void Search_Clause_Gets_FullTextSearch(string searchText)
        {
            string expected = String.Format("{0}={1}", SoqlQuery.SearchKey, searchText);

            string soql = new SoqlQuery().FullTextSearch(searchText).ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Search_Clause_Gets_FormattedInput()
        {
            string format = "search term is {0}";

            string expected = String.Format("{0}={1}", SoqlQuery.SearchKey, String.Format(format, "test"));

            string soql = new SoqlQuery().FullTextSearch(format, "test").ToString();

            StringAssert.Contains(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_FullTextSearch_Overwrites_All_Previous()
        {
            string first = "first text";
            string second = "second text";
            string last = "last text";

            string expected = String.Format("{0}={1}", SoqlQuery.SearchKey, last);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.FullTextSearch(first)
                     .FullTextSearch(second)
                     .FullTextSearch(last);

            StringAssert.Contains(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void All_Query_Methods_Return_The_Original_Instance()
        {
            var original = new SoqlQuery();
            var select = original.Select("something");
            var where = original.Where("something");
            var order = original.Order(SoqlOrderDirection.DESC, "something");
            var group = original.Group("something");
            var limit = original.Limit(10);
            var offset = original.Offset(10);
            var search = original.FullTextSearch("something");

            Assert.AreSame(original, select);
            Assert.AreSame(original, where);
            Assert.AreSame(original, order);
            Assert.AreSame(original, group);
            Assert.AreSame(original, limit);
            Assert.AreSame(original, offset);
            Assert.AreSame(original, search);
        }
    }
}