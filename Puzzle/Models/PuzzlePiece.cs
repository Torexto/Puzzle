using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Puzzle.Models;

public class PuzzlePiece(int index, Rect area) : INotifyPropertyChanged
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


  public event PropertyChangedEventHandler? PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}