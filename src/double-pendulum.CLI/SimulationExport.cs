using double_pendulum.Model;

namespace double_pendulum.CLI;

/// <summary>
/// Export model for a finished simulation run. Holds creation time (UTC), duration, step size,
/// the initial pendulum parameters (angles in radians) and the cartesian positions.
/// </summary>
public class SimulationExport
{
    public DateTime CreatedAtUtc { get; }
    public float Duration { get; }
    public float StepSize { get; }
    public PendulumParameters Parameters { get; }
    public PositionsExport Positions { get; }


    public SimulationExport(DateTime createdAtUtc, float duration, float stepSize, PendulumParameters parameters, PositionsExport positions)
    {
        this.CreatedAtUtc = createdAtUtc;
        this.Duration = duration;
        this.StepSize = stepSize;
        this.Parameters = parameters;
        this.Positions = positions;
    }
}

public class PositionsExport
{
    public List<float> X1 { get; }
    public List<float> Y1 { get; }
    public List<float> X2 { get; }
    public List<float> Y2 { get; }

    public PositionsExport(List<float> x1, List<float> y1, List<float> x2, List<float> y2)
    {
        this.X1 = x1;
        this.Y1 = y1;
        this.X2 = x2;
        this.Y2 = y2;
    }
}