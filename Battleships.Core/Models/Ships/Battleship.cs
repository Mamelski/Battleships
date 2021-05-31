namespace Battleships.Core.Models.Ships
{
    public record Battleship : Ship
    {
        public Battleship() 
            : base(nameof(Battleship), 5)
        {
        }
    }
}