using System.Collections.ObjectModel;
using Puzzle.Helpers;
using SkiaSharp;
using Size = Puzzle.Helpers.Size;

namespace Puzzle;

public partial class MainPage
{
  public ObservableCollection<PuzzlePiece> Pieces { get; set; } = [];
  private int? Index { get; set; }
  private const int Subdivide = 10;

  public MainPage()
  {
    InitializeComponent();
    BindingContext = this;

    if (ImageContainer.ItemsLayout is GridItemsLayout gridLayout)
    {
      gridLayout.Span = Subdivide;
    }
  }

  private async void PickImage(object? sender, EventArgs e)
  {
    try
    {
      var options = new MediaPickerOptions() { Title = "Puzzle" };
      var stream = await ImagePicker.PickImageAsync(options);
      if (stream is null) return;

      var image = stream.ToArray();
      var imageSize = await ImagePicker.GetImageSizeAsync(stream);

      var puzzleSize = new Size(imageSize.Width / Subdivide, imageSize.Height / Subdivide);

      var result = GeneratePuzzlePieces(image, puzzleSize, Subdivide);

      Random.Shared.Shuffle(result);

      for (var i = 0; i < result.Length; i++)
      {
        result[i].CurrentIndex = i;
      }

      Pieces = new ObservableCollection<PuzzlePiece>(result);
      OnPropertyChanged(nameof(Pieces));
    }
    catch (Exception)
    {
      //
    }
  }

  private static PuzzlePiece[] GeneratePuzzlePieces(byte[] image, Size size, int subdivide)
  {
    var total = subdivide * subdivide;
    var pieces = new PuzzlePiece[total];

    for (var i = 0; i < total; i++)
    {
      var col = i % subdivide;
      var row = i / subdivide;

      var crop = Crop(image, row * size.Width, col * size.Height, size.Width, size.Height);
      pieces[i] = new PuzzlePiece(i, crop);
    }

    return pieces;
  }

  private static ImageSource Crop(byte[] imageData, int x, int y, int width, int height)
  {
    using var skStream = new SKMemoryStream(imageData);
    using var codec = SKCodec.Create(skStream);
    using var input = SKBitmap.Decode(codec);

    var rect = new SKRectI(x, y, x + width, y + height);
    using var subset = new SKBitmap(rect.Width, rect.Height);
    input.ExtractSubset(subset, rect);

    using var image = SKImage.FromBitmap(subset);
    var data = image.Encode(SKEncodedImageFormat.Png, 100);

    return ImageSource.FromStream(() => data.AsStream());
  }

  private void OnDrag(object? sender, DragStartingEventArgs e)
  {
    if (sender is Element { BindingContext: PuzzlePiece piece })
    {
      Index = piece.CurrentIndex;
    }
  }

  private async void OnDrop(object sender, DropEventArgs e)
  {
    try
    {
      if (sender is not Element { BindingContext: PuzzlePiece targetPiece }) return;
      if (targetPiece.CurrentIndex == Index) return;

      var draggedPiece = Pieces.FirstOrDefault(p => p.CurrentIndex == Index);
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