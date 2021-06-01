using Battleships.Core.Models;

namespace Battleships.Core
{
    public class GameManager
    {
        private readonly RandomShipPlacer _randomShipPlacer = new();
        private BattleshipGameEngine _battleshipGameEngine;
        
        public void StartGame()
        {
            var unplacedShips = Shipyard.ProduceShips();
            var placedShips = _randomShipPlacer.PlaceShips(unplacedShips);

            _battleshipGameEngine = new BattleshipGameEngine(placedShips);
        }

        public GameState Shoot(Coordinate coordinate)
        {
            return _battleshipGameEngine.Shoot(coordinate);
        }
    }
}