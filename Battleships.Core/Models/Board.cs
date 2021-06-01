using Battleships.Core.Utils;

namespace Battleships.Core.Models
{
    public class Board
    {
        private readonly CoordinateState[,] _innerBoard; 
        
        public CoordinateState this[Coordinate coordinate]
        {
            get => _innerBoard[coordinate.Row, coordinate.Column];
            set => _innerBoard[coordinate.Row, coordinate.Column] = value;
        }

        public Board()
        {
            _innerBoard = new CoordinateState[Consts.Rows, Consts.Columns];

            for (var row = 0; row < Consts.Rows; row++)
            {
                for (var column = 0; column < Consts.Columns; column++)
                {
                    _innerBoard[row, column] = CoordinateState.Unknown;
                }
            }
        }
    }
}