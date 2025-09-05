using BrownianMotion.Helpers.Colors;
using FluentAssertions;

namespace BrownianMotion.Tests.Helpers.Colors;

[TestFixture]
public class ColorsHelperTests
{
    [Test]
    public void ShouldReturnRandomColor()
    {
        ColorsHelper.GetRandomColor().ToRgb(out byte red, out byte green, out byte blue);

        red.Should().BeInRange(50, 205);
        green.Should().BeInRange(50, 205);
        blue.Should().BeInRange(50, 205);
    }
}