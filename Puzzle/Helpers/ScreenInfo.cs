namespace Puzzle.Helpers;

public static class ScreenInfo
{
  private static readonly DisplayInfo Display = DeviceDisplay.MainDisplayInfo;

  public static double Width => Display.Width / Display.Density;
  public static double Height => (Display.Height / Display.Density) - 50;
  public static (double width, double height) Size => (Width, Height);
  public static DisplayOrientation Orientation => Display.Orientation;
}