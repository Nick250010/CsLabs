using MainProject;

namespace ProgramTests;

public class Tests
{
    [Test]
    public void Add_TwoNumbers_ReturnsSum()
    {
        var result = Program.Add(2, 3);

        Assert.That(result, Is.EqualTo(5));
    }
}