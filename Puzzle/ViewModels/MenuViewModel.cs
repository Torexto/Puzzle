using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Puzzle.ViewModels;

public class MenuViewModel : INotifyPropertyChanged
{
  public ObservableCollection<Models.Puzzle> Puzzles { get; } = [];

  public event PropertyChangedEventHandler? PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}