namespace Puzzle.Helpers;

public class PuzzlePiece
{
  public required int OriginalIndex { get; init; }
  public required int CurrentIndex { get; set; }
  public required ImageSource Image { get; set; }
}
