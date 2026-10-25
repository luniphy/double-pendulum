using double_pendulum.Model;

namespace double_pendulum.CLI;

public class Program
{
    public static void Main()
    {
        bool again;

        do
        {
            PendulumParameters parameters = ParameterInputHandler.ReadParameters();
            ExportDTO simExport = Simulation.Run(parameters);

            if (ConsolePrompt.AskYesNo("Want to export data to JSON? (y/n)"))
            {
                JsonExporter.Export(simExport);
            }

            if (ConsolePrompt.AskYesNo("Want to export data to MongoDB? (y/n)"))
            {
                MongoExporter.Export(simExport);
            }

            again = ConsolePrompt.AskYesNo("Run another simulation? (y/n)");
        }
        while (again);
    }
}
