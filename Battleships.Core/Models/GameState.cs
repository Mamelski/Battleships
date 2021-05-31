namespace Battleships.Core.Models
{
    public record GameState(
        Board Board,
        MoveResult MoveResult,
        bool IsGameFinished);
}