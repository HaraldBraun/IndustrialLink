using IndustrialLink.Services.Measurement;

namespace IndustrialLink.Services.Interfaces;

public interface IMeasurementProvider {
    event EventHandler<MeasurementEventArgs> DataReceived;
    void Start( int intervalMs );
    void Stop( );
    bool IsRunning { get; }
}
