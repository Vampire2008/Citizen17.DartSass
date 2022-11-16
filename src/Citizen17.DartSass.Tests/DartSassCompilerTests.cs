namespace Citizen17.DartSass.Tests;

[TestClass]
public class DartSassCompilerTests
{
    [TestMethod]
    public void CreateWithDefaultConstructorTest()
    {
        Assert.IsNotNull(new DartSassCompiler());
    }
}