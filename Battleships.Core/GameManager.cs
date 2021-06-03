using System.Collections.Generic;
using Battleships.Core.Models;
using Battleships.Core.Models.Ships;

namespace Battleships.Core
{
    public class GameManager
    {
        private readonly IShipPlacer _shipPlacer;
        private readonly IBattleshipGameEngine _battleshipGameEngine;

        public GameManager(
            IShipPlacer shipPlacer,
            IBattleshipGameEngine battleshipGameEngine)
        {
            _shipPlacer = shipPlacer;
            _battleshipGameEngine = battleshipGameEngine;
        }

        public Board StartGame()
        {
            var unplacedShips = ProduceShips();
            var placedShips = _shipPlacer.PlaceShips(unplacedShips);

            return _battleshipGameEngine.SetupBoard(placedShips);
        }

        public GameState Shoot(Coordinate coordinate)
        {
            return _battleshipGameEngine.Shoot(coordinate);
        }

        private List<UnplacedShip> ProduceShips()
        {
            return new ()
            {
                new UnplacedShip(ShipClass.Battleship),
                new UnplacedShip(ShipClass.Destroyer),
                new UnplacedShip(ShipClass.Destroyer)
            };
        }
    }
}