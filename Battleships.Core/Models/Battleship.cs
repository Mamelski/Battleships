namespace Battleships.Core.Models
{
    public record Battleship : Ship
    {
        public Battleship() 
            : base(nameof(Battleship), 5)
        {
        }
    }
}