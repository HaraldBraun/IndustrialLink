namespace IndustrialLink.Services;

public class DataStorageService {
    private readonly string _filePath;

    public DataStorageService( ) {
        // Save to temporary folder below IndustrialLink
        string folder = Path.Combine(Path.GetTempPath(), "IndustrialLink");

        if (!Directory.Exists( folder )) {
            Directory.CreateDirectory( folder );
        }

        _filePath = Path.Combine( folder, $"Session_{DateTime.Now:yyyyMMdd_HHmm}.csv" );

        if (!File.Exists( _filePath )) {
            File.WriteAllText( _filePath, $"Zeitstempel;Wert;Einheit{Environment.NewLine}" );
        }
    }

    public async Task AppendMeasureAsync( double value, string unit ) {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string line = $"{timestamp};{value.ToString("F2")};{unit}{Environment.NewLine}";
        await File.AppendAllTextAsync( _filePath, line );
    }
}
