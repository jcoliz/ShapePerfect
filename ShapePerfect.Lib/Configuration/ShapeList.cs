namespace ShapePerfect.Lib.Configuration;

/// <summary>
/// Collection of shape adjustments, indexed by name of underlying shape
/// </summary>
public class ShapeList: Dictionary<string, Shape>
{
    public const decimal Dpi = 96;
    
    public ShapeList()
    {        
    }

    public ShapeList(IEnumerable<KeyValuePair<string,Shape>> source): base(source)
    {        
    }
}
