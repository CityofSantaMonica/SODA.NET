using System;
using System.Linq;
using NUnit.Framework;

namespace SODA.Tests
{
    [TestFixture]
    public class SoqlQueryTests
    {
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
            string[] first = { "first", "second", "last" };
            string[] second = { "first", "second" };
            string[] last =  { "last" };
            string format = String.Format("{0}={{0}}", SoqlQuery.SelectKey);

            string soql = new SoqlQuery().Select(first)
                                         .Select(second)
                                         .Select(last)
                                         .ToString();

            StringAssert.DoesNotContain(String.Format(format, String.Join(SoqlQuery.Delimiter, first)), soql);
            StringAssert.DoesNotContain(String.Format(format, String.Join(SoqlQuery.Delimiter, second)), soql);
            StringAssert.Contains(String.Format(format, String.Join(SoqlQuery.Delimiter, last)), soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Aliases_Are_Ignored()
        {
            string[] columns = new[] { "column1", "column2" };
            string[] startOfAliases = columns.Select(c => String.Format("{0} AS ", c)).ToArray();

            string[] nullAliases = new[] { (string)null, (string)null };
            string[] emptyAliases = new[] { "", "" };

            string nullSoql = new SoqlQuery().Select(columns).As(nullAliases).ToString();
            string emptySoql = new SoqlQuery().Select(columns).As(emptyAliases).ToString();

            foreach (string startOfAlias in startOfAliases)
            {
                StringAssert.DoesNotContain(startOfAlias, nullSoql);
                StringAssert.DoesNotContain(startOfAlias, emptySoql);
            }
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
        public void Select_Clause_With_Aliases_Generates_Valid_SoQL()
        {
            string[] columns = new[] { "column1", "column2" };
            string[] aliases = new[] { "column_a", "column_b" };

            string expected = String.Format(@"{0} AS {1},{2} AS {3}",
                                            columns[0],
                                            aliases[0],
                                            columns[1],
                                            aliases[1]);

            string soql = new SoqlQuery().Select(columns).As(aliases).ToString();

            StringAssert.IsMatch(expected, soql);
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
            string format = String.Format("{0}={{0}}", SoqlQuery.WhereKey);

            string expected = String.Format(format, last);

            string soql = new SoqlQuery().Where(first)
                                         .Where(second)
                                         .Where(last)
                                         .ToString();

            StringAssert.DoesNotContain(String.Format(format, first), soql);
            StringAssert.DoesNotContain(String.Format(format, second), soql);
            StringAssert.Contains(String.Format(format, last), soql);
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
            string[] first = { "first", "second", "last" };
            string[] second = { "first", "second" };
            string[] last = { "last" };
            string format = String.Format("{0}={{0}}", SoqlQuery.OrderKey);

            string soql = new SoqlQuery().Order(first)
                                         .Order(second)
                                         .Order(last)
                                         .ToString();

            StringAssert.DoesNotContain(String.Format(format, String.Join(SoqlQuery.Delimiter, first)), soql);
            StringAssert.DoesNotContain(String.Format(format, String.Join(SoqlQuery.Delimiter, second)), soql);
            StringAssert.Contains(String.Format(format, String.Join(SoqlQuery.Delimiter, last)), soql);
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
            string[] first = { "first", "second", "last" };
            string[] second = { "first", "second" };
            string[] last = { "last" };
            string format = String.Format("{0}={{0}}", SoqlQuery.GroupKey);

            string soql = new SoqlQuery().Group(first)
                                         .Group(second)
                                         .Group(last)
                                         .ToString();

            StringAssert.DoesNotContain(String.Format(format, String.Join(SoqlQuery.Delimiter, first)), soql);
            StringAssert.DoesNotContain(String.Format(format, String.Join(SoqlQuery.Delimiter, second)), soql);
            StringAssert.Contains(String.Format(format, String.Join(SoqlQuery.Delimiter, last)), soql);
        }

        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(0)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SoqlQuery")]
        public void Limit_Less_Than_One_Throws_ArgumentOutOfRangeException(int limit)
        {
            var soql = new SoqlQuery().Limit(limit);
        }

        [TestCase(50001)]
        [TestCase(59999)]
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
            string format = String.Format("{0}={{0}}", SoqlQuery.LimitKey);
            
            string soql = new SoqlQuery().Limit(first)
                                         .Limit(second)
                                         .Limit(last)
                                         .ToString();

            StringAssert.DoesNotContain(String.Format(format, first), soql);
            StringAssert.DoesNotContain(String.Format(format, second), soql);
            StringAssert.Contains(String.Format(format, last), soql);
        }

        [TestCase(-999)]
        [TestCase(-1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SoqlQuery")]
        public void Offset_Less_Than_Zero_Throws_ArgumentOutOfRangeException(int offset)
        {
            var soql = new SoqlQuery().Offset(offset);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Offset_Overwrites_All_Previous()
        {
            int first = 1;
            int second = 2;
            int last = 3;
            string format = String.Format("{0}={{0}}", SoqlQuery.OffsetKey);

            string soql = new SoqlQuery().Offset(first)
                                         .Offset(second)
                                         .Offset(last)
                                         .ToString();

            StringAssert.DoesNotContain(String.Format(format, first), soql);
            StringAssert.DoesNotContain(String.Format(format, second), soql);
            StringAssert.Contains(String.Format(format, last), soql);
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
            string format = String.Format("{0}={{0}}", SoqlQuery.SearchKey);
            
            string soql = new SoqlQuery().FullTextSearch(first)
                                         .FullTextSearch(second)
                                         .FullTextSearch(last)
                                         .ToString();

            StringAssert.DoesNotContain(String.Format(format, first), soql);
            StringAssert.DoesNotContain(String.Format(format, second), soql);
            StringAssert.Contains(String.Format(format, last), soql);
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
