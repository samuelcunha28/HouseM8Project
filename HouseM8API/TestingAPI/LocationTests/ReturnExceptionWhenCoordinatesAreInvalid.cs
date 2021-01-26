using System.Net.Http;
using Xunit;

namespace HouseM8APITests.LocationTests
{
    [Collection("Sequential")]
    public class ReturnExceptionWhenCoordinatesAreInvalid
    {
        [Fact]
        public void ReturnExceptionWhenCoordinatesAreInvalidTest()
        {
            Assert.Throws<HttpRequestException>(() => HouseM8API.Helpers.DistancesHelper.getAddressFromCoordinates("834729183479832749837249379842749", "823749827948237489"));
        }
    }
}
