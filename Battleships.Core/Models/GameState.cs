namespace Battleships.Core.Models
{
    public record GameState(
        Board Board,
        ShotResult ShotResult,
        bool IsGameFinished);
}