namespace Battleships.Core.Models
{
    public record Destroyer: Ship
    {
        public Destroyer() 
            : base(nameof(Destroyer), 4)
        {
        }
    }
}