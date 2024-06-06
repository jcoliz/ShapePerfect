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
        var shapes = presentation.Slides[0].Shapes
            .ToDictionary(
                x => x.Name,
                x => new Shape()
                {
                    X = x.X / ShapeList.Dpi,
                    Y = x.Y / ShapeList.Dpi,
                    Width = x.Width / ShapeList.Dpi,
                    Height = x.Height / ShapeList.Dpi
                });

        Shapes = new ShapeList(shapes);
    }

    /// <summary>
    /// Output loaded shapes to TOML text which could be loaded back in
    /// </summary>
    public void DumpToml(TextWriter writer)
    {
        if (Shapes is null )
        {
            return;
        }

        foreach (var shape in Shapes)
        {
            writer.WriteLine($"[{shape.Key}]");
            if (shape.Value.X is not null)
            {
                writer.WriteLine($"x = {shape.Value.X}");
            }
            if (shape.Value.Y is not null)
            {
                writer.WriteLine($"y = {shape.Value.Y}");
            }
            if (shape.Value.X is not null)
            {
                writer.WriteLine($"width = {shape.Value.Width}");
            }
            if (shape.Value.X is not null)
            {
                writer.WriteLine($"height = {shape.Value.Height}");
            }
            writer.WriteLine();
        }
    }
}
