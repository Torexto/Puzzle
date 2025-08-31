using Puzzle.Models;
using Puzzle.ViewModels;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Views;

public partial class Puzzle
{
  private readonly IImage _image;
  private readonly int _division;

  private readonly PuzzleViewModel _viewModel;

  private int _index;

  public Puzzle(IImage image, int division)
  {
    InitializeComponent();

    _image = image;
    _division = division;

    var display = DeviceDisplay.MainDisplayInfo;
    var screenWidth = display.Width / display.Density;
    var screenHeight = display.Height / display.Density;

    var widthScale = screenWidth / _image.Width;
    var heightScale = (screenHeight - 50) / _image.Height;

    var scale = display.Orientation == DisplayOrientation.Portrait ? widthScale : heightScale;

    _viewModel = new PuzzleViewModel(_image, _division, scale);
    BindingContext = _viewModel;
  }

  private void OnDrag(object? sender, DragStartingEventArgs e)
  {
    if (sender is Element { BindingContext: PuzzleDrawable piece })
    {
      _index = piece.Piece.CurrentIndex;
    }
  }

  private async void OnDrop(object sender, DropEventArgs e)
  {
    try
    {
      if (sender is not Element { BindingContext: PuzzleDrawable targetPiece }) return;
      if (targetPiece.Piece.CurrentIndex == _index) return;

      var draggedPiece = _viewModel.Pieces.FirstOrDefault(p => p.Piece.CurrentIndex == _index);
      if (draggedPiece is null)
        return;

      (draggedPiece.Piece.CurrentIndex, targetPiece.Piece.CurrentIndex) =
        (targetPiece.Piece.CurrentIndex, draggedPiece.Piece.CurrentIndex);

      _viewModel.Pieces[draggedPiece.Piece.CurrentIndex] = draggedPiece;
      _viewModel.Pieces[targetPiece.Piece.CurrentIndex] = targetPiece;

      if (_viewModel.Pieces.All(p => p.Piece.CurrentIndex == p.Piece.OriginalIndex))
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