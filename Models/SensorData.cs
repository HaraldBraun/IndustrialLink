namespace IndustrialLink.Models;

public class SensorData {
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public double Value { get; set; }
    public string Unit { get; set; } = "V";     // Unit string for value
}
