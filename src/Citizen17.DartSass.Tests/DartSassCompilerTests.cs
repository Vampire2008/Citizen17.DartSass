﻿namespace Citizen17.DartSass.Tests;

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
        var exception = Assert.ThrowsException<ArgumentException>(() => new DartSassCompiler("/not/real/path.exe"));
        Assert.AreEqual(Messages.ErrorSassNotFound, exception.Message);
    }

    [TestMethod]
    public void CreateWithWrongPathAndUseFallbackFalseTest()
    {
        var exception = Assert.ThrowsException<ArgumentException>(() => new DartSassCompiler("/not/real/path.exe", false));
        Assert.AreEqual(Messages.ErrorSassNotFound, exception.Message);
    }

    [TestMethod]
    public void CreateWithWrongPathAndUseFallbackTest()
    {
        Assert.IsNotNull(new DartSassCompiler("/not/real/path.exe", true));
    }

    [TestMethod]
    public async Task CompileFileWithDefaultOptionsTest()
    {
        var compiler = new DartSassCompiler();

        var file = new TestFileKey(ScssTestFiles.TestSheetScss, false);

        var result = await compiler.CompileAsync(file.FileName);

        Assert.IsNotNull(result.Code);
        Assert.IsFalse(result.Debug.Any());
        Assert.IsFalse(result.DeprecationWarnings.Any());
        Assert.IsFalse(result.Warnings.Any());
        Assert.AreEqual(ScssTestFiles.ExpectedResults[file], result.Code.Trim());
    }

    [TestMethod]
    public async Task CompileNotExistFileTest()
    {
        var compiler = new DartSassCompiler();

        await Assert.ThrowsExceptionAsync<FileNotFoundException>(() => compiler.CompileAsync("./not/exist/file"));
    }

    [TestMethod]
    public async Task CompileNullFileTest()
    {
        var compiler = new DartSassCompiler();

        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => compiler.CompileAsync(null));
    }

    [TestMethod]
    public async Task CompileEmptyNameFileTest()
    {
        var compiler = new DartSassCompiler();

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => compiler.CompileAsync(""));
    }

    [TestMethod]
    public async Task CompileFileWithEmptyOptionsTest()
    {
        var compiler = new DartSassCompiler();

        var file = new TestFileKey(ScssTestFiles.TestSheetScss, false);

        var result = await compiler.CompileAsync(file.FileName, new());

        Assert.IsNotNull(result.Code);
        Assert.IsFalse(result.Debug.Any());
        Assert.IsFalse(result.DeprecationWarnings.Any());
        Assert.IsFalse(result.Warnings.Any());
        
        Assert.AreEqual(ScssTestFiles.ExpectedResults[file], result.Code.Trim());
    }

    [TestMethod]
    public async Task CompileFileWithNullOptionsTest()
    {
        var compiler = new DartSassCompiler();

        var file = new TestFileKey(ScssTestFiles.TestSheetScss, false);

        var result = await compiler.CompileAsync(file.FileName, null);

        Assert.IsNotNull(result.Code);
        Assert.IsFalse(result.Debug.Any());
        Assert.IsFalse(result.DeprecationWarnings.Any());
        Assert.IsFalse(result.Warnings.Any());
        Assert.AreEqual(ScssTestFiles.ExpectedResults[file], result.Code.Trim());
    }

    [TestMethod]
    [DataRow(null, null, false)]
    [DataRow(null, true, true)]
    [DataRow(null, false, false)]
    
    [DataRow(true, true, true)]
    
    [DataRow(false, null, false)]
    public async Task CompileWithGenerateSourceMapTest(bool? generateSourceMap, bool? embedSourceMap,  bool expectedPresence)
    {
        var testOptions = new SassCompileOptions();
        testOptions.GenerateSourceMap = generateSourceMap;
        testOptions.EmbedSourceMap = embedSourceMap;

        var compiler = new DartSassCompiler();
        var file = new TestFileKey(ScssTestFiles.TestSheetScss, expectedPresence);
        var result = await compiler.CompileAsync(file.FileName, testOptions);
        
        Assert.IsNotNull(result.Code);
        Assert.IsFalse(result.Debug.Any());
        Assert.IsFalse(result.DeprecationWarnings.Any());
        Assert.IsFalse(result.Warnings.Any());

        Assert.AreEqual(ScssTestFiles.ExpectedResults[file], result.Code.Trim());
    }

    [TestMethod]
    [DataRow(true, null)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(false, false)]
    public async Task CompileWithGenerateSourceMapWithIncompatibleParametersTest(bool? generateSourceMap, bool? embedSourceMap)
    {
        var testOptions = new SassCompileOptions();
        testOptions.GenerateSourceMap = generateSourceMap;
        testOptions.EmbedSourceMap = embedSourceMap;

        var compiler = new DartSassCompiler();
        var file = new TestFileKey(ScssTestFiles.TestSheetScss, false);
        var exception = await Assert.ThrowsExceptionAsync<SassCompileException>(() => compiler.CompileAsync(file.FileName, testOptions));
        
        Assert.IsTrue(exception.Errors.Count() == 1);
        Assert.IsFalse(exception.Debug.Any());
        Assert.IsFalse(exception.DeprecationWarnings.Any());
        Assert.IsFalse(exception.Warnings.Any());
    }

    //public async Task CompileWithChan
}
