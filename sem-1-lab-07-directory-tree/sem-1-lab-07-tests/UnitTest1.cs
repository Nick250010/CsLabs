using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
[NonParallelizable]
public class ProgramTests
{
    private string _originalDirectory = null!;
    private string _testDirectory = null!;
    private TextWriter _originalOutput = null!;
    private TextReader _originalInput = null!;

    [SetUp]
    public void SetUp()
    {
        _originalDirectory = Directory.GetCurrentDirectory();
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Directory.CreateDirectory(_testDirectory);
        Directory.SetCurrentDirectory(_testDirectory);

        _originalOutput = Console.Out;
        _originalInput = Console.In;
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(_originalOutput);
        Console.SetIn(_originalInput);
        Directory.SetCurrentDirectory(_originalDirectory);
        Directory.Delete(_testDirectory, true);
    }

    [Test]
    public void BuildTree_PrintsRootWithChildCount()
    {
        File.WriteAllLines("tree.txt", new[]
        {
            "A",
            "BEGIN B",
            "END",
            "BEGIN C",
            "END"
        });

        var output = RunBuildTree();

        Assert.That(output, Does.Contain("A (2)"));
        Assert.That(output, Does.Contain("├─B (0)"));
        Assert.That(output, Does.Contain("├─C (0)"));
    }

    [Test]
    public void BuildTree_PrintsNestedChildren()
    {
        File.WriteAllLines("tree.txt", new[]
        {
            "A",
            "BEGIN B",
            "BEGIN C",
            "END",
            "END"
        });

        var output = RunBuildTree();

        Assert.That(output, Does.Contain("A (1)"));
        Assert.That(output, Does.Contain("├─B (1)"));
        Assert.That(output, Does.Contain("│ ├─C (0)"));
    }

    [Test]
    public void BuildTree_IgnoresEmptyLines()
    {
        File.WriteAllLines("tree.txt", new[]
        {
            "A",
            "",
            "BEGIN B",
            "",
            "END"
        });

        var output = RunBuildTree();

        Assert.That(output, Does.Contain("A (1)"));
        Assert.That(output, Does.Contain("├─B (0)"));
    }

    [Test]
    public void BuildTree_PrintsZeroChildrenForLeaf()
    {
        File.WriteAllLines("tree.txt", new[]
        {
            "A",
            "BEGIN B",
            "END"
        });

        var output = RunBuildTree();

        Assert.That(output, Does.Contain("B (0)"));
    }

    [Test]
    public void BuildTree_HandlesEmptyFile()
    {
        File.WriteAllText("tree.txt", "");

        var output = RunBuildTree();

        Assert.That(output, Does.Contain("Tree file is empty."));
    }

    private static string RunBuildTree()
    {
        typeof(Program).GetField("_lineIndex", BindingFlags.NonPublic | BindingFlags.Static)!
            .SetValue(null, 0);

        typeof(Program).GetField("_depth", BindingFlags.NonPublic | BindingFlags.Static)!
            .SetValue(null, 0);

        using var writer = new StringWriter();

        Console.SetOut(writer);
        Console.SetIn(new StringReader(""));

        var method = typeof(Program).GetMethod(
            "BuildTree",
            BindingFlags.NonPublic | BindingFlags.Static)!;

        method.Invoke(null, new object[] { "tree.txt" });

        return writer.ToString();
    }
}