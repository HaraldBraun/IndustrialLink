using IndustrialLink.ViewModels;

namespace IndustrialLink.Views;

public partial class MainPage : ContentPage {
    public MainPage( MainViewModel viewModel ) {
        try {
            InitializeComponent( );
            BindingContext = viewModel;
        } catch (Exception ex ) {
            System.Diagnostics.Debug.WriteLine( ex.ToString( ) );
            throw;
        }
    }
}
