using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialLink.Services;

public class MeasurementEventArgs: EventArgs {
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}

public class MeasurementService {
    private IDispatcherTimer? _timer;
    private Random _random = new ();
    private double _currentValue = 25.0;

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
