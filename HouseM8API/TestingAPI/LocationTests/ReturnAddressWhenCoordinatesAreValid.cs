using Xunit;

namespace HouseM8APITests.LocationTests
{
    [Collection("Sequential")]
    public class ReturnAddressWhenCoordinatesAreValid
    {
        [Fact]
        public void ReturnExceptionWhenCoordinatesAreInvalidTest()
        {
            Assert.Equal("R. Eng. Luís Carneiro Leão 66, 4590-235 Figueiró, Portugal", HouseM8API.Helpers.DistancesHelper.getAddressFromCoordinates("41.3085168113476", "-8.344125348737023"));
        }
    }
}
