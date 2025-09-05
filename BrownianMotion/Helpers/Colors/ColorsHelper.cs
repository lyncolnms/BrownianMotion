namespace BrownianMotion.Helpers.Colors;

public static class ColorsHelper
{
    public static Color GetRandomColor()
    {
        Random rnd = new();

        int r = rnd.Next(50, 206);
        int g = rnd.Next(50, 206);
        int b = rnd.Next(50, 206);

        return Color.FromRgb(r, g, b);
    }
}