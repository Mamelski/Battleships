using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Models;
using Battleships.Core.Models.Ships;
using Battleships.Core.ShipPlacement.Models;
using Battleships.Core.Utils;

namespace Battleships.Core.ShipPlacement
{
    public class RandomShipPlacementStrategy : IShipPlacementStrategy
    {
        private readonly Random _random = new();
        
        public void AssignCoordinatesToShips(List<Ship> ships)
        {
            var allFreeCoordinates = GetListOfAllFreeCoordinates();
            
            foreach (var ship in ships)
            {
                var coordinatesForShip = FindCoordinatesForShip(ship.Size, allFreeCoordinates);

                foreach (var coordinate in coordinatesForShip)
                {
                    allFreeCoordinates.Remove(coordinate);
                }

                ship.Coordinates = coordinatesForShip;
            }
        }

        private static List<Coordinate> GetListOfAllFreeCoordinates()
        {
            var freeCoordinates = new List<Coordinate>();

            for (var row = 0; row < Consts.Rows; row++)
            {
                for (var column = 0; column < Consts.Columns; column++)
                {
                    freeCoordinates.Add(new Coordinate(row,column));
                }
            }

            return freeCoordinates;
        }
        
        private List<Coordinate> FindCoordinatesForShip(
            int shipSize,
            ICollection<Coordinate> freeCoordinates)
        {
            var possibleStartingCoordinates = freeCoordinates
                .Select(c => c)
                .ToList();
            
            while(possibleStartingCoordinates.Any())
            {
                var startingCoordinateIndex = _random.Next(freeCoordinates.Count);
                var startingCoordinate = possibleStartingCoordinates.ElementAt(startingCoordinateIndex);

                var (isPlacementPossible, coordinates) = TryToPlaceShipFromStartingCoordinate(shipSize, startingCoordinate, freeCoordinates);

                if (isPlacementPossible)
                {
                    return coordinates;
                }
            }

            return new List<Coordinate>();
        }

        private (bool, List<Coordinate>) TryToPlaceShipFromStartingCoordinate(
            int shipSize,
            Coordinate startingPoint,
            ICollection<Coordinate> freeCoordinates)
        {
            var possiblePlacementDirections = new List<PlacementDirection>
            {
                PlacementDirection.Up,
                PlacementDirection.Down,
                PlacementDirection.Left,
                PlacementDirection.Right
            };
            
            var randomPossiblePlacementDirections = possiblePlacementDirections
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

            foreach (var direction in randomPossiblePlacementDirections)
            {
                var proposedPlacementCoordinates = GetCoordinatesOfPlacementProposition(startingPoint, shipSize, direction);
                if (IsPlacementPossible(proposedPlacementCoordinates, freeCoordinates))
                {
                    return (true, proposedPlacementCoordinates);
                }
            }

            return (false, new List<Coordinate>());
        }

        private static List<Coordinate> GetCoordinatesOfPlacementProposition(
            Coordinate startingPoint,
            int shipSize,
            PlacementDirection direction)
        {
            return direction switch
            {
                PlacementDirection.Up => GetCoordinatesForUpDirectionPlacement(startingPoint, shipSize),
                PlacementDirection.Down => GetCoordinatesForDownDirectionPlacement(startingPoint, shipSize),
                PlacementDirection.Left => GetCoordinatesForLeftDirectionPlacement(startingPoint, shipSize),
                PlacementDirection.Right => GetCoordinatesForRightDirectionPlacement(startingPoint, shipSize),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Not Supported direction")
            };
        }

        private static List<Coordinate> GetCoordinatesForUpDirectionPlacement(
            Coordinate startingPoint, 
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingPoint.Row + i, startingPoint.Column));
            }

            return coordinates;
        } 
        
        private static List<Coordinate> GetCoordinatesForDownDirectionPlacement(
            Coordinate startingPoint,
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingPoint.Row - i, startingPoint.Column));
            }

            return coordinates;
        }

        private static List<Coordinate> GetCoordinatesForLeftDirectionPlacement(
            Coordinate startingPoint,
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingPoint.Row, startingPoint.Column - i));
            }

            return coordinates;
        }
        
        private static List<Coordinate> GetCoordinatesForRightDirectionPlacement(
            Coordinate startingPoint,
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingPoint.Row, startingPoint.Column + i));
            }

            return coordinates;
        }

        private static bool IsPlacementPossible(
            IEnumerable<Coordinate> shipCoordinates,
            ICollection<Coordinate> freeCoordinates)
                => shipCoordinates.All(freeCoordinates.Contains);
    }
}