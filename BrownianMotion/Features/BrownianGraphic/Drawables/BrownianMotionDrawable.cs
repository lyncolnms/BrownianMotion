using BrownianMotion.Helpers.Graphs;
using Font = Microsoft.Maui.Graphics.Font;

namespace BrownianMotion.Features.BrownianGraphic.Drawables;

public class BrownianMotionDrawable : IDrawable
{
    private readonly double[] _prices;
    private readonly int _numDays;

    public BrownianMotionDrawable(double sigma, double mean, double initialPrice, int numDays)
    {
        _numDays = numDays;

        if (sigma == 0 && mean == 0 && initialPrice == 0 && numDays == 0)
        {
            _prices = null;
        }
        else
        {
            _prices = GraphicsHelpers.GenerateBrownianMotion(sigma, mean, initialPrice, numDays);
        }
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_prices is not { Length: >= 2 }) return;

        float leftMargin = 80f;
        float rightMargin = 20f;
        float topMargin = 40f;
        float bottomMargin = 80f;

        float chartWidth = dirtyRect.Width - leftMargin - rightMargin;
        float chartHeight = dirtyRect.Height - topMargin - bottomMargin;
        RectF chartArea = new(leftMargin, topMargin, chartWidth, chartHeight);

        double minPrice = _prices.Min();
        double maxPrice = _prices.Max();
        double priceRange = maxPrice - minPrice;

        if (priceRange == 0) priceRange = 1;

        (double niceMin, double niceMax, double step, int decimals) = CalculateScale(minPrice, maxPrice, 8);

        DrawBackground(canvas, chartArea);
        DrawVerticalScale(canvas, chartArea, minPrice, maxPrice, niceMin, niceMax, step, decimals);
        DrawHorizontalScale(canvas, chartArea, _numDays);
        DrawChart(canvas, chartArea, minPrice, priceRange);
    }

    private void DrawBackground(ICanvas canvas, RectF chartArea)
    {
        canvas.FillColor = Colors.White;
        canvas.FillRectangle(chartArea);

        canvas.StrokeColor = Colors.Gray;
        canvas.StrokeSize = 1;
        canvas.DrawRectangle(chartArea);
    }

    private void DrawVerticalScale(ICanvas canvas, RectF chartArea, double dataMin, double dataMax, double niceMin,
        double niceMax, double step, int decimals)
    {
        canvas.FontSize = 12;
        canvas.FontColor = Colors.White;
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 0.5f;

        double dataRange = dataMax - dataMin;
        if (dataRange == 0) dataRange = 1;

        for (double price = niceMin; price <= niceMax + step / 2; price += step)
        {
            float y = chartArea.Bottom - (float)((price - dataMin) / dataRange * chartArea.Height);

            if (y >= chartArea.Top && y <= chartArea.Bottom)
            {
                canvas.DrawLine(chartArea.Left, y, chartArea.Right, y);

                string format = decimals > 0 ? $"F{decimals}" : "F0";
                string priceText = price.ToString(format);
                RectF labelRect = new(10, y - 8, chartArea.Left - 15, 16);
                canvas.DrawString(priceText, labelRect, HorizontalAlignment.Right, VerticalAlignment.Center);
            }
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

    private void DrawChart(ICanvas canvas, RectF chartArea, double minPrice, double priceRange)
    {
        canvas.StrokeColor = Colors.BlueViolet;
        canvas.StrokeSize = 2;

        float stepX = chartArea.Width / Math.Max(1, _prices.Length - 1);

        for (int i = 1; i < _prices.Length; i++)
        {
            float x0 = chartArea.Left + (i - 1) * stepX;
            float x1 = chartArea.Left + i * stepX;
            float y0 = chartArea.Bottom - (float)((_prices[i - 1] - minPrice) / priceRange * chartArea.Height);
            float y1 = chartArea.Bottom - (float)((_prices[i] - minPrice) / priceRange * chartArea.Height);

            canvas.DrawLine(x0, y0, x1, y1);
        }
    }

    private (double min, double max, double step, int decimals) CalculateScale(double dataMin, double dataMax,
        int targetTicks)
    {
        double range = dataMax - dataMin;
        if (range == 0) range = Math.Abs(dataMax) * 0.1;

        double roughStep = range / (targetTicks - 1);

        double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(roughStep))));
        double normalizedStep = roughStep / magnitude;

        double niceStep = normalizedStep switch
        {
            <= 1 => 1,
            <= 2 => 2,
            <= 5 => 5,
            _ => 10
        };

        double step = niceStep * magnitude;
        double niceMin = Math.Floor(dataMin / step) * step;
        double niceMax = Math.Ceiling(dataMax / step) * step;

        int decimals = Math.Max(0, -(int)Math.Floor(Math.Log10(Math.Abs(step))));

        return (niceMin, niceMax, step, decimals);
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