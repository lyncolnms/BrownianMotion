namespace BrownianMotion.Helpers.Graphs;

public static class GraphicsHelpers
{
    public static double[] GenerateBrownianMotion(double sigma, double mean, double initialPrice, int numDays)
    {
        Random rand = new();
        double[] prices = new double[numDays];
        prices[0] = initialPrice;

        for (int i = 1; i < numDays; i++)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

            double retornoDiario = mean + sigma * z;
            
            prices[i] = prices[i - 1] * Math.Exp(retornoDiario);

            // DEBUG: Adicione isso temporariamente para ver os valores
            if (i <= 5) // Apenas os primeiros 5 valores
            {
                System.Diagnostics.Debug.WriteLine($"Day {i}: price = {prices[i]:F2}, return = {retornoDiario:F4}");
            }
        }

        System.Diagnostics.Debug.WriteLine($"Final range: Min = {prices.Min():F2}, Max = {prices.Max():F2}");

        return prices;
    }
}