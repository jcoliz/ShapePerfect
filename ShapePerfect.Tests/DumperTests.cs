using System.Reflection;
using ShapeCrawler;
using ShapePerfect.Lib;

namespace ShapePerfect.Tests;

public class DumperTests
{
    [Test]
    [Explicit("Failing test for in-progress feature")]
    /// <summary>
    /// Scenario: Can load presentation without error
    /// </summary>    
    public void LoadAndNothingElse()
    {
        var dumper = new Dumper();
        var pres = Load<IPresentation>("empty.pptx");
        dumper.Load(pres);
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