using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Helpers;

public class PuzzleDrawable : IDrawable
{
  private readonly IImage _originalImage;
  public readonly PuzzlePiece Piece;

  public float Width => (float)Piece.Area.Width;
  public float Height => (float)Piece.Area.Height;

  public PuzzleDrawable(IImage originalImage, PuzzlePiece piece)
  {
    _originalImage = originalImage;
    Piece = piece;
  }

  public void Draw(ICanvas canvas, RectF dirtyRect)
  {
    canvas.SaveState();
    canvas.ClipRectangle(Piece.Area with { X = 0, Y = 0 });
    canvas.DrawImage(_originalImage, -(float)Piece.Area.X, -(float)Piece.Area.Y, _originalImage.Width,
      _originalImage.Height);
    canvas.RestoreState();
  }
}