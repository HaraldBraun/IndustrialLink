using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IndustrialLink.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace IndustrialLink.ViewModels;
public partial class MainViewModel: ObservableObject {
    private readonly SerialPortService _serialService;
    private readonly MeasurementService _measurementService;
    private readonly DataStorageService _dataStorageService;

    private readonly ObservableCollection<double> _chartValues = new()  { 2, 1, 3, 5, 3, 4, 6 };
    public ObservableCollection<Axis> XAxes { get; set; } = new( ) { new Axis { Name = "Zeit" } };
    public ObservableCollection<Axis> YAxes { get; set; } = new( ) { new Axis { Name = "Messwert" } };

    // Graph properties
    [ObservableProperty]
    private ISeries[] _series;
    [ObservableProperty]
    private int _maxDataPoints = 50; // Default 

    public MainViewModel( SerialPortService serialService, MeasurementService measurement, DataStorageService storage ) {
        _serialService = serialService;
        _measurementService = measurement;
        _dataStorageService = storage;

        // Register event
        _measurementService.DataReceived += OnDataReceived;

        // Initialise graph series
        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = _chartValues,
                Fill = null,
                GeometrySize = 5
            }
        };

        // Beim Start direkt die Ports suchen
        LoadPorts( );
    }

    [ObservableProperty]
    private string _statusMessage = "Bereit zum Verbinden";

    public ObservableCollection<string> ReceivedData { get; } = new( );

    private void OnDataReceived(object sender, MeasurementEventArgs e) {
        // Initialise graph
        _chartValues.Add( e.Value );

        // Dynamic data limitiation through UI field
        while (_chartValues.Count > _maxDataPoints) {
            _chartValues.RemoveAt( 0 );
        }

        // Update log
        ReceivedData.Insert( 0, $"{e.Timestamp:HH:mm:ss} -> {e.Value:F2}" );

        // Call memory placeholder
        _ = _dataStorageService.AppendMeasureAsync( e.Value, "V" );
    }
    [RelayCommand]
    private void LoadPorts( ) {
        ReceivedData.Clear( );
        var ports = _serialService.GetAvailablePorts();

        if (ports.Any( )) {
            foreach (var port in ports) {
                ReceivedData.Add( $"Gefundener Port: {port}" );
            }
            StatusMessage = $"{ports.Count} Port(s) gefunden.";
        } else {
            StatusMessage = "Keine COM-Ports gefunden.";
            ReceivedData.Add( "Hinweis: Schliessen Sie ein USB-Gerät an." );
        }
    }

    [RelayCommand]
    private async Task Connect( ) {
        StatusMessage = "Verbindung wird aufgebaut...";

        // Logik zum Öffnen eines Ports (Platzhalter)
        await Task.Delay( 500 );

        LoadPorts( ); // Aktualisiert die Liste beim Klick
    }
}

