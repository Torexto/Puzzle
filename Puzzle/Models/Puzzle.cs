using System.Text.Json.Serialization;
using Puzzle.Helpers;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Models;

public class Puzzle
{
  public Puzzle(IImage image, int division, PuzzlePiece[] pieces)
  {
    Image = image;
    ImageData = image.AsBytes();
    Division = division;
    Pieces = pieces;
  }

  public Puzzle(IImage image, int division)
  {
    Image = image;
    ImageData = image.AsBytes();
    Division = division;
    Pieces = Divide();

    Shuffle();
  }

  private PuzzlePiece[] Divide()
  {
    var width = (int)(Image.Width / Division);
    var height = (int)(Image.Height / Division);

    var total = Division * Division;
    var result = new PuzzlePiece[total];

    for (var i = 0; i < total; i++)
    {
      var col = i % Division;
      var row = i / Division;

      var area = new Rect(row * width, col * height, width, height);
      result[i] = new PuzzlePiece(Image, i, area, GetScale());
    }

    return result;
  }

  private void Shuffle()
  {
    Random.Shared.Shuffle(Pieces);

    for (var i = 0; i < Pieces.Length; i++)
    {
      Pieces[i].CurrentIndex = i;
    }
  }

  private double GetScale()
  {
    var widthScale = ScreenInfo.Width / Image.Width;
    var heightScale = ScreenInfo.Height / Image.Height;

    return ScreenInfo.Orientation == DisplayOrientation.Portrait ? widthScale : heightScale;
  }

  [JsonIgnore] //
  public IImage Image { get; }

  [JsonPropertyName("imageData")] //
  public byte[] ImageData { get; set; }

  [JsonPropertyName("division")] //
  public int Division { get; }

  public PuzzlePiece[] Pieces { get; set; }
}