using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle;

public partial class MainPage
{
  public ObservableCollection<Models.Puzzle> Puzzles { get; } = [];

  public MainPage()
  {
    InitializeComponent();

    BindingContext = this;
  }

  private async void PickImage(object? sender, EventArgs e)
  {
    try
    {
      var options = new MediaPickerOptions() { Title = "Puzzle" };
      var image = await PickImageAsync(options);
      if (image is null) return;

      var result = await DisplayPromptAsync("", "Divide for: (1-16)", "OK", "CANCEL", "", 2, Keyboard.Numeric, "5");
      if (result is null) return;

      if (!int.TryParse(result, out var division)) return;

      var puzzle = new Models.Puzzle(image, Math.Clamp(division, 1, 16));

      Puzzles.Add(puzzle);

      await Navigation.PushAsync(new Views.PuzzleView(puzzle));
    }
    catch (Exception err)
    {
      await DisplayAlert(err.Message, "Error", "Ok");
    }
  }

  private static async Task<IImage?> PickImageAsync(MediaPickerOptions? options = null)
  {
    try
    {
      var file = await MediaPicker.PickPhotoAsync(options ?? new MediaPickerOptions());
      if (file is null) return null;

      await using var stream = await file.OpenReadAsync();
      var memoryStream = new MemoryStream();
      await stream.CopyToAsync(memoryStream);
      memoryStream.Position = 0;

      return PlatformImage.FromStream(memoryStream);
    }
    catch (Exception)
    {
      return null;
    }
  }

  private async void OnTapped(object? sender, TappedEventArgs e)
  {
    if (sender is not Element {BindingContext: Models.Puzzle puzzle}) return;

    await Navigation.PushAsync(new Views.PuzzleView(puzzle));
  }
}