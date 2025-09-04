using BrownianMotion.Helpers.Graphs;

namespace BrownianMotion.Features.BrownianGraphic.Drawables;

public class BrownianMotionDrawable(double sigma, double mean, double initialPrice, int numDays) : IDrawable
{
    private readonly double[] _prices = GraphicsHelpers.GenerateBrownianMotion(sigma, mean, initialPrice, numDays);

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_prices == null || _prices.Length < 2) return;

        float margin = 20f;
        float chartWidth = dirtyRect.Width - (2 * margin);
        float chartHeight = dirtyRect.Height - (2 * margin);

        double minPrice = _prices.Min();
        double maxPrice = _prices.Max();
        double priceRange = maxPrice - minPrice;

        if (priceRange == 0) priceRange = 1;

        float stepX = chartWidth / (_prices.Length - 1);

        canvas.FillColor = Colors.Black.WithAlpha(0.1f);
        canvas.FillRectangle(margin, margin, chartWidth, chartHeight);

        canvas.StrokeColor = Colors.BlueViolet;
        canvas.StrokeSize = 2;

        for (int i = 1; i < _prices.Length; i++)
        {
            float x0 = margin + (i - 1) * stepX;
            float x1 = margin + i * stepX;

            float y0 = margin + chartHeight - (float)((_prices[i - 1] - minPrice) / priceRange * chartHeight);
            float y1 = margin + chartHeight - (float)((_prices[i] - minPrice) / priceRange * chartHeight);

            canvas.DrawLine(x0, y0, x1, y1);
        }

        canvas.FontColor = Colors.White;
        canvas.FontSize = 12;
        canvas.DrawString($"Min: {minPrice:F2}", 10, 10, HorizontalAlignment.Left);
        canvas.DrawString($"Max: {maxPrice:F2}", 10, 25, HorizontalAlignment.Left);
    }
}