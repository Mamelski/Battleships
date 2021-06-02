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
            
            // while (allCoordinates.Any())
            {

                // var randomCoordinateIndex = random.Next(allCoordinates.Count);
                // var randomCoordinate = allCoordinates.ElementAt(randomCoordinateIndex);

                //  allCoordinates.Remove(randomCoordinate);
                
                
                var move = Console.ReadLine();
                var shotCoordinates = ParseMove(move);
                var result = gameManager.Shoot(shotCoordinates);
                
                // var result = gameManager.Shoot(randomCoordinate);
                ConsolePrinter.PrintBoard(result.Board);
                Thread.Sleep(100);
            }
        }
        
        private static Coordinate ParseMove(string move)
        {
            if (move.Length != 2)
            {
                throw new IncorrectMoveException();
            }
            
            var lower = move.ToLower();

            var row = lower[0] - 96;
            var column = lower[1] - 48;

            if (column is < 1 or > 10 || row is < 1 or > 10)
            {
                throw new IncorrectMoveException();
            }
            
            return new Coordinate(row, column);
        }
       
    }
}