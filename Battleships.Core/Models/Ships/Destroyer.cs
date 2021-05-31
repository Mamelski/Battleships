namespace Battleships.Core.Models.Ships
{
    public record Destroyer: Ship
    {
        public Destroyer() 
            : base(nameof(Destroyer), 4)
        {
        }
    }
}