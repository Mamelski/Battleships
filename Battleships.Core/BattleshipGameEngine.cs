using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Models;
using Battleships.Core.Models.Ships;

namespace Battleships.Core
{
    public interface IBattleshipGameEngine
    {
        GameState Shoot(Coordinate shotCoordinate);
        Board SetupBoard(IEnumerable<PlacedShip> placedShips);
    }

    public class BattleshipGameEngine : IBattleshipGameEngine
    {
        private readonly Board _board = new();
        private readonly List<FightingShip> _fightingShips = new();
        
        public Board SetupBoard(IEnumerable<PlacedShip> placedShips)
        {
            foreach (var placedShip in placedShips)
            {
                var fightingShip = new FightingShip(
                    placedShip.ShipClass,
                    placedShip.Coordinates);
                
                _fightingShips.Add(fightingShip);
            }

            return _board;
        }

        public GameState Shoot(Coordinate shotCoordinate)
        {
            if (_board[shotCoordinate] != CoordinateState.Unknown)
            {
                return new GameState(
                    _board,
                    ShotResult.Illegal,
                    false);
            }

            var (moveResult, hitShip) = TryToHitShip(shotCoordinate);
            UpdateBoard(moveResult, hitShip, shotCoordinate);
            
            var isGameFinished = _fightingShips.All(fightingShip => fightingShip.IsSunk);

            return new GameState(
                _board,
                moveResult,
                isGameFinished);
        }

        private (ShotResult, FightingShip) TryToHitShip(Coordinate shotCoordinate)
        {
            var hitShip = FindHitShip(shotCoordinate);

            if (hitShip == null)
            {
                return (ShotResult.Miss, null);
            }
            
            hitShip.NotHitCoordinates.Remove(shotCoordinate);
            
            return hitShip.IsSunk 
                ? (ShotResult.Sink, hitShip)
                : (ShotResult.Hit, hitShip);
        }
        
        private FightingShip FindHitShip(Coordinate shotCoordinate) 
            => _fightingShips
                .SingleOrDefault(fightingShip => fightingShip.NotHitCoordinates
                    .Contains(shotCoordinate));
        
        private void UpdateBoard(
            ShotResult shotResult,
            FightingShip fightingShip,
            Coordinate shotCoordinate)
        {
            if (shotResult == ShotResult.Sink)
            {
                foreach (var sunkShipCoordinate in fightingShip.Coordinates)
                {
                    _board[sunkShipCoordinate] = CoordinateState.Sunk;
                }

                return;
            }
            
            _board[shotCoordinate] = shotResult switch
            {
                ShotResult.Miss => CoordinateState.Miss,
                ShotResult.Hit => CoordinateState.Hit,
                _ => _board[shotCoordinate]
            };
        }
    }
}