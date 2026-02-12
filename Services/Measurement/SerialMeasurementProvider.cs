using IndustrialLink.Services.Interfaces;

namespace IndustrialLink.Services.Measurement;
public class SerialMeasurementProvider : IMeasurementProvider {
    public bool IsRunning { get; private set; }

    public event EventHandler<MeasurementEventArgs> DataReceived;

    public void Start(int intervalMs) {
        IsRunning = true;

        // Logic to open COM Ports
    }

    public void Stop() {
        IsRunning = false;

        // Close all open Ports
    }
}
