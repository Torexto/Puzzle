using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Helpers;

public static class ImageConversion
{
  public static IImage ToImage(this byte[] image)
  {
    var stream = new MemoryStream(image);
    return PlatformImage.FromStream(stream);
  }
}