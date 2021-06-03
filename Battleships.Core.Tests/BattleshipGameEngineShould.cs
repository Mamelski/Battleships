using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Models;
using Battleships.Core.Models.Ships;
using Battleships.Core.Utils;
using Shouldly;
using Xunit;

namespace Battleships.Core.Tests
{
    public class BattleshipGameEngineShould
    {
        private readonly List<Coordinate> _allCoordinates = CoordinateHelper.GetAllCoordinates();

        private readonly PlacedShip _placedShip = new(ShipClass.Destroyer, new List<Coordinate>
        {
            new(0, 0),
            new(0, 1),
            new(0, 2),
            new(0, 3)
        });

        [Fact]
        public void ReturnCorrectStateAfterIllegalShot()
        {
            var sut = new BattleshipGameEngine();
            sut.SetupBoard(new[] {_placedShip});
            
            var shot = new Coordinate(1, 1);

            var firstResult = sut.Shoot(shot);
            var resultAfterIllegalMove = sut.Shoot(shot);

            resultAfterIllegalMove.ShotResult.ShouldBe(ShotResult.Illegal);
            resultAfterIllegalMove.IsGameFinished.ShouldBe(firstResult.IsGameFinished);
            resultAfterIllegalMove.Board.ShouldBe(firstResult.Board);
        }

        [Fact]
        public void ReturnCorrectStateAfterMissShot()
        {
            var sut = new BattleshipGameEngine();
            sut.SetupBoard(new[] {_placedShip});
            
            var shot = new Coordinate(1, 1);

            var result = sut.Shoot(shot);
            
            result.Board[shot].ShouldBe(CoordinateState.Miss);

            foreach (var coordinate in _allCoordinates.Except(new[] {shot}))
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            result.ShotResult.ShouldBe(ShotResult.Miss);
            result.IsGameFinished.ShouldBe(false);
        }
        
        [Fact]
        public void ReturnCorrectStateAfterHitShot()
        {
            var sut = new BattleshipGameEngine();
            sut.SetupBoard(new[] {_placedShip});
            
            var shot = new Coordinate(0, 0);

            var result = sut.Shoot(shot);
            
            result.Board[shot].ShouldBe(CoordinateState.Hit);

            foreach (var coordinate in _allCoordinates.Except(new[] {shot}))
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            result.ShotResult.ShouldBe(ShotResult.Hit);
            result.IsGameFinished.ShouldBe(false);
        }
        
        [Fact]
        public void ReturnCorrectStateAfterSinkTheLastShipShot()
        {
            var sut = new BattleshipGameEngine();
            sut.SetupBoard(new[] {_placedShip});

            GameState result = null;
            foreach (var coordinate in _placedShip.Coordinates)
            {
                result = sut.Shoot(coordinate);
            }
            
            result.ShouldNotBe(null);

            var surroundingCoordinates = CoordinateHelper.GetSurroundingCoordinates(_placedShip.Coordinates);
            
            foreach (var coordinate in surroundingCoordinates)
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Miss);
            }

            foreach (var coordinate in _allCoordinates
                .Except(_placedShip.Coordinates)
                .Except(surroundingCoordinates))
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            foreach (var coordinate in _placedShip.Coordinates)
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Sunk);
            }

            result.ShotResult.ShouldBe(ShotResult.Sink);
            result.IsGameFinished.ShouldBe(true);
        }
    }
}