using System.Reflection;
using System.Text;
using ShapeCrawler;
using ShapePerfect.Lib;
using ShapePerfect.Lib.Configuration;
using Tomlyn;

namespace ShapePerfect.Tests;

public class DumperTests
{
    /// <summary>
    /// Scenario: Test code can load presentation without error
    /// </summary>    
    [Test]
    public void LoadSampleData()
    {
        var pres = Load<IPresentation>("chaos.pptx");
        Assert.That(pres.Slides[0].Shapes, Has.Count.EqualTo(8));
    }

    /// <summary>
    /// Scenario: Can load slides from presentation without error
    /// </summary>
    [Test]
    public void LoadShapes()
    {
        var dumper = new Dumper();
        var pres = Load<IPresentation>("chaos.pptx");
        dumper.Load(pres);

        Assert.That(dumper.Shapes, Has.Count.EqualTo(8));
    }

    /// <summary>
    /// Scenario: Can dump results of load to Toml
    /// </summary>
    [Test]
    public void DumpShapes()
    {
        var dumper = new Dumper();
        var pres = Load<IPresentation>("chaos.pptx");
        dumper.Load(pres);

        var stringBuilder = new StringBuilder();
        using (TextWriter writer = new StringWriter(stringBuilder))
        {
            dumper.DumpToml(writer);
        }
        var lines = stringBuilder.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        File.Delete("DumpShapes.toml");
        using var stream = File.OpenWrite("DumpShapes.toml");
        using var fileWriter = new StreamWriter(stream);
        dumper.DumpToml(fileWriter);
        fileWriter.Flush();
        TestContext.AddTestAttachment("DumpShapes.toml");

        // Each shape should be 6 lines
        Assert.That(lines, Has.Length.EqualTo(48));
    }

    /// <summary>
    /// Scenario: Can load dump results back in as shapes
    /// </summary>
    [Test]
    public void LoadDumpedShapes()
    {
        var dumper = new Dumper();
        var pres = Load<IPresentation>("chaos.pptx");
        dumper.Load(pres);

        var stringBuilder = new StringBuilder();
        using (TextWriter writer = new StringWriter(stringBuilder))
        {
            dumper.DumpToml(writer);
        }
        var toml = stringBuilder.ToString();

        var loaded = Toml.ToModel<ShapeList>(toml);

        // Eight shapes were loaded
        Assert.That(loaded.shapes, Has.Count.EqualTo(8));
    }

    /// <summary>
    /// Scenario: Can adjust presentation using shapes
    /// </summary>
    [Test]
    public void AdjustPresentation()
    {
        // Given: A presentation with shapes organized chaotically
        var pres = Load<IPresentation>("chaos.pptx");

        // And: A shape list to impose order on those shapes
        var shapes = LoadToml<ShapeList>("order.toml");
        var dumper = new Dumper() { Shapes = shapes!.shapes };

        // When: Adjusting the presentation to match the shape list
        dumper.Adjust(pres);

        // Then: All boxes are 1.5 inches wide and high
        Assert.That(pres.Slides[0].Shapes.Where(x => x.Name.StartsWith("Box")), Has.All.Property("Width").EqualTo(1.5m * 96));
        Assert.That(pres.Slides[0].Shapes.Where(x => x.Name.StartsWith("Box")), Has.All.Property("Height").EqualTo(1.5m * 96));

        File.Delete("AdjustPresentation.pptx");
        pres.SaveAs("AdjustPresentation.pptx");
        TestContext.AddTestAttachment("AdjustPresentation.pptx");
    }

    private T Load<T>(string name) where T : class
    {
        var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        var filename = names.Single(x => x.EndsWith($".data.{name}"));
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);

        IPresentation result = new Presentation(stream!);

        return (T)result;
    }
    private T LoadToml<T>(string name) where T : class, new()
    {
        var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        var filename = names.Single(x => x.EndsWith($".data.{name}"));
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);
        using var reader = new StreamReader(stream!);
        var toml = reader.ReadToEnd();
        return Toml.ToModel<T>(toml);
    }    
}