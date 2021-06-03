using System;

namespace Battleships.Cmd
{
    public class InvalidMoveException : Exception
    {
        public string Move { get; }
        
        public InvalidMoveException(string move)
        {
            Move = move;
        }
    }
}