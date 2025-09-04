using BrownianMotion.Features.BrownianGraphic.Drawables;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BrownianMotion.Features.BrownianGraphic;

public partial class BrownianGraphicPageViewModel : ObservableObject
{
    [ObservableProperty] private IDrawable _brownianMotionDrawable;
    [ObservableProperty] private double _sigma = 20;
    [ObservableProperty] private double _mean = 1;
    [ObservableProperty] private double _initialPrice = 100;
    [ObservableProperty] private int _numDays = 252;

    [RelayCommand]
    private async Task GenerateBrownianMotion()
    {
        BrownianMotionDrawable = new BrownianMotionDrawable(Sigma/100, Mean/100, InitialPrice, NumDays);
    }
}