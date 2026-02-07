using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IndustrialLink.Services;
using System.Collections.ObjectModel;

namespace IndustrialLink.ViewModels;
public partial class MainViewModel : ObservableObject {
    private readonly SerialPortService _serialService;

    public MainViewModel(SerialPortService serialService) {
        _serialService = serialService;

        // Beim Start direkt die Ports suchen
        LoadPorts( );
    }

    [ObservableProperty]
    private string _statusMessage = "Bereit zum Verbinden";
    
    public ObservableCollection<string> ReceivedData { get; } = new( );

    [RelayCommand]
    private void LoadPorts() {
        ReceivedData.Clear();
        var ports = _serialService.GetAvailablePorts();

        if (ports.Any()) {
            foreach (var port in ports ) {
                ReceivedData.Add( $"Gefundener Port: {port}" );
            }
            StatusMessage = $"{ports.Count} Port(s) gefunden.";
        } else {
            StatusMessage = "Keine COM-Ports gefunden.";
            ReceivedData.Add( "Hinweis: Schliessen Sie ein USB-Gerät an." );
        }
    }

    [RelayCommand]
    private async Task Connect() {
        StatusMessage = "Verbindung wird aufgebaut...";

        // Logik zum Öffnen eines Ports (Platzhalter)
        await Task.Delay( 500 );

        LoadPorts( ); // Aktualisiert die Liste beim Klick
    }
}

