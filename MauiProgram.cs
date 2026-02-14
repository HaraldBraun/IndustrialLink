using IndustrialLink.Services.DataStorage;
using IndustrialLink.Services.Measurement;
using IndustrialLink.Services.SerialPorts;
using IndustrialLink.ViewModels;
using IndustrialLink.Views;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace IndustrialLink {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp( ) {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>( )
                .UseSkiaSharp( )
                .UseLiveCharts( )
                .ConfigureFonts( fonts => {
                    fonts.AddFont( "OpenSans-Regular.ttf", "OpenSansRegular" );
                    fonts.AddFont( "OpenSans-Semibold.ttf", "OpenSansSemibold" );
                } );

#if DEBUG
            builder.Logging.AddDebug( );
#endif

            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping( "NoLabel", ( handler, view ) => {
#if WINDOWS
            handler.PlatformView.OnContent = null;
            handler.PlatformView.OffContent = null;
            handler.PlatformView.MinWidth = 0;
#endif
            } );

            // Services
            builder.Services.AddSingleton<SerialPortService>( );
            builder.Services.AddSingleton<DataStorageService>( );
            builder.Services.AddSingleton<SimulationProvider>( );
            builder.Services.AddSingleton<SerialMeasurementProvider>( );

            // ViewModels
            builder.Services.AddSingleton<MainViewModel>( );

            // Views 
            builder.Services.AddSingleton<MainPage>( );

            return builder.Build( );
        }
    }
}
