using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IndustrialLink.Services.DataStorage;
using IndustrialLink.Services.Interfaces;
using IndustrialLink.Services.Measurement;
using IndustrialLink.Services.SerialPorts;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace IndustrialLink.ViewModels;
public partial class MainViewModel: ObservableObject {
    // Actual provider
    private IMeasurementProvider _currentProvider;

    // Concrete provider
    private readonly SimulationProvider _simProvider;
    private readonly SerialMeasurementProvider _hardwareProvider;

    // Services for hardware 
    private readonly DataStorageService _dataStorageService;
    private readonly SerialPortService _serialService;

    private readonly ObservableCollection<double> _chartValues = new()  { 2, 1, 3, 5, 3, 4, 6 };
    public ObservableCollection<Axis> XAxes { get; set; } = new( ) { new Axis { Name = "Zeit" } };
    public ObservableCollection<Axis> YAxes { get; set; } = new( ) { new Axis { Name = "Messwert" } };
    public ObservableCollection<string> AvailablePorts { get; } = new( );

    [ObservableProperty]
    private string _statusMessage = "Bereit zum Verbinden";
    [ObservableProperty]
    private bool _isHardwareMode;
    [ObservableProperty]
    private string _selectedPort;
    // Graph properties
    [ObservableProperty]
    private ISeries[] _series;
    [ObservableProperty]
    private int _maxDataPoints = 50; // Default 

    public MainViewModel( SerialPortService serialService, SimulationProvider simProvider, SerialMeasurementProvider hardwareProvider, DataStorageService storage ) {
        _serialService = serialService;
        _simProvider = simProvider;
        _hardwareProvider = hardwareProvider;
        _dataStorageService = storage;

        // Start simulation as default
        _currentProvider = _simProvider;

        // Register event
        _currentProvider.DataReceived += OnDataReceived;

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

    partial void OnIsHardwareModeChanged( bool value ) {
        // Calls SetProvider method
        SetProvider( value );

        // Feedback in Log
        StatusMessage = value ? "Modus gewechselt: Hardware" : "Modus gewechselt: Simulation";
    }

    public ObservableCollection<string> ReceivedData { get; } = new( );

    private async void OnDataReceived( object sender, MeasurementEventArgs e ) {
        // UI updates at MainThread (Graph & Log)
        MainThread.BeginInvokeOnMainThread( ( ) => {
            // Initialise graph
            _chartValues.Add( e.Value );

            // Dynamic data limitiation through UI field
            while (_chartValues.Count > MaxDataPoints) {
                _chartValues.RemoveAt( 0 );
            }

            // Update log
            //ReceivedData.Insert( 0, $"{e.Timestamp:HH:mm:ss} -> {e.Value:F2}" );
        } );

        // Data record 
        try {
            // Call memory placeholder
            await _dataStorageService.AppendMeasureAsync( e.Value, "V" );
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine( $"Fehler beim Speichern: {ex.Message}" );
        }
    }
    [RelayCommand]
    private void LoadPorts( ) {
        AvailablePorts.Clear( );
        var ports = _serialService.GetAvailablePorts();

        foreach (var port in ports) {
            AvailablePorts.Add( $"Gefundener Port: {port}" );
        }

        if (ports.Any( )) {
            SelectedPort = AvailablePorts[0];
            StatusMessage = $"{AvailablePorts.Count} Port(s) gefunden.";
        } else {
            StatusMessage = "Kein Gerät gefunden.";
        }
    }

    [RelayCommand]
    private async Task Connect( ) {
        StatusMessage = "Verbindung wird aufgebaut...";

        // Start simulation
        _currentProvider.Start( 500 );

        // Logik zum Öffnen eines Ports (Platzhalter)
        await Task.Delay( 500 );
        StatusMessage = IsHardwareMode ? "Hardware-Messung läuft ..." : "Simulation läuft ...";

        LoadPorts( ); // Aktualisiert die Liste beim Klick
    }

    [RelayCommand]
    private void Start( ) {
        if (IsHardwareMode && string.IsNullOrEmpty( SelectedPort )) {
            StatusMessage = "Bitte zuerst einen Port wählen!";
            return;
        }

        _currentProvider.Start( 500 );
        StatusMessage = IsHardwareMode ? $"Messung auf {SelectedPort} ..." : "Simulation läuft ...";
    }

    [RelayCommand]
    private void Stop( ) {
        _currentProvider?.Stop( );
        StatusMessage = "Aufzeichnung gestoppt.";
    }

    public void SetProvider( bool useHardware ) {
        _currentProvider.Stop( );
        _currentProvider.DataReceived -= OnDataReceived;

        _currentProvider = useHardware ? _hardwareProvider : _simProvider;

        _currentProvider.DataReceived += OnDataReceived;
    }
}

