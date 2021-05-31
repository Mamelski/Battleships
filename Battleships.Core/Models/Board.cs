using Battleships.Core.Utils;

namespace Battleships.Core.Models
{
    public class Board
    {
        private readonly FieldState[,] _innerBoard = new FieldState[Consts.Rows, Consts.Columns];
        
        public FieldState this[int row, int column] => _innerBoard[row, column];
    }
}