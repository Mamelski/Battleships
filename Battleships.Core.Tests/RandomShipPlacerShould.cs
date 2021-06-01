using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Models.Ships;
using Battleships.Core.Utils;
using Shouldly;
using Xunit;

namespace Battleships.Core.Tests
{
    public class RandomShipPlacerShould
    {
        private readonly List<UnplacedShip> _unplacedShips =  new ()
        {
            new UnplacedShip(ShipClass.Battleship),
            new UnplacedShip(ShipClass.Destroyer),
            new UnplacedShip(ShipClass.Destroyer)
        };
        
        private readonly RandomShipPlacer _sut = new();

        [Fact]
        public void ReturnTheSameNumberOfPlacedShipsAsGivenNumberOfUnplacedShips()
        {
            var placedShips = _sut.PlaceShips(_unplacedShips);
            
            placedShips.Count.ShouldBe(_unplacedShips.Count);
        }
        
        [Fact]
        public void AssignCorrectTypesToPlacedShips()
        {
            var placedShips = _sut.PlaceShips(_unplacedShips);

            var placedShipTypes = placedShips.Select(ship => ship.ShipClass);
            var expectedTypes = _unplacedShips.Select(ship => ship.ShipClass);
            
            placedShipTypes.ShouldBe(expectedTypes);
        }
        
        [Fact]
        public void AssignNumberOfCoordinatesEqualToSizeOfShipToPlacedShips()
        {
            var placedShips = _sut.PlaceShips(_unplacedShips);

            placedShips.ShouldAllBe(ship => ship.Coordinates.Count == ship.Size);
        }
        
        [Fact]
        public void AssignDistinctCoordinatesToPlacedShips()
        {
            var placedShips = _sut.PlaceShips(_unplacedShips);

            var allCoordinates = placedShips.SelectMany(ship => ship.Coordinates).ToList();
            var distinctCoordinates = allCoordinates.Distinct().ToList();
            
            allCoordinates.ShouldBe(distinctCoordinates);
        }
        
        [Fact]
        public void AssignCoordinatesThatAreLineToPlacedShips()
        {
            var placedShips = _sut.PlaceShips(_unplacedShips);

            foreach (var placedShip in placedShips)
            {
                var distinctRows = placedShip.Coordinates.Select(coordinate => coordinate.Row).Distinct().ToList();
                var distinctColumns = placedShip.Coordinates.Select(coordinate => coordinate.Column).Distinct().ToList();

                var isSingleRowXorColumn = (distinctRows.Count == 1) ^ (distinctColumns.Count == 1);

                isSingleRowXorColumn.ShouldBe(true);
            }
        }
        
        [Fact]
        public void AssignCoordinatesThatAreInRangeInPlacedShips()
        {
            var placedShips = _sut.PlaceShips(_unplacedShips);

            foreach (var placedShip in placedShips)
            {
               placedShip.Coordinates.ShouldAllBe(coordinate => coordinate.Row >= 0 && coordinate.Row <= Consts.Rows);
               placedShip.Coordinates.ShouldAllBe(coordinate => coordinate.Column >= 0 && coordinate.Row <= Consts.Columns);
            }
        }
    }
}