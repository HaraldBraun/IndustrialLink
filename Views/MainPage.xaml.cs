using IndustrialLink.ViewModels;

namespace IndustrialLink.Views;

public partial class MainPage : ContentPage {
    public MainPage( MainViewModel viewModel ) {
        InitializeComponent( );
        BindingContext = viewModel;
    }
}
