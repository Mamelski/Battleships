using Shouldly;
using Xunit;

namespace Battleships.Core.Tests
{
    public class ShipyardShould
    {
        [Fact]
        public void ProduceSomeShips()
        {
            var ships = Shipyard.ProduceShips();
            
            ships.ShouldNotBeEmpty();
        }
    }
}