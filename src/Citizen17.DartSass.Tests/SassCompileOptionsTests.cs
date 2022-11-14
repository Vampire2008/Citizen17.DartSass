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
        [DataRow(null, null, false, null)]
        [DataRow(null, null, false, null)]
        [DataRow(null, null, true, null)]
        [DataRow(null, true, false, null)]
        [DataRow(null, true, true, null)]
        [DataRow(null, false, false, null)]
        [DataRow(null, false, true, null)]

        [DataRow(true, null, false, true)]
        [DataRow(true, null, true, null)]
        [DataRow(true, true, false, true)]
        [DataRow(true, true, true, true)]
        [DataRow(true, false, false, true)]
        [DataRow(true, false, true, null)]

        [DataRow(false, null, false, false)]
        [DataRow(false, null, true, false)]
        [DataRow(false, true, false, false)]
        [DataRow(false, true, true, false)]
        [DataRow(false, false, false, false)]
        [DataRow(false, false, true, false)]
        public void GenerateSourceMapTest(bool? generateSourceMap, bool? embedSourceMap, bool useStdout, bool? expectedPresence)
        {
            var testOptions = new SassCompileOptions();
            testOptions.GenerateSourceMap = generateSourceMap;
            testOptions.EmbedSourceMap = embedSourceMap;

            var result = testOptions.BuildArgs(useStdout);
            switch (expectedPresence)
            {
                case true:
                    Assert.IsTrue(result.Contains("--source-map"), "Result doesn't contains activate parameter");
                    Assert.IsFalse(result.Contains("--no-source-map"), "Result contains deactivate parameter");
                    break;
                case false:
                    Assert.IsFalse(result.Contains("--source-map"), "Result contains activate parameter");
                    Assert.IsTrue(result.Contains("--no-source-map"), "Result doesn't contains deactivate parameter");
                    break;
                case null:
                    Assert.IsFalse(result.Contains("--source-map"), "Result contains activate parameter");
                    Assert.IsFalse(result.Contains("--no-source-map"), "Result contains deactivate parameter");
                    break;
            }
        }

        [TestMethod]
        [DataRow(null, false)]
        [DataRow(SourceMapUrlType.Absolute, true)]
        [DataRow(SourceMapUrlType.Relative, true)]
        public void SourceMapUrlTypeTest(SourceMapUrlType? value, bool expectedPresence)
        {
            var testOptions = new SassCompileOptions();
            testOptions.SourceMapUrlType = value;

            var result = testOptions.BuildArgs(false);

            if (expectedPresence)
            {
                Assert.IsTrue(result.Contains($"--source-map-urls {value?.ToString().ToLowerInvariant()}"));
            }
            else
            {
                Assert.IsFalse(result.Contains("--source-map-urls"));
            }
        }

        [TestMethod]
        [DataRow(null, null, false, null)]
        [DataRow(null, null, false, null)]
        [DataRow(null, null, true, null)]
        [DataRow(null, true, false, null)]
        [DataRow(null, true, true, null)]
        [DataRow(null, false, false, null)]
        [DataRow(null, false, true, null)]

        [DataRow(true, null, false, true)]
        [DataRow(true, null, true, null)]
        [DataRow(true, true, false, true)]
        [DataRow(true, true, true, true)]
        [DataRow(true, false, false, true)]
        [DataRow(true, false, true, null)]

        [DataRow(false, null, false, false)]
        [DataRow(false, null, true, false)]
        [DataRow(false, true, false, false)]
        [DataRow(false, true, true, false)]
        [DataRow(false, false, false, false)]
        [DataRow(false, false, true, false)]
        public void EmbedSourcesTest(bool? embedSources, bool? embedSourceMap, bool useStdout, bool? expectedPresence)
        {
            var testOptions = new SassCompileOptions();
            testOptions.EmbedSources = embedSources;
            testOptions.EmbedSourceMap = embedSourceMap;

            var result = testOptions.BuildArgs(useStdout);
            switch (expectedPresence)
            {
                case true:
                    Assert.IsTrue(result.Contains("--embed-sources"), "Result doesn't contains activate parameter");
                    Assert.IsFalse(result.Contains("--no-embed-sources"), "Result contains deactivate parameter");
                    break;
                case false:
                    Assert.IsFalse(result.Contains("--embed-sources"), "Result contains activate parameter");
                    Assert.IsTrue(result.Contains("--no-embed-sources"), "Result doesn't contains deactivate parameter");
                    break;
                case null:
                    Assert.IsFalse(result.Contains("--embed-sources"), "Result contains activate parameter");
                    Assert.IsFalse(result.Contains("--no-embed-sources"), "Result contains deactivate parameter");
                    break;
            }
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow(true, true)]
        [DataRow(false, false)]
        public void EmbedSourceMapTest(bool? embedSourceMap, bool? expectedPresence)
        {
            var testOptions = new SassCompileOptions();
            testOptions.EmbedSourceMap = embedSourceMap;

            var result = testOptions.BuildArgs(false);
            switch (expectedPresence)
            {
                case true:
                    Assert.IsTrue(result.Contains("--embed-source-map"), "Result doesn't contains activate parameter");
                    Assert.IsFalse(result.Contains("--no-embed-source-map"), "Result contains deactivate parameter");
                    break;
                case false:
                    Assert.IsFalse(result.Contains("--embed-source-map"), "Result contains activate parameter");
                    Assert.IsTrue(result.Contains("--no-embed-source-map"), "Result doesn't contains deactivate parameter");
                    break;
                case null:
                    Assert.IsFalse(result.Contains("--embed-source-map"), "Result contains activate parameter");
                    Assert.IsFalse(result.Contains("--no-embed-source-map"), "Result contains deactivate parameter");
                    break;
            }
        }
    }
}