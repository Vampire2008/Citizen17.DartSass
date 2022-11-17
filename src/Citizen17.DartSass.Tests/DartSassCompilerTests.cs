namespace Citizen17.DartSass.Tests;

[TestClass]
public class DartSassCompilerTests
{
    [TestMethod]
    public void CreateWithDefaultConstructorTest()
    {
        Assert.IsNotNull(new DartSassCompiler());
    }

    [TestMethod]
    public void CreateWithWrongPathTest()
    {
        Assert.IsNotNull(new DartSassCompiler("/not/real/path.exe"));
    }

    [TestMethod]
    public async Task CompileFileWithDefaultOptionsTest()
    {
        var compiler = new DartSassCompiler();

        var result = await compiler.CompileAsync("./TestSourceFiles/TestSheet.scss");

        Assert.IsNotNull(result.Code);
        Assert.IsTrue(!result.Debug.Any());
        Assert.IsTrue(!result.DeprecationWarnings.Any());
        Assert.IsTrue(!result.Warnings.Any());

        var expectedResult = """
            .my-class {
              color: red;
            }
            .my-class.active {
              color: green;
            }
            .my-class .text {
              text-align: right;
            }
            """;
        Assert.AreEqual(expectedResult, result.Code.Trim());
    }
}
