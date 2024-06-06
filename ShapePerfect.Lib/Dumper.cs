using ShapeCrawler;
using ShapePerfect.Lib.Configuration;

namespace ShapePerfect.Lib;

/// <summary>
/// Utilities to load presentation into configuration objects
/// </summary>
public class Dumper
{
    public List<Shape> Shapes { get; set; } = new();

    /// <summary>
    /// Load presentation into configuration objects
    /// </summary>
    public void Load(IPresentation presentation)
    {
        var shapes = presentation.Slides[0].Shapes
            .Select(
                x => new Shape()
                {
                    Name = x.Name,
                    X = x.X / ShapeList.Dpi,
                    Y = x.Y / ShapeList.Dpi,
                    Width = x.Width / ShapeList.Dpi,
                    Height = x.Height / ShapeList.Dpi
                });
        Shapes.Clear();
        Shapes.AddRange(shapes);
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
            writer.WriteLine($"[[shapes]]");
            writer.WriteLine($"name = \"{shape.Name}\"");
            if (shape.X is not null)
            {
                writer.WriteLine($"x = {shape.X}");
            }
            if (shape.Y is not null)
            {
                writer.WriteLine($"y = {shape.Y}");
            }
            if (shape.Width is not null)
            {
                writer.WriteLine($"width = {shape.Width}");
            }
            if (shape.Height is not null)
            {
                writer.WriteLine($"height = {shape.Height}");
            }
            writer.WriteLine();
        }
    }

    public void Adjust(IPresentation presentation)
    {
        foreach(var shape in Shapes)
        {
            var adjustMe = presentation.Slides[0].ShapeWithName(shape.Name);

            if (shape.X is not null)
            {
                adjustMe.X = shape.X.Value * ShapeList.Dpi;
            }
            if (shape.Y is not null)
            {
                adjustMe.Y = shape.Y.Value * ShapeList.Dpi;
            }
            if (shape.Width is not null)
            {
                adjustMe.Width = shape.Width.Value * ShapeList.Dpi;
            }
            if (shape.Height is not null)
            {
                adjustMe.Height = shape.Height.Value * ShapeList.Dpi;
            }
        }
    }
}
