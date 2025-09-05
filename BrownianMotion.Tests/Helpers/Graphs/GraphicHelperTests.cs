using BrownianMotion.Helpers.Graphs;
using FluentAssertions;

namespace BrownianMotion.Tests.Helpers.Graphs;

[TestFixture]
public class GraphicHelperTests
{
    [Test]
    public void ShouldGenerateBrownianMotionPrices()
    {
        double sigma = 0.1;
        double mean = 0.05;
        double initialPrice = 100.0;
        int numDays = 10;

        double[] prices = GraphicsHelper.GenerateBrownianMotion(sigma, mean, initialPrice, numDays);

        Assert.That(prices.Length, Is.EqualTo(numDays));
        Assert.That(prices[0], Is.EqualTo(initialPrice));
        for (int i = 1; i < numDays; i++)
        {
            Assert.That(prices[i], Is.GreaterThan(0));
        }
    }

    [Test]
    public void GenerateBrownianMotion_ProducesNoNaNValues()
    {
        double sigma = 0.1;
        double mean = 0.05;
        double initialPrice = 100.0;
        int numDays = 10;

        double[] prices = GraphicsHelper.GenerateBrownianMotion(sigma, mean, initialPrice, numDays);

        foreach (double price in prices)
        {
            Assert.That(double.IsNaN(price), Is.False);
        }
    }
}