using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Models;
using Battleships.Core.Models.Ships;

namespace Battleships.Core.Engine
{
    public class BattleshipGameEngine
    {
        private readonly Board _board = new();
        private readonly List<FightingShip> _fightingShips = new();
        
        public BattleshipGameEngine(IEnumerable<PlacedShip> placedShips)
        {
            PrepareFightingShips(placedShips);
        }
        
        public GameState MakeMove(Coordinate shotCoordinate)
        {
            if (_board[shotCoordinate] != CoordinateState.Free)
            {
                return new GameState(
                    _board,
                    MoveResult.Illegal,
                    false);
            }

            var moveResult = TryToHitShip(shotCoordinate);
            UpdateBoard(moveResult, shotCoordinate);
            
            var isGameFinished = _fightingShips.All(fightingShip => fightingShip.IsSunk);

            return new GameState(
                _board,
                moveResult,
                isGameFinished);
        }
        
        private void PrepareFightingShips(IEnumerable<PlacedShip> placedShips)
        {
            foreach (var placedShip in placedShips)
            {
                var fightingShip = new FightingShip(
                    placedShip.ShipClass,
                    placedShip.Coordinates);
                
                _fightingShips.Add(fightingShip);
            }
        }

        private MoveResult TryToHitShip(Coordinate shotCoordinate)
        {
            var hitShip = FindHitShip(shotCoordinate);

            if (hitShip == null)
            {
                return MoveResult.Miss;
            }
            
            hitShip.NotHitCoordinates.Remove(shotCoordinate);
            
            return hitShip.IsSunk 
                ? MoveResult.Sink 
                : MoveResult.Hit;
        }
        
        private FightingShip FindHitShip(Coordinate shotCoordinate) 
            => _fightingShips
                .SingleOrDefault(fightingShip => fightingShip.NotHitCoordinates
                    .Contains(shotCoordinate));
        
        private void UpdateBoard(MoveResult moveResult, Coordinate shotCoordinate)
        {
            _board[shotCoordinate] = moveResult switch
            {
                MoveResult.Miss => CoordinateState.Miss,
                MoveResult.Hit => CoordinateState.Hit,
                MoveResult.Sink => CoordinateState.Sunk,
                _ => _board[shotCoordinate]
            };
        }
    }
}