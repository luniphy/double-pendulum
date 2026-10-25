using System.Text.Json;

namespace double_pendulum.CLI;

public static class JsonExporter
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// Serializes the simulation to output/simulation_timestamp.json.
    /// </summary>
    /// <param name="simExport">Export model containing simulation data.</param>
    public static void Export(ExportDTO simExport)
    {
        //string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        //string outputPath = Path.Combine(rootPath, "output");
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputPath);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
        string filePath = Path.Combine(outputPath, $"simulation_{timestamp}.json");

        try
        {
            string json = JsonSerializer.Serialize(simExport, _jsonOptions);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Exported to: {filePath}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Export failed: {e.Message}");
        }
    }
}
