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

        // Each shape should be 5 lines
        Assert.That(lines, Has.Length.EqualTo(40));
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

    private T Load<T>(string name) where T : class
    {
        var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        var filename = names.Where(x => x.EndsWith($".data.{name}")).Single();
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);

        IPresentation result = new Presentation(stream!);

        return (T)result;
    }
}