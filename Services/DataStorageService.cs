namespace IndustrialLink.Services;

public class DataStorageService {
    private readonly string _filePath;

    public DataStorageService( ) {
        // Save to standard dokument folder below IndustrialLink
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IndustrialLink");

        if (!Directory.Exists( folder ) ) {
            Directory.CreateDirectory( folder );
        }

        _filePath = Path.Combine( folder, $"Session_{DateTime.Now:yyyyMMdd_HHmm}.csv" );

        if (!File.Exists( _filePath ) ) {
            File.WriteAllText(_filePath, $"Zeitstempel;Wert;Einheit{Environment.NewLine}");
        }
    }

    public async Task AppendMeasureAsync( double value, string unit ) {
        string line = $"{DateTime.Now:0};{value};{unit}{Environment.NewLine}";
        await File.AppendAllTextAsync( _filePath, line );
    }
}
