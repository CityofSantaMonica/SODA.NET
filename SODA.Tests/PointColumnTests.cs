using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;
namespace SODA.Tests
{
    [TestFixture]
    [Category("PointColumn")]
    public class PointColumnTests
    {
        [Test]
        public void Can_Serialize_Point_Feature()
        {
            var point = new PointColumn(new Positions(new[] {-87.653274,41.936172}));

            var expectedJSON = "{\"type\":\"Point\",\"coordinates\":[-87.653274,41.936172]}";

            var actualJSON = JsonConvert.SerializeObject(point);

            Assert.AreEqual(expectedJSON, actualJSON);
        }

        [Test]
        public void Coordinates_In_PositionsClass_And_PointColumnClass_Are_Equal()
        {
            var array = new[] {-87.653274, 41.936172};
            var positionFromConstructor = new PointColumn(array);
            var positionFromPositionsClass = new PointColumn(new Positions(array));

            Assert.AreEqual(positionFromConstructor.Coordinates.PositionsArray[0], positionFromPositionsClass.Coordinates.PositionsArray[0]);
            Assert.AreEqual(positionFromConstructor.Coordinates.PositionsArray[1], positionFromPositionsClass.Coordinates.PositionsArray[1]);
        }

        [Test]
        public void Can_Deserialize_Point_Feature()
        {
            var json = "{\"type\":\"Point\",\"coordinates\":[-87.653274,41.936172]}";

            var actualPointColumn = JsonConvert.DeserializeObject<PointColumn>(json);

            Assert.AreEqual(-87.653274, actualPointColumn.Coordinates.PositionsArray[0]);
            Assert.AreEqual(41.936172, actualPointColumn.Coordinates.PositionsArray[1]);
        }
    }
}