using System.Collections.Generic;

namespace Battleships.Core.Models.Ships
{
    public record Ship(
        string Name,
        int Size)
    {
        public List<Coordinate> Coordinates { get; set; } = new();
    }
}