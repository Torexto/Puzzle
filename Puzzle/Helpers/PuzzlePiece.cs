using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Puzzle.Helpers;

public class PuzzlePiece(int index, ImageSource image) : INotifyPropertyChanged

{
  private int _currentIndex = index;
  public ImageSource Image { get; } = image;
  public int OriginalIndex { get; } = index;

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
  
  public required Rect Area { get; set; }

  public event PropertyChangedEventHandler? PropertyChanged;

  protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}