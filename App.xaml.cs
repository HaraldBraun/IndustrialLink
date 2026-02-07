
namespace IndustrialLink;

public partial class App : Application {
    public App( ) {
        InitializeComponent( );

        //MainPage = new AppShell( );
    }

    protected override Window CreateWindow( IActivationState? activationState ) {
        var window = new Window( new AppShell( ) );

        // Initial window size and position
        window.Width = 1024;
        window.Height = 800;
        window.X = 100;
        window.Y = 100;

        // Window title at runtime
        window.Title = "IndustrialLink - Hardware Monitor";

        return window;
    }
}
