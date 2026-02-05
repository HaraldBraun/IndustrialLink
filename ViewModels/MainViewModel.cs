using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace IndustrialLink.ViewModels;
public partial class MainViewModel : ObservableObject {
    [ObservableProperty]
    private string _statusMessage = "Bereit zum Verbinden";
    public ObservableCollection<string> ReceivedData { get; } = new( );

    [RelayCommand]
    private async Task Connect() {
        StatusMessage = "Verbindung wird aufgebaut...";

        await Task.Delay( 2000 ); // Simulates Hardware Search

        ReceivedData.Add( $"{DateTime.Now:HH:mm:ss}: Test-Device gefunden." );
        StatusMessage = "Verbunden"; 
    }
}

