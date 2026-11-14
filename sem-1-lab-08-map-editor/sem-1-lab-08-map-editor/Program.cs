
using System;
using System.IO;
using System.Collections.Generic;

namespace MainProject;

public struct Position
{
    public int X;
    public int Y;

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public struct Cell
{
    public char Type;

    public void Show()
    {
        Console.Write(Type);
    }
}

public struct Player
{
    public Position Position;
}

public class MapSnapshot
{
    public Cell[,] Cells { get; private set; }
    public Position PlayerPosition { get; private set; }
    public bool HasPlayer { get; private set; }

    public MapSnapshot(Cell[,] cells, Position playerPosition, bool hasPlayer)
    {
        Cells = cells;
        PlayerPosition = playerPosition;
        HasPlayer = hasPlayer;
    }
}

public class Map
{
    public int height;
    public int width;

    public Cell[,] kletki;
    public Player Player;
    public bool hasPlayer;

    private List<MapSnapshot> snapshots = new List<MapSnapshot>();

    public Map(int width, int height)
    {
        this.width = width;
        this.height = height;
        kletki = new Cell[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                kletki[i, j].Type = '.';
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (hasPlayer && Player.Position.X == j && Player.Position.Y == i)
                {
                    Console.Write('P');
                }
                else
                {
                    kletki[i, j].Show();
                }
            }

            Console.WriteLine();
        }
    }

    public void SetCell(int x, int y, char type)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Console.WriteLine("Некорректные координаты.");
            return;
        }

        kletki[y, x].Type = type;
    }

    public void PlacePlayer(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Console.WriteLine("Некорректные координаты.");
            return;
        }

        Player.Position = new Position(x, y);
        hasPlayer = true;
    }

    public void MovePlayer(Position position)
    {
        MovePlayer(position.X, position.Y);
    }

    public void MovePlayer(int x, int y)
    {
        if (!hasPlayer)
        {
            Console.WriteLine("Игрок ещё не размещён.");
            return;
        }

        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Console.WriteLine("Некорректные координаты.");
            return;
        }

        Player.Position = new Position(x, y);
    }

    public MapSnapshot CreateSnapshot()
    {
        Cell[,] cells = new Cell[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cells[i, j] = kletki[i, j];
            }
        }

        MapSnapshot snapshot = new MapSnapshot(cells, Player.Position, hasPlayer);

        snapshots.Add(snapshot);

        Console.WriteLine($"Контрольная точка №{snapshots.Count - 1} создана.");

        return snapshot;
    }

    public void Restore(MapSnapshot snapshot)
    {
        if (snapshot == null ||
            snapshot.Cells.GetLength(0) != height ||
            snapshot.Cells.GetLength(1) != width)
        {
            Console.WriteLine("Некорректный снимок.");
            return;
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                kletki[i, j] = snapshot.Cells[i, j];
            }
        }

        Player.Position = snapshot.PlayerPosition;
        hasPlayer = snapshot.HasPlayer;
    }

    public void DisplaySnapshots()
    {
        if (snapshots.Count == 0)
        {
            Console.WriteLine("Контрольные точки отсутствуют.");
            return;
        }

        Console.WriteLine("Список контрольных точек:");

        for (int i = 0; i < snapshots.Count; i++)
        {
            Console.WriteLine($"{i} - Контрольная точка №{i}");
        }
    }

    public void RestoreSnapshot(int index)
    {
        if (index < 0 || index >= snapshots.Count)
        {
            Console.WriteLine("Такой контрольной точки не существует.");
            return;
        }

        Restore(snapshots[index]);
        Console.WriteLine("Карта восстановлена.");
    }

    public void SaveToFile(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine($"{width}|{height}");

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    writer.Write(kletki[i, j].Type);
                }

                writer.WriteLine();
            }
        }
    }

    public static Map Load(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);
        string[] dimensions = lines[0].Split('|');

        int width = Convert.ToInt32(dimensions[0]);
        int height = Convert.ToInt32(dimensions[1]);

        if (lines.Length != height + 1)
        {
            throw new FormatException("Некорректная высота карты.");
        }

        Map map = new Map(width, height);

        for (int i = 0; i < height; i++)
        {
            if (lines[i + 1].Length != width)
            {
                throw new FormatException("Некорректная ширина карты.");
            }

            for (int j = 0; j < width; j++)
            {
                map.kletki[i, j].Type = lines[i + 1][j];
            }
        }

        return map;
    }
}

public class Program
{
    public static void Main()
    {
        Map map = Map.Load("map.txt");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1 - Показать карту");
            Console.WriteLine("2 - Изменить клетку");
            Console.WriteLine("3 - Разместить игрока");
            Console.WriteLine("4 - Переместить игрока");
            Console.WriteLine("5 - Создать контрольную точку");
            Console.WriteLine("6 - Показать контрольные точки");
            Console.WriteLine("7 - Восстановить контрольную точку");
            Console.WriteLine("8 - Сохранить карту");
            Console.WriteLine("9 - Создать новую карту");
            Console.WriteLine("0 - Выход");
            Console.Write("Выберите команду: ");

            string command = Console.ReadLine();

            switch (command)
            {
                case "1":
                {
                    map.Display();
                    break;
                }

                case "2":
                {
                    Console.Write("Координата X: ");
                    int cellX = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Координата Y: ");
                    int cellY = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Новый тип клетки: ");
                    char type = Convert.ToChar(Console.ReadLine());

                    map.SetCell(cellX, cellY, type);
                    break;
                }

                case "3":
                {
                    Console.Write("Координата X игрока: ");
                    int playerX = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Координата Y игрока: ");
                    int playerY = Convert.ToInt32(Console.ReadLine());

                    map.PlacePlayer(playerX, playerY);
                    break;
                }

                case "4":
                {
                    Console.Write("Новая координата X: ");
                    int moveX = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Новая координата Y: ");
                    int moveY = Convert.ToInt32(Console.ReadLine());

                    map.MovePlayer(moveX, moveY);
                    break;
                }

                case "5":
                {
                    map.CreateSnapshot();
                    break;
                }

                case "6":
                {
                    map.DisplaySnapshots();
                    break;
                }

                case "7":
                {
                    map.DisplaySnapshots();

                    Console.Write("Введите номер контрольной точки: ");
                    int index = Convert.ToInt32(Console.ReadLine());

                    map.RestoreSnapshot(index);
                    break;
                }

                case "8":
                {
                    map.SaveToFile("map.txt");
                    Console.WriteLine("Карта сохранена.");
                    break;
                }

                case "9":
                {
                    Console.Write("Ширина: ");
                    int newWidth = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Высота: ");
                    int newHeight = Convert.ToInt32(Console.ReadLine());

                    map = new Map(newWidth, newHeight);

                    Console.WriteLine("Новая карта создана.");
                    break;
                }

                case "0":
                {
                    return;
                }

                default:
                {
                    Console.WriteLine("Неизвестная команда.");
                    break;
                }
            }
        }
    }
}
