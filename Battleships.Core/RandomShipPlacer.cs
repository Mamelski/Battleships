using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Models;
using Battleships.Core.Models.Ships;
using Battleships.Core.Utils;

namespace Battleships.Core
{
    public interface IShipPlacer
    {
        List<PlacedShip> PlaceShips(IEnumerable<UnplacedShip> unplacedShips);
    }
    
    public class RandomShipPlacer : IShipPlacer
    {
        private readonly Random _random = new();
        
        public List<PlacedShip> PlaceShips(IEnumerable<UnplacedShip> unplacedShips)
        {
            var placedShips = new List<PlacedShip>();
            
            var freeCoordinates = CoordinateHelper.GetAllCoordinates();

            foreach (var unplacedShip in unplacedShips)
            {
                var placementCoordinates = FindPlacementCoordinatesForShip(unplacedShip.Size, freeCoordinates);
                
                var surroundingCoordinates = CoordinateHelper.GetSurroundingCoordinates(placementCoordinates);

                foreach (var coordinate in placementCoordinates.Concat(surroundingCoordinates))
                {
                    freeCoordinates.Remove(coordinate);
                }

                var placedShip = new PlacedShip(unplacedShip.ShipClass, placementCoordinates);
                
                placedShips.Add(placedShip);
            }

            return placedShips;
        }

        private List<Coordinate> FindPlacementCoordinatesForShip(
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
            Coordinate startingCoordinates,
            int shipSize,
            PlacementDirection direction)
        {
            return direction switch
            {
                PlacementDirection.Up => GetCoordinatesForUpDirectionPlacement(startingCoordinates, shipSize),
                PlacementDirection.Down => GetCoordinatesForDownDirectionPlacement(startingCoordinates, shipSize),
                PlacementDirection.Left => GetCoordinatesForLeftDirectionPlacement(startingCoordinates, shipSize),
                PlacementDirection.Right => GetCoordinatesForRightDirectionPlacement(startingCoordinates, shipSize),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Not Supported direction")
            };
        }

        private static List<Coordinate> GetCoordinatesForUpDirectionPlacement(
            Coordinate startingCoordinates, 
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingCoordinates.Row + i, startingCoordinates.Column));
            }

            return coordinates;
        } 
        
        private static List<Coordinate> GetCoordinatesForDownDirectionPlacement(
            Coordinate startingCoordinates,
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingCoordinates.Row - i, startingCoordinates.Column));
            }

            return coordinates;
        }

        private static List<Coordinate> GetCoordinatesForLeftDirectionPlacement(
            Coordinate startingCoordinates,
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingCoordinates.Row, startingCoordinates.Column - i));
            }

            return coordinates;
        }
        
        private static List<Coordinate> GetCoordinatesForRightDirectionPlacement(
            Coordinate startingCoordinates,
            int shipSize)
        {
            var coordinates = new List<Coordinate>();
            for (var i = 0; i < shipSize; i++)
            {
                coordinates.Add(new Coordinate(startingCoordinates.Row, startingCoordinates.Column + i));
            }

            return coordinates;
        }

        private static bool IsPlacementPossible(
            IEnumerable<Coordinate> shipCoordinates,
            ICollection<Coordinate> freeCoordinates)
                => shipCoordinates.All(freeCoordinates.Contains);
    }
}