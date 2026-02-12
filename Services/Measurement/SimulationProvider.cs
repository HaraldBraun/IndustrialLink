using IndustrialLink.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialLink.Services.Measurement;

public class MeasurementEventArgs: EventArgs {
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}

public class SimulationProvider: IMeasurementProvider {
    private IDispatcherTimer? _timer;
    private Random _random = new ();
    private double _currentValue = 25.0;

    public bool IsRunning => _timer?.IsRunning ?? false;
    public void Start( int intervalMs ) => StartCapture( intervalMs );
    public void Stop( ) => StopCapture( );

    // Event, to register in ViewModel
    public event EventHandler<MeasurementEventArgs> DataReceived;

    public void StartCapture( int intervalMs ) {
        if (_timer != null) StopCapture( );

        _timer = Application.Current.Dispatcher.CreateTimer( );
        _timer.Interval = TimeSpan.FromMilliseconds( intervalMs );
        _timer.Tick += ( s, e ) => {
            _currentValue += (_random.NextDouble( ) - 0.5) * 2; // Simulation
            DataReceived?.Invoke( this, new MeasurementEventArgs
            {
                Value = _currentValue,
                Timestamp = DateTime.Now,
            } );
        };

        _timer.Start( );
    }

    public void StopCapture( ) {
        _timer?.Stop( );
    }
}
