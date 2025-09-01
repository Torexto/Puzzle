using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Models;

public class PuzzlePiece(IImage originalImage, int index, Rect area, double scale) : IDrawable, INotifyPropertyChanged
{
  private int _currentIndex = index;
  public int OriginalIndex { get; } = index;
  public Rect Area { get; } = area;

  public int CurrentIndex
  {
    get => _currentIndex;
    set
    {
      if (_currentIndex == value) return;
      _currentIndex = value;
      OnPropertyChanged();
    }
  }

  public double Width => Area.Width * scale;
  public double Height => Area.Height * scale;
  public double Scale => scale;

  public void Draw(ICanvas canvas, RectF dirtyRect)
  {
    canvas.SaveState();
    canvas.DrawImage(
      originalImage,
      (float)(-Area.X * scale),
      (float)(-Area.Y * scale),
      (float)(originalImage.Width * scale),
      (float)(originalImage.Height * scale)
    );
    canvas.RestoreState();
  }


  public event PropertyChangedEventHandler? PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}