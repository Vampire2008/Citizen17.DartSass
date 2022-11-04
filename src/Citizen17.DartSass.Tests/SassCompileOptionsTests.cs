namespace Citizen17.DartSass.Tests
{
    [TestClass]
    public class SassCompileOptionsTests
    {
        [TestMethod]
        public void NoOptionsTest()
        {
            var testOptions = new SassCompileOptions();
            Assert.AreEqual(string.Empty, testOptions.BuildArgs(false));
        }

        [TestMethod]
        [DataRow(null, null, false, "")]
        [DataRow(true, null, false, "--source-map")]
        [DataRow(true, null, true, "")]
        [DataRow(true, true, false, "--source-map")]
        [DataRow(true, true, true, "--source-map")]
        [DataRow(true, false, false, "--source-map")]
        [DataRow(true, false, true, "")]
        public void GenerateSourceMapTest(bool? generateSourceMap, bool? embedSourceMap, bool useStdout, string expectedResult)
        {
            var testOptions = new SassCompileOptions();
            testOptions.GenerateSourceMap = generateSourceMap;
            testOptions.EmbedSourceMap = embedSourceMap;

            var result = testOptions.BuildArgs(useStdout);
            if (string.IsNullOrEmpty(expectedResult))
            {
                Assert.AreEqual(string.Empty, result);
            }
            else
            {
                Assert.IsTrue(testOptions.BuildArgs(useStdout).Contains(expectedResult));
            }
        }
    }
}