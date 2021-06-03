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
                var mode = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(mode))
                {
                    ConsolePrinter.PrintInColor(ConsoleColor.DarkRed, "Invalid mode");
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
                
                ConsolePrinter.PrintInColor(ConsoleColor.DarkRed, "Invalid mode");
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

                var moveString = ParseCoodinateToMove(shot);
                ConsolePrinter.PrintInColor(ConsoleColor.Cyan, moveString);
                Console.WriteLine();
                
                var result = gameManager.Shoot(shot);
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
                    shotCoordinate = ParseMove(move);
                  
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
        
        private static Coordinate ParseMove(string move)
        {
            int row, column;
            string lower;
            switch (move.Length)
            {
                case 2:
                    lower = move.ToLower();
                    column = lower[0] - 96;
                    row = lower[1] - 48;
                    break;
                case 3:
                    lower = move.ToLower();
                    column = lower[0] - 96;
                    row = (lower[1] - 48) * 10 + lower[2] - 48;
                    break;
                default:
                    throw new InvalidMoveException(move);
            }

            if (column is < 1 or > 10 || row is < 1 or > 10)
            {
                throw new InvalidMoveException(move);
            }
            
            return new Coordinate(row - 1, column - 1);
        }

        private static string ParseCoodinateToMove(Coordinate coordinate)
        {
            var column = Convert.ToChar(coordinate.Column + 97);
            var row = (coordinate.Row +1).ToString();

            return column + row;
        }
    }
}