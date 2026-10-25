using MongoDB.Bson;
using MongoDB.Driver;

namespace double_pendulum.CLI;

public static class MongoExporter
{
    private const string DatabaseName = "double-pendulum";
    private const string CollectionName = "dp-simulations";

    /// <summary>
    /// Inserts the simulation data into the MongoDB collection configured by <c>MONGODB_URI</c>.
    /// </summary>
    /// <param name="simExport">Export model containing simulation data.</param>
    public static void Export(ExportDTO simExport)
    {
        string? connectionUri = Environment.GetEnvironmentVariable("MONGODB_URI");

        if (string.IsNullOrWhiteSpace(connectionUri))
        {
            Console.WriteLine("MONGODB_URI is not set. MongoDB export skipped.");
            return;
        }

        MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionUri);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
        MongoClient client = new MongoClient(settings);
        try
        {
            client.GetDatabase(DatabaseName).GetCollection<ExportDTO>(CollectionName).InsertOne(simExport);
            Console.WriteLine("Exported to MongoDB.");
        }
        catch (Exception ex) when (ex is MongoException or TimeoutException or BsonException)
        {
            Console.WriteLine($"MongoDB export failed: {ex.Message}");
        }
    }
}
