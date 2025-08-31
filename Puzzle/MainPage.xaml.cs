using Microsoft.Maui.Graphics.Platform;
using Puzzle.ViewModels;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Puzzle;

public partial class MainPage
{
  private readonly MenuViewModel _viewModel;

  public MainPage()
  {
    InitializeComponent();

    _viewModel = new MenuViewModel();
    BindingContext = _viewModel;
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

      _viewModel.Puzzles.Add(new ViewModels.Puzzle(image, division));

      await Navigation.PushAsync(new Views.Puzzle(image, Math.Clamp(division, 1, 16)));
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
    await DisplayAlert("t", "t", "t");
  }
}