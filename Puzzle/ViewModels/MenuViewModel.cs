using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Puzzle.Models;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.ViewModels;

public class Puzzle(IImage image, int division, PuzzleDrawable[] pieces = null) : IDrawable
{
  public IImage Image { get; } = image;
  public int Division { get; } = division;
  public PuzzleDrawable[] Pieces { get; } = pieces;


  public void Draw(ICanvas canvas, RectF dirtyRect)
  {
    canvas.SaveState();
    canvas.DrawImage(Image, dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
    canvas.RestoreState();
  }
}

public class MenuViewModel : INotifyPropertyChanged
{
  public ObservableCollection<Puzzle> Puzzles { get; } = [];

  public event PropertyChangedEventHandler? PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}