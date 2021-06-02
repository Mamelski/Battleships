using System.Collections.Generic;
using Battleships.Core.Models.Ships;

namespace Battleships.Core.Models
{
    public record GameState(
        Board Board,
        ShotResult ShotResult,
        List<FightingShip> FightingShips,
        bool IsGameFinished);
}