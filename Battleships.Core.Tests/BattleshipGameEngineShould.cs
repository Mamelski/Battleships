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

            var firstGameState = sut.Shoot(shot);
            var gameStateAfterIllegalMove = sut.Shoot(shot);

            gameStateAfterIllegalMove.ShotResult.ShouldBe(ShotResult.Illegal);
            gameStateAfterIllegalMove.IsGameFinished.ShouldBe(firstGameState.IsGameFinished);
            gameStateAfterIllegalMove.Board.ShouldBe(firstGameState.Board);

            var expectedShip = new FightingShip(_placedShip.ShipClass, _placedShip.Coordinates);
            AssertShips(gameStateAfterIllegalMove, expectedShip);
        }

        [Fact]
        public void ReturnCorrectStateAfterMissShot()
        {
            var sut = new BattleshipGameEngine();
            sut.SetupBoard(new[] {_placedShip});
            
            var shot = new Coordinate(1, 1);

            var gameState = sut.Shoot(shot);
            
            gameState.Board[shot].ShouldBe(CoordinateState.Miss);

            foreach (var coordinate in _allCoordinates.Except(new[] {shot}))
            {
                gameState.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            gameState.ShotResult.ShouldBe(ShotResult.Miss);
            gameState.IsGameFinished.ShouldBe(false);
            
            var expectedShip = new FightingShip(_placedShip.ShipClass, _placedShip.Coordinates);
            AssertShips(gameState, expectedShip);
        }
        
        [Fact]
        public void ReturnCorrectStateAfterHitShot()
        {
            var sut = new BattleshipGameEngine();
            sut.SetupBoard(new[] {_placedShip});
            
            var shot = new Coordinate(0, 0);

            var gameState = sut.Shoot(shot);
            
            gameState.Board[shot].ShouldBe(CoordinateState.Hit);

            foreach (var coordinate in _allCoordinates.Except(new[] {shot}))
            {
                gameState.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            gameState.ShotResult.ShouldBe(ShotResult.Hit);
            gameState.IsGameFinished.ShouldBe(false);

            var expectedShip = new FightingShip(_placedShip.ShipClass, _placedShip.Coordinates);
            expectedShip.NotHitCoordinates.Remove(shot);
            
            AssertShips(gameState, expectedShip);
        }
        
        [Fact]
        public void ReturnCorrectStateAfterSinkTheLastShipShot()
        {
            var sut = new BattleshipGameEngine();
            sut.SetupBoard(new[] {_placedShip});

            GameState gameState = null;
            foreach (var coordinate in _placedShip.Coordinates)
            {
                gameState = sut.Shoot(coordinate);
            }
            
            gameState.ShouldNotBe(null);

            var surroundingCoordinates = CoordinateHelper.GetSurroundingCoordinates(_placedShip.Coordinates);
            
            foreach (var coordinate in surroundingCoordinates)
            {
                gameState.Board[coordinate].ShouldBe(CoordinateState.Miss);
            }

            foreach (var coordinate in _allCoordinates
                .Except(_placedShip.Coordinates)
                .Except(surroundingCoordinates))
            {
                gameState.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            foreach (var coordinate in _placedShip.Coordinates)
            {
                gameState.Board[coordinate].ShouldBe(CoordinateState.Sunk);
            }

            gameState.ShotResult.ShouldBe(ShotResult.Sink);
            gameState.IsGameFinished.ShouldBe(true);
            
            var expectedShip = new FightingShip(_placedShip.ShipClass, _placedShip.Coordinates);
            expectedShip.NotHitCoordinates.Clear();
            
            AssertShips(gameState, expectedShip);
        }

        private void AssertShips(GameState gameState, FightingShip expected)
        {
            gameState.FightingShips.Count.ShouldBe(1);
            
            var actual = gameState.FightingShips.Single();
            
            actual.IsSunk.ShouldBe(expected.IsSunk);
            actual.Size.ShouldBe(expected.Size);
            actual.ShipClass.ShouldBe(expected.ShipClass);
            actual.Coordinates.ShouldBeEquivalentTo(expected.Coordinates);
            actual.NotHitCoordinates.ShouldBeEquivalentTo(expected.NotHitCoordinates);
        }
    }
}