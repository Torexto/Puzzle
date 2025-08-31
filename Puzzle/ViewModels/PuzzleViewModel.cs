using Puzzle.Models;

namespace Puzzle.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;

public class PuzzleViewModel : INotifyPropertyChanged
{
  public ObservableCollection<PuzzleDrawable> Pieces { get; } = [];
  public int Division { get; set; }

  public PuzzleViewModel(IImage? originalImage, int division, double scale)
  {
    if (originalImage == null) return;

    Division = division;

    var width = (int)(originalImage.Width / division);
    var height = (int)(originalImage.Height / division);

    var total = division * division;
    var result = new PuzzleDrawable[total];

    for (var i = 0; i < total; i++)
    {
      var col = i % division;
      var row = i / division;

      var area = new Rect(row * width, col * height, width, height);
      var piece = new PuzzlePiece(i, area);
      result[i] = new PuzzleDrawable(originalImage, piece, scale);
    }

    Random.Shared.Shuffle(result);

    for (var i = 0; i < result.Length; i++)
    {
      result[i].Piece.CurrentIndex = i;
    }

    Pieces = new ObservableCollection<PuzzleDrawable>(result);
    OnPropertyChanged(nameof(Pieces));
  }

  public event PropertyChangedEventHandler? PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}