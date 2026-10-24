using double_pendulum.Model;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;


namespace double_pendulum.CLI;

public static class SimulationPrinter
{
    /// <summary>
    /// Runs a full physics simulation on all initial parameters in real time.
    /// <remarks>Possibility to set a fixed sim timespan or start & end by key press.</remarks>
    /// </summary>
    /// <param name="parameters">Set of initial physical parameters of type <see cref="PendulumParameters"/></param>
    public static void Run(PendulumParameters parameters)
    {
        PendulumPhysics physics = new PendulumPhysics(parameters);

        List<float> x1 = new List<float>();
        List<float> y1 = new List<float>();
        List<float> x2 = new List<float>();
        List<float> y2 = new List<float>();

        bool fixedTime = false;
        bool timePromptBool = true;
        double timeSpan = 0.0;

        Console.WriteLine("\n-------------------------------------------------------------------------------");
        Console.WriteLine("Do you want to simulate for a fixed time, or start & end yourself by key press?\n");
        while (timePromptBool)
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
                timePromptBool = false;
            }
            else
            {
                Console.WriteLine("Invalid Input!\n");
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

            Vector4 positions = physics.GetPosition();
            Console.WriteLine($"{simTime, 12:F8}{positions.X, 12:F8}{positions.Y, 12:F8}{positions.Z, 12:F8}{positions.W, 12:F8}");

            x1.Add(positions.X);
            y1.Add(positions.Y);
            x2.Add(positions.Z);
            y2.Add(positions.W);

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

        PositionsExport positionsExport = new PositionsExport(x1, y1, x2, y2);
        PromptAndExport(simTime, stepSize, parameters, positionsExport);
    }


    /// <summary>
    /// Asks user if the data should be exported as JSON.
    /// </summary>
    /// <param name="duration">Simulation time length</param>
    /// <param name="stepSize">Simulation step size</param>
    /// <param name="parameters">Initial pendulum parameters</param>
    /// <param name="positions">Cartesian positions as separate lists</param>
    private static void PromptAndExport(float duration, float stepSize, PendulumParameters parameters, PositionsExport positions)
    {
        string answer;

        do
        {
            Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine("Want to export data to JSON? (y/n)");

            answer = (Console.ReadLine() ?? string.Empty).ToLower();

            if (answer != "y" && answer != "yes" && answer != "n" && answer != "no")
            {
                Console.WriteLine("Invalid input!");
            }
        }
        while (answer != "y" && answer != "yes" && answer != "n" && answer != "no");

        if (answer != "y" && answer != "yes") { return; }


        DateTime createdAtUtc = DateTime.UtcNow;
        SimulationExport simExport = new SimulationExport(createdAtUtc, duration, stepSize, parameters, positions);

        JsonExporter.Export(simExport);
    }
}
