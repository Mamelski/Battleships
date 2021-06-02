using System;
using Battleships.Core.Models;
using Battleships.Core.Utils;

namespace Battleships.Cmd
{
    public static class ConsolePrinter
    {
        public static void PrintIntroduction(Board board)
        {
            Console.WriteLine();
            PrintInColor(ConsoleColor.Yellow, "Hello Commander! ");
            Console.Write("Enemies are at our waters and we need to ");
            PrintInColor(ConsoleColor.DarkRed, "destroy them!");
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
            PrintInColor(ConsoleColor.Green, $"{GameMode.simulation}");
            Console.WriteLine(" to let me win this battle for us");
            Console.Write("- type ");
            PrintInColor(ConsoleColor.Green, $"{GameMode.manual}");
            Console.WriteLine(" to fight without my help (I will still show you state of the battlefield)");
            Console.WriteLine();
        }

        public static void PrintBoard(Board board)
        {
            Console.WriteLine("    A B C D E F G H I J ");
            Console.WriteLine("    --------------------");

            for (var row = 0; row < Consts.Rows; row++)
            {
                Console.Write(row < 9 ? $"{row + 1}  |" : $"{row + 1} |");

                for (var column = 0; column < Consts.Columns; column++)
                {
                    PrintCoordinateState(board[row, column]);
                }
                Console.Write("|\n");
            }
            Console.WriteLine("    --------------------");
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
        
        private static void PrintCoordinateState(CoordinateState coordinateState)
        {
            switch (coordinateState)
            {
                case CoordinateState.Miss:
                    PrintInColor(ConsoleColor.White, ". ");
                    break;
                case CoordinateState.Hit:
                    PrintInColor(ConsoleColor.Yellow, "x ");
                    break;
                case CoordinateState.Sunk:
                    PrintInColor(ConsoleColor.DarkRed, "x ");
                    break;
                case CoordinateState.Unknown:
                    Console.Write("  ");
                    break;
            }
        }

        private static void PrintInColor(
            ConsoleColor color,
            string message)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }
    }
}