using System.Reflection;
using ShapeCrawler;
using ShapePerfect.Lib;

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
    /// Scenario: Can load presentation without error
    /// </summary>    
    [Test]
    [Explicit]
    public void LoadShapes()
    {
        var dumper = new Dumper();
        var pres = Load<IPresentation>("chaos.pptx");
        dumper.Load(pres);

        Assert.That(dumper.Shapes, Has.Count.EqualTo(8));
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