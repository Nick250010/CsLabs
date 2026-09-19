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

    [Test]
    public void IsEven_EvenNumber_ReturnsTrue()
    {
        var result = Program.IsEven(10);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEven_OddNumber_ReturnsFalse()
    {
        var result = Program.IsEven(11);

        Assert.That(result, Is.False);
    }
}