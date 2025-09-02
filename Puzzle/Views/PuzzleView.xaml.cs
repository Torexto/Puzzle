using System.Collections.ObjectModel;
using Puzzle.Models;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Views;

public partial class PuzzleView
{
  private readonly Models.Puzzle _puzzle;

  public IImage Image { get; set; }
  public ObservableCollection<PuzzlePiece> Pieces { get; }
  public int Division { get; set; }

  private int _index;

  public PuzzleView(Models.Puzzle puzzle)
  {
    InitializeComponent();

    _puzzle = puzzle;

    Image = puzzle.Image;
    Division = puzzle.Division;
    Pieces = new ObservableCollection<PuzzlePiece>(puzzle.Pieces);

    Pieces.CollectionChanged += (s, e) =>
    {
      _puzzle.Pieces = Pieces.ToArray();
    };

    BindingContext = this;
  }

  private void OnDrag(object? sender, DragStartingEventArgs e)
  {
    if (sender is Element { BindingContext: PuzzlePiece piece })
    {
      _index = piece.CurrentIndex;
    }
  }

  private async void OnDrop(object sender, DropEventArgs e)
  {
    try
    {
      if (sender is not Element { BindingContext: PuzzlePiece targetPiece }) return;
      if (targetPiece.CurrentIndex == _index) return;

      var draggedPiece = Pieces.FirstOrDefault(p => p.CurrentIndex == _index);
      if (draggedPiece is null)
        return;

      (draggedPiece.CurrentIndex, targetPiece.CurrentIndex) =
        (targetPiece.CurrentIndex, draggedPiece.CurrentIndex);

      Pieces[draggedPiece.CurrentIndex] = draggedPiece;
      Pieces[targetPiece.CurrentIndex] = targetPiece;

      if (Pieces.All(p => p.CurrentIndex == p.OriginalIndex))
      {
        await DisplayAlert("🎉", "Puzzle solved!", "OK");
      }
    }
    catch (Exception)
    {
      //
    }
  }
}