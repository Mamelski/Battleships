using System;
using Battleships.Core.Models;

namespace Battleships.Cmd
{
    public static class MoveParser
    {
        public static Coordinate ParseMove(string move)
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

        public static string ParseCoordinateToMove(Coordinate coordinate)
        {
            var column = Convert.ToChar(coordinate.Column + 97);
            var row = (coordinate.Row +1).ToString();

            return column + row;
        }
    }
}