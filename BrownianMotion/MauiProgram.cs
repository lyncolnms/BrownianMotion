using BrownianMotion.Features.BrownianGraphic;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BrownianMotion;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddPages<BrownianGraphicPage, BrownianGraphicPageViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void AddPages<TPage, TPageViewModel>(this IServiceCollection services)
        where TPage : ContentPage, new() 
        where TPageViewModel : class
    {
        services.AddTransient<TPageViewModel>();
        services.AddTransient<TPage>(serviceProvider => new TPage
        {
            BindingContext = serviceProvider.GetRequiredService<TPageViewModel>()
        });
    }
}