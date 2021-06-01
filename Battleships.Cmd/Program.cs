using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // TODO command: coordinates, simulation, restart, legend, help
            var allCoordinates = GetListOfAllFreeCoordinates();
            var gameManager = new GameManager(new RandomShipPlacer(), new BattleshipGameEngine());
            
            var board = gameManager.StartGame();
            
            Console.WriteLine();
            Console.WriteLine("Hello Commander! Enemies are at our waters and we need to destroy them!");
            Console.WriteLine("I am the next generation AI system and I will help you win this battle.");
            Console.WriteLine("Below you can see our battlefield:");
            Console.WriteLine();
            PrintBoard(board);

            PrintLegend();
            
              
            Console.WriteLine("Remember that we have to save ammunition and you cannot shoot two time on the same coordinate.");
            Console.WriteLine();
            
            Console.WriteLine("Now we have two options to play this battle:");
            Console.WriteLine("");
            
            
            
            while (allCoordinates.Any())
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
        }

        private static void PrintLegend()
        {
            Console.WriteLine("Battlefield is a 10x10 grid.");
            Console.WriteLine("Every coordinate is identified by a letter (\"a\" to \"j\") and a number (\"1\" to \"10\").");
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