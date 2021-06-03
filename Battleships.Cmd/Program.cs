using System;
using System.Linq;
using System.Threading;
using Battleships.Core;
using Battleships.Core.Models;
using Battleships.Core.Utils;

namespace Battleships.Cmd
{
    static class Program
    {
        static void Main()
        {
            var gameManager = new GameManager(new RandomShipPlacer(), new BattleshipGameEngine());
            
            var board = gameManager.StartGame();
            ConsolePrinter.PrintIntroduction(board);

            var mode = ReadGameMode();

            switch (mode)
            {
                case GameMode.Auto:
                    RunAutoMode(gameManager);
                    break;
                case GameMode.Manual:
                    RunManualMode(gameManager);
                    break;
            }
        }

        private static GameMode ReadGameMode()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                var mode = Console.ReadLine();
                Console.ResetColor();
                
                if (string.IsNullOrWhiteSpace(mode))
                {
                    ConsolePrinter.PrintInColor(ConsoleColor.Red, "Invalid mode");
                    Console.WriteLine();
                    continue;
                }
                
                if (mode.Equals(GameMode.Manual.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return GameMode.Manual;
                }

                if (mode.Equals(GameMode.Auto.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return GameMode.Auto;
                }
                
                ConsolePrinter.PrintInColor(ConsoleColor.Red, "Invalid mode");
                Console.WriteLine();
            }
        }

        private static void RunAutoMode(GameManager gameManager)
        {
            var random = new Random();
            var coordinatesToShoot = CoordinateHelper.GetAllCoordinates();

            while (coordinatesToShoot.Any())
            {
                var randomCoordinateIndex = random.Next(coordinatesToShoot.Count);
                var shot = coordinatesToShoot.ElementAt(randomCoordinateIndex);
                
                coordinatesToShoot.Remove(shot);

                var moveString = MoveParser.ParseCoordinateToMove(shot);
                ConsolePrinter.PrintInColor(ConsoleColor.Cyan, moveString);
                Console.WriteLine();
                
                var result = gameManager.Shoot(shot);

                if (result.ShotResult == ShotResult.Sink)
                {
                    foreach (var sunkShip in result.FightingShips.Where(ship => ship.IsSunk))
                    {
                        var surroundingCoordinates = CoordinateHelper.GetSurroundingCoordinates(sunkShip.Coordinates);
                        foreach (var coordinate in surroundingCoordinates)
                        {
                            coordinatesToShoot.Remove(coordinate);
                        }
                    }
                }
                
                ConsolePrinter.PrintStateAfterMove(result);
                
                if (result.IsGameFinished)
                {
                    ConsolePrinter.PrintEndGameMessage();
                    return;
                }
                
                Thread.Sleep(250);
            }
        }
        
        private static void RunManualMode(GameManager gameManager)
        {
            ConsolePrinter.PrintManualIntroduction();
            
            var isGameFinished = false;
            while (!isGameFinished)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                var move = Console.ReadLine();
                Console.ResetColor();

                Coordinate shotCoordinate;
                try
                {
                    shotCoordinate = MoveParser.ParseMove(move);
                  
                }
                catch (InvalidMoveException exception)
                {
                    ConsolePrinter.PrintInColor(ConsoleColor.Red, $"Invalid coordinates: {exception.Move}");
                    Console.WriteLine();
                    continue;
                }

                var result = gameManager.Shoot(shotCoordinate);

                ConsolePrinter.PrintStateAfterMove(result);

                isGameFinished = result.IsGameFinished;
            }
            ConsolePrinter.PrintEndGameMessage();
        }
    }
}