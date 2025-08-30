using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle.Helpers;

public static class ImagePicker
{
  public static async Task<MemoryStream?> PickImageAsync(MediaPickerOptions? options = null)
  {
    try
    {
      var file = await MediaPicker.PickPhotoAsync(options ?? new MediaPickerOptions());
      if (file is null) return null;

      await using var stream = await file.OpenReadAsync();
      var memoryStream = new MemoryStream();
      await stream.CopyToAsync(memoryStream);
      memoryStream.Position = 0;

      return memoryStream;
    }
    catch (Exception)
    {
      return null;
    }
  }

  public static async Task<Size> GetImageSizeAsync(Stream stream)
  {
    var memory = new MemoryStream();
    await stream.CopyToAsync(memory);
    memory.Position = 0;

    IImage image = PlatformImage.FromStream(memory);

    return new Size(Convert.ToInt32(image.Width), Convert.ToInt32(image.Height));
  }
}