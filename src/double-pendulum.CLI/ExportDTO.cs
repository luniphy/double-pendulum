using double_pendulum.Model;

namespace double_pendulum.CLI;

/// <summary>
/// Export model for a finished simulation run. Creation time is in UTC and pendulum angles are in radians.
/// </summary>
public class ExportDTO
{
    public DateTime CreatedAtUtc { get; }
    public float Duration { get; }
    public float StepSize { get; }
    public PendulumParameters Parameters { get; }
    public Coordinates Coordinates { get; }


    public ExportDTO(DateTime createdAtUtc, float duration, float stepSize, PendulumParameters parameters, Coordinates coordinates)
    {
        this.CreatedAtUtc = createdAtUtc;
        this.Duration = duration;
        this.StepSize = stepSize;
        this.Parameters = parameters;
        this.Coordinates = coordinates;
    }
}

/// <summary>
/// Lists of the cartesian coordinates of the pendulums.
/// </summary>
public class Coordinates
{
    public List<float> X1 { get; }
    public List<float> Y1 { get; }
    public List<float> X2 { get; }
    public List<float> Y2 { get; }

    public Coordinates(List<float> x1, List<float> y1, List<float> x2, List<float> y2)
    {
        this.X1 = x1;
        this.Y1 = y1;
        this.X2 = x2;
        this.Y2 = y2;
    }
}