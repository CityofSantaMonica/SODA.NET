
using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;

namespace SODA.Tests
{
    [TestFixture]
    [Category("Positions")]
    public class PointColumnTests
    {
        [Test]
        public void Can_Serialize_Point_Feature()
        {
            var point = new PointColumn(new Positions(new[] {-87.653274,41.936172}));

            string expectedJSON = "{\"type\":\"Point\",\"coordinates\":[-87.653274,41.936172]}";

            string actualJSON = JsonConvert.SerializeObject(point);

            Assert.AreEqual(actualJSON, expectedJSON);
            Assert.IsNotNull(actualJSON);
        }


        [Test]
        public void Can_Deserialize_Point_Feature()
        {
            var json = "{\"type\":\"Point\",\"coordinates\":[-87.653274,41.936172]}";

            var actualPointColumn = JsonConvert.DeserializeObject<PointColumn>(json);

            Assert.IsNotNull(actualPointColumn);
            Assert.AreEqual(actualPointColumn.Coordinates.PositionsArray[0], -87.653274);
            Assert.AreEqual(actualPointColumn.Coordinates.PositionsArray[1], 41.936172);                
        }        
    }
}

