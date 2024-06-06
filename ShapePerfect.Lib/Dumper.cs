using ShapeCrawler;
using ShapePerfect.Lib.Configuration;

namespace ShapePerfect.Lib;

/// <summary>
/// Utilities to load presentation into configuration objects
/// </summary>
public class Dumper
{
    public ShapeList? Shapes {
        get;
        private set;
    }
    
    /// <summary>
    /// Load presentation into configuration objects
    /// </summary>
    public void Load(IPresentation presentation)
    {
        // In progress
    }

    /// <summary>
    /// Output loaded shapes to TOML text which could be loaded back in
    /// </summary>
    public void DumpToml(TextWriter writer)
    {
        throw new NotImplementedException();
    }
}
