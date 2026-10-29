using System;
using System.IO;

public class Program
{
    // Current line in file
    private static int _lineIndex = 0;

    // Current nesting level
    private static int _depth=0; //была не до конца инициализирована что давала не правдивый вывод глубины

    // File content
    private static string[] _lines = null!;

    private static void Main()
    {
        BuildTree("tree.txt");
    }

    private static void BuildTree(string fileName)
    {
        _lines = File.ReadAllLines(fileName);

        if (_lines.Length == 0)
        {
            Console.WriteLine("Tree file is empty.");
            return;
        }
        var child = CountChildren(_lineIndex);
        Console.WriteLine(_lines[_lineIndex].Trim() + $" ({child})");

        _lineIndex = 1;
        PrintTree();

        Console.ReadLine();
    }

    private static void PrintTree()
    {
        while (_lineIndex < _lines.Length)
        {
            var line = _lines[_lineIndex].Trim();

            if (line == "")
            {
                _lineIndex++;
                continue;
            }

            if (line == "END")
            {
                _lineIndex++;
                return;
            }

            if (line.StartsWith("BEGIN "))
            {
                var name = line.Substring(6).Trim();
                var child = CountChildren(_lineIndex);

                Console.Write(GetIndent());
                Console.WriteLine($"├─{name} ({child})");

                _lineIndex++;
                _depth++;

                PrintTree();

                _depth--;
                continue;
            }

            throw new FormatException($"Invalid line: {line}");
        }
    }
    

    private static string GetIndent()
    {
        var result = "";

        for (var i = 1; i <= _depth; i++)
        {
            result += "│ ";
        }

        return result;
    }

    private static int CountChildren(int index)
    {
        int count = 0,depth=0;
        for(int i = index+1; i < _lines.Length; i++)
        {
            var line = _lines[i].Trim();
            if(line.StartsWith("BEGIN "))
            {
                if(depth == 0)
                {
                    count++;
                }
                depth++;
            }else if(line == "END")
            {
                if(depth == 0)
                {
                    return count;
                }
                depth--;
            }
        }
        return count;

    }
}