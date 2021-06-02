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
                var randomCoordinate = coordinatesToShoot.ElementAt(randomCoordinateIndex);
                
                coordinatesToShoot.Remove(randomCoordinate);

                var result = gameManager.Shoot(randomCoordinate);
                ConsolePrinter.PrintBoard(result.Board);
                
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
                var move = Console.ReadLine();
                var shotCoordinates = ParseMove(move);
                var result = gameManager.Shoot(shotCoordinates);

                ConsolePrinter.PrintStateAfterMove(result);

                isGameFinished = result.IsGameFinished;
            }
        }
        
        private static Coordinate ParseMove(string move)
        {
            int row, column;
            string lower;
            switch (move.Length)
            {
                case 2:
                    lower = move.ToLower();
                    row = lower[0] - 96;
                    column = lower[1] - 48;
                    break;
                case 3:
                    lower = move.ToLower();
                    row = lower[0] - 96;
                    column = (lower[1] - 48) * 10 + lower[2] - 48;
                    break;
                default:
                    throw new IncorrectMoveException();
            }

            if (column is < 1 or > 10 || row is < 1 or > 10)
            {
                throw new IncorrectMoveException();
            }
            
            return new Coordinate(row - 1, column - 1);
        }
       
    }
}