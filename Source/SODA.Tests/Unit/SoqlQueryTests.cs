using System;
using System.Linq;
using NUnit.Framework;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class SoqlQueryTests
    {
        private static string selectStar = String.Format("{0}={1}", SoqlQuery.SelectKey, "*");
        
        [Test]
        [Category("SoqlQuery")]
        public void New_Soql_Selects_Star()
        {
            string soql = new SoqlQuery().ToString();
            
            Assert.AreEqual(selectStar, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Select_Selects_Star()
        {
            string emptySelect = new SoqlQuery().Select("").ToString();
            string manyEmptySelect = new SoqlQuery().Select("", "", "").ToString();
            string nullSelect = new SoqlQuery().Select(null).ToString();

            Assert.AreEqual(selectStar, emptySelect);
            Assert.AreEqual(selectStar, nullSelect);
        }

        [TestCase("column1", "")]
        [TestCase("column1", "", "column2")]
        [Category("SoqlQuery")]
        public void Select_Clause_Only_Gets_Valid_Columns(params string[] columns)
        {
            string expected = String.Format("{0}={1}", SoqlQuery.SelectKey, String.Join(SoqlQuery.Delimiter, columns.Where(c => !String.IsNullOrEmpty(c))));

            string soql = new SoqlQuery().Select(columns).ToString();

            Assert.AreEqual(expected, soql);
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

            Assert.AreEqual(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Where_Ignores_Where_Clause()
        {
            string emptyWhere = new SoqlQuery().Where("").ToString();
            string nullWhere = new SoqlQuery().Where(null).ToString();

            Assert.AreEqual(selectStar, emptyWhere);
            Assert.AreEqual(selectStar, nullWhere);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Where_Clause_Gets_Valid_Predicate()
        {
            string predicate = "something > nothing";

            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.WhereKey, predicate);

            string soql = new SoqlQuery().Where(predicate).ToString();

            Assert.AreEqual(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Where_Overwrites_All_Previous()
        {
            string first = "first > 0";
            string second = "second > first";
            string last = "last > anything";

            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.WhereKey, last);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Where(first)
                     .Where(second)
                     .Where(last);

            Assert.AreEqual(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Order_Ignores_Order_Clause()
        {
            string emptyGroup = new SoqlQuery().Order("").ToString();
            string manyEmptyGroup = new SoqlQuery().Order("", "", "").ToString();
            string nullGroup = new SoqlQuery().Order(null).ToString();

            Assert.AreEqual(selectStar, emptyGroup);
            Assert.AreEqual(selectStar, manyEmptyGroup);
            Assert.AreEqual(selectStar, nullGroup);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Default_Sort_Order_Is_Ascending()
        {
            string[] columns = { "column1", "column2" };

            string expected = String.Format("{0}&{1}={2} {3}", selectStar, SoqlQuery.OrderKey, String.Join(SoqlQuery.Delimiter, columns), SortOrder.ASC);

            string soql = new SoqlQuery().Order(columns).ToString();

            Assert.AreEqual(expected, soql);
        }

        [TestCase(SortOrder.DESC, "column1", "")]
        [TestCase(SortOrder.ASC, "column1", "", "column2")]
        [Category("SoqlQuery")]
        public void Order_Clause_Gets_Sort_Order_And_Only_Gets_Valid_Columns(SortOrder sortOrder, params string[] columns)
        {
            string expected = String.Format("{0}&{1}={2} {3}", selectStar, SoqlQuery.OrderKey, String.Join(SoqlQuery.Delimiter, columns.Where(c => !String.IsNullOrEmpty(c))), sortOrder);

            string soql = new SoqlQuery().Order(sortOrder, columns).ToString();

            Assert.AreEqual(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Order_Overwrites_All_Previous()
        {
            string[] first = { "first" };
            string[] second = { "first", "second" };
            string[] last = { "first", "second", "last" };
            SortOrder sortOrder = SortOrder.ASC;

            string expected = String.Format("{0}&{1}={2} {3}", selectStar, SoqlQuery.OrderKey, String.Join(SoqlQuery.Delimiter, last), sortOrder);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Order(sortOrder, first)
                     .Order(sortOrder, second)
                     .Order(sortOrder, last);

            Assert.AreEqual(expected, soqlQuery.ToString());
        }

        [Test]
        [Category("SoqlQuery")]
        public void Empty_Group_Ignores_Group_Clause()
        {
            string emptyGroup = new SoqlQuery().Group("").ToString();
            string manyEmptyGroup = new SoqlQuery().Group("", "", "").ToString();
            string nullGroup = new SoqlQuery().Group(null).ToString();

            Assert.AreEqual(selectStar, emptyGroup);
            Assert.AreEqual(selectStar, manyEmptyGroup);
            Assert.AreEqual(selectStar, nullGroup);
        }

        [TestCase("column1", "")]
        [TestCase("column1", "column2", "")]
        [Category("SoqlQuery")]
        public void Group_Clause_Only_Gets_Valid_Columns(params string[] columns)
        {
            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.GroupKey, String.Join(SoqlQuery.Delimiter, columns.Where(c => !String.IsNullOrEmpty(c))));

            string soql = new SoqlQuery().Group(columns).ToString();

            Assert.AreEqual(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Group_Overwrites_All_Previous()
        {
            string[] first = { "first" };
            string[] second = { "first", "second" };
            string[] last = { "first", "second", "last" };

            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.GroupKey, String.Join(SoqlQuery.Delimiter, last));

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Group(first)
                     .Group(second)
                     .Group(last);

            Assert.AreEqual(expected, soqlQuery.ToString());
        }

        [TestCase(-1)]
        [TestCase(-999)]
        [TestCase(1)]
        [TestCase(999)]
        [Category("SoqlQuery")]
        public void Limit_Clause_Gets_Absolute_Limit(int limit)
        {
            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.LimitKey, Math.Abs(limit));

            string soql = new SoqlQuery().Limit(limit).ToString();

            Assert.AreEqual(expected, soql);
        }

        [TestCase(1000)]
        [TestCase(1001)]
        [TestCase(9999)]
        [Category("SoqlQuery")]
        public void Limit_Max_Is_1000(int limit)
        {
            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.LimitKey, 1000);

            string soql = new SoqlQuery().Limit(limit).ToString();

            Assert.AreEqual(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Limit_Overwrites_All_Previous()
        {
            int first = 1;
            int second = 2;
            int last = 3;

            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.LimitKey, last);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Limit(first)
                     .Limit(second)
                     .Limit(last);

            Assert.AreEqual(expected, soqlQuery.ToString());
        }

        [TestCase(-1)]
        [TestCase(-999)]
        [TestCase(1)]
        [TestCase(999)]
        [Category("SoqlQuery")]
        public void Offset_Clause_Gets_Absolute_Offset(int offset)
        {
            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.OffsetKey, Math.Abs(offset));

            string soql = new SoqlQuery().Offset(offset).ToString();

            Assert.AreEqual(expected, soql);
        }

        [Test]
        [Category("SoqlQuery")]
        public void Last_Offset_Overwrites_All_Previous()
        {
            int first = 1;
            int second = 2;
            int last = 3;

            string expected = String.Format("{0}&{1}={2}", selectStar, SoqlQuery.OffsetKey, last);

            SoqlQuery soqlQuery = new SoqlQuery();

            soqlQuery.Offset(first)
                     .Offset(second)
                     .Offset(last);

            Assert.AreEqual(expected, soqlQuery.ToString());
        }
    }
}