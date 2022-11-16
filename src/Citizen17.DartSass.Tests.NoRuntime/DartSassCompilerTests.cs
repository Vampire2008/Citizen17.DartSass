namespace Citizen17.DartSass.Tests.NoRuntime
{
    [TestClass]
    public class DartSassCompilerTests
    {
        [TestMethod]
        public void CreateDartSassCompilerWithNoRuntime()
        {
            Assert.ThrowsException<ArgumentException>(() => new DartSassCompiler(), "Runtime presents on computer in PATH");
        }
    }
}