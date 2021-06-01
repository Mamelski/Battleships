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
        private readonly List<Coordinate> _allCoordinates = GetListOfAllCoordinates();

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
            var sut = new BattleshipGameEngine(new[] {_placedShip});
            var shot = new Coordinate(1, 1);

            var firstResult = sut.MakeMove(shot);
            var resultAfterIllegalMove = sut.MakeMove(shot);

            resultAfterIllegalMove.MoveResult.ShouldBe(MoveResult.Illegal);
            resultAfterIllegalMove.IsGameFinished.ShouldBe(firstResult.IsGameFinished);
            resultAfterIllegalMove.Board.ShouldBe(firstResult.Board);
        }

        [Fact]
        public void ReturnCorrectStateAfterMissShot()
        {
            var sut = new BattleshipGameEngine(new[] {_placedShip});
            var shot = new Coordinate(1, 1);

            var result = sut.MakeMove(shot);
            
            result.Board[shot].ShouldBe(CoordinateState.Miss);

            foreach (var coordinate in _allCoordinates.Except(new[] {shot}))
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            result.MoveResult.ShouldBe(MoveResult.Miss);
            result.IsGameFinished.ShouldBe(false);
        }
        
        [Fact]
        public void ReturnCorrectStateAfterHitShot()
        {
            var sut = new BattleshipGameEngine(new[] {_placedShip});
            var shot = new Coordinate(0, 0);

            var result = sut.MakeMove(shot);
            
            result.Board[shot].ShouldBe(CoordinateState.Hit);

            foreach (var coordinate in _allCoordinates.Except(new[] {shot}))
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            result.MoveResult.ShouldBe(MoveResult.Hit);
            result.IsGameFinished.ShouldBe(false);
        }
        
        [Fact]
        public void ReturnCorrectStateAfterSinkTheLastShipShot()
        {
            var sut = new BattleshipGameEngine(new[] {_placedShip});

            GameState result = null;
            foreach (var coordinate in _placedShip.Coordinates)
            {
                result = sut.MakeMove(coordinate);
            }
            
            result.ShouldNotBe(null);
            
            foreach (var coordinate in _allCoordinates.Except(_placedShip.Coordinates))
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Unknown);
            }
            
            foreach (var coordinate in _placedShip.Coordinates)
            {
                result.Board[coordinate].ShouldBe(CoordinateState.Sunk);
            }

            result.MoveResult.ShouldBe(MoveResult.Sink);
            result.IsGameFinished.ShouldBe(true);
        }
        
        private static List<Coordinate> GetListOfAllCoordinates()
        {
            var allCoordinates = new List<Coordinate>();

            for (var row = 0; row < Consts.Rows; row++)
            {
                for (var column = 0; column < Consts.Columns; column++)
                {
                    allCoordinates.Add(new Coordinate(row,column));
                }
            }

            return allCoordinates;
        }
    }
}