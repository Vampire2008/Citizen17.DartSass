using System.Text.RegularExpressions;

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

        [TestMethod]
        [DataRow(null)]
        [DataRow(StyleType.Expanded)]
        [DataRow(StyleType.Compressed)]
        public void StyleTypeTest(StyleType? value)
        {
            var testOptions = new SassCompileOptions();
            testOptions.StyleType = value;
            testOptions.SourceMapUrlType = SourceMapUrlType.Absolute;

            var result = testOptions.BuildArgs(false);
            switch (value)
            {
                case StyleType.Compressed:
                case StyleType.Expanded:
                    Assert.IsTrue(result.Contains($"-s {value.Value.ToString().ToLowerInvariant()}"));
                    break;
                case null:
                    var regEx = new Regex(@$"[^-]?-s ({StyleType.Expanded.ToString().ToLowerInvariant()}|{StyleType.Compressed.ToString().ToLowerInvariant()})");
                    Assert.IsFalse(regEx.IsMatch(result));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(true)]
        [DataRow(false)]
        public void EmitCharsetTest(bool? value)
        {
            var testOptions = new SassCompileOptions();
            testOptions.EmitCharset = value;

            var result = testOptions.BuildArgs(false);

            switch (value)
            {
                case true:
                    Assert.IsTrue(result.Contains("--charset"));
                    Assert.IsFalse(result.Contains("--no-charset"));
                    break;
                case false:
                    Assert.IsFalse(result.Contains("--charset"));
                    Assert.IsTrue(result.Contains("--no-charset"));
                    break;
                case null:
                    Assert.IsFalse(result.Contains("--charset"));
                    Assert.IsFalse(result.Contains("--no-charset"));
                    break;
            }
        }

        [TestMethod]
        [DataRow(false, false, false)]
        [DataRow(false, true, false)]
        [DataRow(true, false, true)]
        [DataRow(true, true, false)]
        public void UpdateTest(bool value, bool stdOut, bool expectedPresence)
        {
            var testOptions = new SassCompileOptions();
            testOptions.Update = value;

            var result = testOptions.BuildArgs(stdOut);

            Assert.AreEqual(expectedPresence, result.Contains("--update"));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(new[] { "C:\\path1" })]
        [DataRow(new[] { "C:\\path1", "C:\\path2" })]
        [DataRow(new[] { "C:\\parent1\\parent2\\path" })]
        [DataRow(new[] { "./relative" })]
        [DataRow(new[] { "../../base/relative" })]
        [DataRow(new[] { "/linux/path1", "/linux/path2" })]
        [DataRow(new[] { "/path with space" })]
        public void ImportPathsTest(string[] inputPath)
        {
            var testOptions = new SassCompileOptions();
            testOptions.ImportPaths = inputPath;

            var result = testOptions.BuildArgs(false);

            if (inputPath == null)
            {
                var regEx = new Regex(@"[^-]?-I ""[^""]*""");
                Assert.IsFalse(regEx.IsMatch(result));
            }
            else
            {
                foreach (var path in inputPath)
                {
                    Assert.IsTrue(result.Contains($"-I \"{path}\""));
                }
            }
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(true)]
        [DataRow(false)]
        public void QuietTest(bool? value)
        {
            var testOptions = new SassCompileOptions();
            testOptions.Quiet = value;

            var result = testOptions.BuildArgs(false);

            var regex = new Regex(@"[^-]?-q\s?$");
            switch (value)
            {
                case true:
                    Assert.IsTrue(regex.IsMatch(result));
                    Assert.IsFalse(result.Contains("--no-quiet"));
                    break;
                case false:
                    Assert.IsFalse(regex.IsMatch(result));
                    Assert.IsTrue(result.Contains("--no-quiet"));
                    break;
                case null:
                    Assert.IsFalse(regex.IsMatch(result));
                    Assert.IsFalse(result.Contains("--no-quiet"));
                    break;
            }
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(true)]
        [DataRow(false)]
        public void QuietDepsTest(bool? value)
        {
            var testOptions = new SassCompileOptions();
            testOptions.QuietDeps = value;

            var result = testOptions.BuildArgs(false);
            
            switch (value)
            {
                case true:
                    Assert.IsTrue(result.Contains("--quiet-deps"));
                    Assert.IsFalse(result.Contains("--no-quiet-deps"));
                    break;
                case false:
                    Assert.IsFalse(result.Contains("--quiet-deps"));
                    Assert.IsTrue(result.Contains("--no-quiet-deps"));
                    break;
                case null:
                    Assert.IsFalse(result.Contains("--quiet-deps"));
                    Assert.IsFalse(result.Contains("--no-quiet-deps"));
                    break;
            }
        }
    }
}