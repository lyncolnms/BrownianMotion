using BrownianMotion.Features.BrownianGraphic.Drawables;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BrownianMotion.Features.BrownianGraphic;

public partial class BrownianGraphicPageViewModel : ObservableObject
{
    [ObservableProperty] private IDrawable _brownianMotionDrawable;
    [ObservableProperty] private double _sigma;
    [ObservableProperty] private double _mean;
    [ObservableProperty] private double _initialPrice;
    [ObservableProperty] private int _numDays;

    [RelayCommand]
    private async Task GenerateBrownianMotion()
    {
        BrownianMotionDrawable = new BrownianMotionDrawable(Sigma/100, Mean/100, InitialPrice, NumDays);
    }

    [RelayCommand]
    private async Task ResetValues()
    {
        Sigma = 0;
        Mean = 0;
        InitialPrice = 0;
        NumDays = 0;
        BrownianMotionDrawable = new BrownianMotionDrawable(Sigma/100, Mean/100, InitialPrice, NumDays);
    }
}