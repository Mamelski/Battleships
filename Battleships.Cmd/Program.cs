using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Hello Commander! ");
            Console.ResetColor();

            Console.Write("Enemies are at our waters and we need to ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("destroy them!");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("I am the next generation AI system and I will help you win this battle.");
            Console.WriteLine("I have intel what ships our enemy has:");
            Console.WriteLine();
            Console.WriteLine("- 1x Battleship (5 squares)");
            Console.WriteLine("- 2x Destroyers (4 squares)");
            Console.WriteLine();
            
            Console.WriteLine("Unfortunately I don't know their exact location, so we need to shoot blindly and hope for a hit.");

            
            Console.WriteLine("Battlefield is a 10x10 grid.");
            Console.WriteLine("Every coordinate is identified by a letter (\"a\" to \"j\") and a number (\"1\" to \"10\"):");
            Console.WriteLine();
            PrintBoard(board);

            PrintLegend();

            Console.WriteLine("Now you need to decide how we play this battle:");
            Console.WriteLine();

            Console.Write("- type ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(GameMode.simulation);
            Console.ResetColor();
            Console.WriteLine(" to let me win this battle for us");
            
            Console.Write("- type ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(GameMode.manual);
            Console.ResetColor();
            Console.WriteLine(" to fight without my help (I will still show you state of the battlefield)");
            Console.WriteLine();

            var modeChosen = false;
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
                        PrintBoard(result.Board);
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
                PrintBoard(result.Board);
                
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

        private static void PrintLegend()
        {
            Console.WriteLine("Coordinates can have few possible states:");
            Console.WriteLine();
            
            PrintCoordinateState(CoordinateState.Unknown);
            Console.WriteLine(" - means that have no idea what is there");
            
            PrintCoordinateState(CoordinateState.Miss);
            Console.WriteLine(" - means that we shot there and missed");
            
            PrintCoordinateState(CoordinateState.Hit);
            Console.WriteLine(" - means that we hit part of the enemy ship");
            
            PrintCoordinateState(CoordinateState.Sunk);
            Console.WriteLine(" - means that we hit enemy ship on this coordinate and sunk it");
            
            Console.WriteLine();
        }

        public static void PrintBoard(Board board)
        {
            var stringBuilder = new StringBuilder();

            Console.WriteLine("    A B C D E F G H I J ");
            Console.WriteLine("    --------------------");

            for (var row = 0; row < Consts.Rows; row++)
            {
                if (row < 9)
                {
                    Console.Write($"{row + 1}  |");
                }
                else
                {
                    Console.Write($"{row + 1} |");
                }

                for (var column = 0; column < Consts.Columns; column++)
                {
                    PrintCoordinateState(board[row, column]);
                }

                Console.Write("|\n");
            }
            
            stringBuilder.AppendLine("    --------------------");
            
            Console.WriteLine(stringBuilder);
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

        private static void PrintCoordinateState(CoordinateState coordinateState)
        {
            switch (coordinateState)
            {
               case CoordinateState.Miss:
                   Console.ForegroundColor = ConsoleColor.White;
                   Console.Write(". ");
                   Console.ResetColor();
                   
                   break;
               case CoordinateState.Hit:
                   Console.ForegroundColor = ConsoleColor.Yellow;
                   Console.Write("x ");
                   Console.ResetColor();
                   
                   break;
               case CoordinateState.Sunk:
                   Console.ForegroundColor = ConsoleColor.Red;
                   Console.Write("x ");
                   Console.ResetColor();
                   
                   break;
               case CoordinateState.Unknown:
                   Console.ResetColor();
                   Console.Write("  ");

                   break;
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
    }
}