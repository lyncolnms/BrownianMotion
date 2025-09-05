using BrownianMotion.Helpers.Colors;
using BrownianMotion.Helpers.Graphs;

namespace BrownianMotion.Features.BrownianGraphic.Drawables;

public class BrownianMotionDrawable : IDrawable
{
    private readonly List<double[]> _simulations = [];
    private readonly int _numDays;

    public BrownianMotionDrawable(int numberOfSimulations, double sigma, double mean, double initialPrice, int numDays)
    {
        _numDays = numDays;

        if (numberOfSimulations <= 0 || sigma < 0 || initialPrice <= 0 || numDays <= 0) return;

        for (int i = 0; i < numberOfSimulations; i++)
        {
            _simulations.Add(GraphicsHelper.GenerateBrownianMotion(sigma, mean, initialPrice, numDays));
        }
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_simulations is not { Count: > 0 }) return;

        float leftMargin = 80f;
        float rightMargin = 20f;
        float topMargin = 40f;
        float bottomMargin = 80f;

        float chartWidth = dirtyRect.Width - leftMargin - rightMargin;
        float chartHeight = dirtyRect.Height - topMargin - bottomMargin;
        RectF chartArea = new(leftMargin, topMargin, chartWidth, chartHeight);

        double minPrice = _simulations.Min(simulation => simulation.Min());
        double maxPrice = _simulations.Max(simulation => simulation.Max());
        double priceRange = maxPrice - minPrice;

        if (priceRange == 0) priceRange = 1;

        DrawBackground(canvas, chartArea);
        DrawVerticalScale(canvas, chartArea, minPrice, maxPrice);
        DrawHorizontalScale(canvas, chartArea, _numDays);

        foreach (double[] currentSimulation in _simulations)
        {
            if (currentSimulation is not { Length: >= 2 }) continue;

            DrawChart(canvas, chartArea, minPrice, priceRange, currentSimulation);
        }
    }

    private void DrawBackground(ICanvas canvas, RectF chartArea)
    {
        canvas.FillColor = Colors.White;
        canvas.FillRectangle(chartArea);

        canvas.StrokeColor = Colors.Gray;
        canvas.StrokeSize = 1;
        canvas.DrawRectangle(chartArea);
    }

    private void DrawVerticalScale(ICanvas canvas, RectF chartArea, double minPrice, double maxPrice)
    {
        canvas.FontSize = 12;
        canvas.FontColor = Colors.White;
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 0.5f;

        int numberOfTicks = 8;
        double priceRange = maxPrice - minPrice;
        double step = priceRange / (numberOfTicks - 1);

        for (int i = 0; i < numberOfTicks; i++)
        {
            double price = minPrice + (i * step);
            float y = chartArea.Bottom - i * chartArea.Height / (numberOfTicks - 1);

            canvas.DrawLine(chartArea.Left, y, chartArea.Right, y);

            string priceText = price.ToString("F2");
            RectF labelRect = new(10, y - 8, chartArea.Left - 15, 16);
            canvas.DrawString(priceText, labelRect, HorizontalAlignment.Right, VerticalAlignment.Center);
        }

        canvas.FontSize = 14;
        canvas.FontColor = Colors.White;
        RectF yTitleRect = new(5, chartArea.Top, 40, chartArea.Height);
        canvas.DrawString("Preço (R$)", yTitleRect, HorizontalAlignment.Center, VerticalAlignment.Center);
    }

    private void DrawHorizontalScale(ICanvas canvas, RectF chartArea, int days)
    {
        int interval = CalculateTimeInterval(days);

        canvas.FontSize = 10;
        canvas.FontColor = Colors.White;
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 0.5f;

        for (int day = 0; day < days; day += interval)
        {
            float x = chartArea.Left + (day * chartArea.Width / Math.Max(1, days - 1));

            if (x >= chartArea.Left && x <= chartArea.Right)
            {
                canvas.DrawLine(x, chartArea.Top, x, chartArea.Bottom);

                RectF dayRect = new(x - 20, chartArea.Bottom + 5, 40, 20);
                canvas.DrawString(day.ToString(), dayRect, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }

        if (days > 1 && (days - 1) % interval != 0)
        {
            float lastX = chartArea.Right;
            canvas.DrawLine(lastX, chartArea.Top, lastX, chartArea.Bottom);
            RectF lastDayRect = new(lastX - 20, chartArea.Bottom + 5, 40, 20);
            canvas.DrawString((days - 1).ToString(), lastDayRect, HorizontalAlignment.Center, VerticalAlignment.Center);
        }

        canvas.FontSize = 14;
        canvas.FontColor = Colors.White;
        RectF xTitleRect = new(chartArea.Left, chartArea.Bottom + 35, chartArea.Width, 25);
        canvas.DrawString("Tempo (dias)", xTitleRect, HorizontalAlignment.Center, VerticalAlignment.Center);
    }

    private void DrawChart(ICanvas canvas, RectF chartArea, double minPrice, double priceRange,
        double[] prices)
    {
        canvas.StrokeColor = ColorsHelper.GetRandomColor();
        canvas.StrokeSize = 2;

        float stepX = chartArea.Width / Math.Max(1, prices.Length - 1);

        for (int i = 1; i < prices.Length; i++)
        {
            float x0 = chartArea.Left + (i - 1) * stepX;
            float x1 = chartArea.Left + i * stepX;
            float y0 = chartArea.Bottom - (float)((prices[i - 1] - minPrice) / priceRange * chartArea.Height);
            float y1 = chartArea.Bottom - (float)((prices[i] - minPrice) / priceRange * chartArea.Height);

            canvas.DrawLine(x0, y0, x1, y1);
        }
    }

    private int CalculateTimeInterval(int days)
    {
        return days switch
        {
            <= 10 => 1,
            <= 30 => 5,
            <= 90 => 15,
            <= 252 => 30,
            _ => 50
        };
    }
}