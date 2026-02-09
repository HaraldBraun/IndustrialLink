using System.IO.Ports;

namespace IndustrialLink.Services;

public class SerialPortService {
    public List<string> GetAvailablePorts() {
        // Gibt eine Liste wie ["COM1", "COM3"] zurück
        return SerialPort.GetPortNames( ).ToList( );
    }
}