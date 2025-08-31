using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Models;

public class PuzzleDrawable(IImage originalImage, PuzzlePiece piece, double scale) : IDrawable
{
  public readonly PuzzlePiece Piece = piece;

  public double Width => Piece.Area.Width * scale;
  public double Height => Piece.Area.Height * scale;
  public double Scale => scale;

  public void Draw(ICanvas canvas, RectF dirtyRect)
  {
    canvas.SaveState();
    canvas.DrawImage(
      originalImage,
      (float)(-Piece.Area.X * scale),
      (float)(-Piece.Area.Y * scale),
      (float)(originalImage.Width * scale),
      (float)(originalImage.Height * scale)
    );
    canvas.RestoreState();
  }
}