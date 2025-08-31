using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics.Platform;
using Puzzle.Helpers;
using Puzzle.Models;
using SkiaSharp;
using Size = Puzzle.Helpers.Size;

namespace Puzzle;

public partial class MainPage
{
  public MainPage()
  {
    InitializeComponent();
  }

  private async void PickImage(object? sender, EventArgs e)
  {
    try
    {
      var options = new MediaPickerOptions() { Title = "Puzzle" };
      var stream = await ImagePicker.PickImageAsync(options);
      if (stream is null) return;
      var image = PlatformImage.FromStream(stream);

      // var imageSize = await ImagePicker.GetImageSizeAsync(stream);
      //
      // var puzzleSize = new Size(imageSize.Width / Subdivide, imageSize.Height / Subdivide);
      //
      // var result = GeneratePuzzlePieces(stream, puzzleSize, Subdivide);
      //
      // Random.Shared.Shuffle(result);
      //
      // for (var i = 0; i < result.Length; i++)
      // {
      //   result[i].Piece.CurrentIndex = i;
      // }
      //
      // Pieces = new(result);
      // OnPropertyChanged(nameof(Pieces));


      await Navigation.PushAsync(new Views.Puzzle(image, 5));
    }
    catch (Exception err)
    {
      await DisplayAlert(err.Message, "Error", "Ok");
    }
  }

  private PuzzleDrawable[] GeneratePuzzlePieces(MemoryStream stream, Size size, int subdivide)
  {
    var total = subdivide * subdivide;
    var pieces = new PuzzleDrawable[total];
    stream.Position = 0;
    var image = stream.ToArray();
    var platformImage = PlatformImage.FromStream(new MemoryStream(image));

    for (var i = 0; i < total; i++)
    {
      var col = i % subdivide;
      var row = i / subdivide;

      var width = size.Width;
      var height = size.Height;

      var area = new Rect(col * width, row * height, width, height);
      var piece = new PuzzlePiece(i, area);
      // pieces[i] = new PuzzleDrawable(platformImage, piece);
    }

    return pieces;
  }
}