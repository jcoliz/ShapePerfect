namespace ShapePerfect.Lib.Configuration;

/// <summary>
/// Adjustable properties of a shape
/// </summary>
/// <remarks>
/// Null values indicate no adjustment is desired to that property of the
/// underlying shape.
/// </remarks>
public record Shape
{
    public string Name
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Horizontal position of shape, in inches
    /// </summary>
    public decimal? X {
        get;
        set;
    }
    /// <summary>
    /// Vertical position of shape, in inches
    /// </summary>
    public decimal? Y {
        get;
        set;
    }
    /// <summary>
    /// Width of shape, in inches
    /// </summary>
    public decimal? Width {
        get;
        set;
    }
    /// <summary>
    /// Height of shape, in inches
    /// </summary>
    public decimal? Height {
        get;
        set;
    }
}
