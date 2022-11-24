namespace Citizen17.DartSass.Tests;

internal struct TestFileKey : IEquatable<TestFileKey>
{
    internal string FileName { get; }
    internal bool SourceMap { get; }

    public TestFileKey(string fileName, bool sourceMap)
    {
        FileName = fileName;
        SourceMap = sourceMap;
    }

    public bool Equals(TestFileKey other)
    {
        return FileName == other.FileName && SourceMap == other.SourceMap;
    }

    public override bool Equals(object? obj)
    {
        return obj is TestFileKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FileName, SourceMap);
    }
}