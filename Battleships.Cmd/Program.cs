using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Battleships.Core;
using Battleships.Core.Models;
using Battleships.Core.Utils;

namespace Battleships.Cmd
{
    static class Program
    {
        static void Main(string[] args)
        {
            var gameManager = new GameManager(new RandomShipPlacer(), new BattleshipGameEngine());
            var board = gameManager.StartGame();
            
            ConsolePrinter.PrintIntroduction(board);

            while (true)
            {
                var mode = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(mode))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid mode");
                    Console.ResetColor();
                    continue;
                }

                if (mode.ToLower().Equals(GameMode.manual.ToString()))
                {
                    //TODO manual
                    Console.WriteLine("Remember that we have to save ammunition and you cannot shoot two time on the same coordinate.");
                    Console.WriteLine();
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
                    Console.WriteLine("MANUAL");

                    break;
                }

                if (mode.ToLower().Equals(GameMode.simulation.ToString()))
                {
                    // TODO simulation
                    Console.WriteLine("Great choice. Sit back, relax and watch me fight this battle for you");
                    RunSimulationMode(gameManager);
                    return;
                }
    
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid mode");
                Console.ResetColor();
            }
        }

        private static void RunSimulationMode(GameManager gameManager)
        {
            var random = new Random();
            var allCoordinates = GetListOfAllFreeCoordinates();

            while (allCoordinates.Any())
            {
                var randomCoordinateIndex = random.Next(allCoordinates.Count);
                var randomCoordinate = allCoordinates.ElementAt(randomCoordinateIndex);
                
                allCoordinates.Remove(randomCoordinate);

                var result = gameManager.Shoot(randomCoordinate);
                ConsolePrinter.PrintBoard(result.Board);
                
                if (result.IsGameFinished)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("We won Captain! Our enemies are defeated");
                    Console.ResetColor();

                    Console.WriteLine("Press ENTER to close me ...");
                    Console.ReadKey();
                    return;
                }
                
                Thread.Sleep(250);
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
    }
}