using double_pendulum.Model;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;


namespace double_pendulum.CLI;

public static class Simulation
{
    /// <summary>
    /// Runs a full physics simulation on all initial parameters in real time.
    /// </summary>
    /// <remarks>Possibility to set a fixed sim timespan or start and end by key press.</remarks>
    /// <param name="parameters">Set of initial physical parameters of type <see cref="PendulumParameters"/></param>
    /// <returns>Simulation export data transfer object of type <see cref="ExportDTO"/></returns>
    public static ExportDTO Run(PendulumParameters parameters)
    {
        PendulumPhysics physics = new PendulumPhysics(parameters);

        List<float> x1 = new List<float>();
        List<float> y1 = new List<float>();
        List<float> x2 = new List<float>();
        List<float> y2 = new List<float>();

        bool fixedTime = false;
        bool timePromptIsInvalid = true;
        double timeSpan = 0.0;

        Console.WriteLine("\n-------------------------------------------------------------------------------");
        Console.WriteLine("Do you want to simulate for a fixed time, or start & end yourself by key press?\n");
        while (timePromptIsInvalid)
        {
            Console.WriteLine("Enter time span in [s]: (Enter 0 if you want key press based start & stop.)");
            string input = Console.ReadLine() ?? string.Empty;
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                if (value != 0)
                {
                    fixedTime = true;
                    timeSpan = value;
                }
                timePromptIsInvalid = false;
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }

        Console.WriteLine(fixedTime ? "\nPress any key to Start the simulation.\n" : "\nPress any key to Start & Stop the simulation.\n");
        
        Console.WriteLine($"{"t",12}{"X1",12}{"Y1",12}{"X2",12}{"Y2", 12}");
        Console.ReadKey(intercept: true);

        float simTime = 0.0f;
        float stepSize = physics.StepSize;

        Stopwatch stopwatch = Stopwatch.StartNew();
        TimeSpan previousTime = stopwatch.Elapsed;
        double accumulator = 0.0;

        bool running = true;
        while (running)
        {
            TimeSpan currentTime = stopwatch.Elapsed;
            double deltaTime = (currentTime - previousTime).TotalSeconds;
            previousTime = currentTime;
            accumulator += deltaTime;

            while (accumulator >= stepSize)
            {
                physics.Step();
                simTime += stepSize;
                accumulator -= stepSize;
            }

            Vector4 position = physics.GetPosition();
            Console.WriteLine($"{simTime, 12:F8}{position.X, 12:F8}{position.Y, 12:F8}{position.Z, 12:F8}{position.W, 12:F8}");

            x1.Add(position.X);
            y1.Add(position.Y);
            x2.Add(position.Z);
            y2.Add(position.W);

            Thread.Sleep(1); // Yield CPU and cap the console output rate.

            if (fixedTime)
            {
                running = simTime < timeSpan;
            }
            else
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(intercept: true);
                    running = false;
                }
            }
        }

        // Flush any buffered key presses before export prompt.
        while (Console.KeyAvailable)
            Console.ReadKey(intercept: true);

        Coordinates coordinates = new Coordinates(x1, y1, x2, y2);
        DateTime createdAtUtc = DateTime.UtcNow;
        ExportDTO simExport = new ExportDTO(createdAtUtc, simTime, stepSize, parameters, coordinates);

        return simExport;
    }
}
