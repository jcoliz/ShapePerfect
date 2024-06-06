namespace ShapePerfect.Lib.Configuration;

/// <summary>
/// Collection of shape adjustments, indexed by name of underlying shape
/// </summary>
public record ShapeList
{
    public const decimal Dpi = 96;

    public List<Shape> shapes
    {
        get;
        set;
    } = new();
}
