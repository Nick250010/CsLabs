
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Mainproject;

public class Position
{
    public int x;
    public int y;
}

public class SaveData
{
    public string Character { get; set; }
    public string Map { get; set; }
    public Position Position { get; set; }
    public int Health { get; set; }
    public List<string> Inventory { get; set; }
}

public class SaveEntry
{
    public string name { get; set; }
    public string file { get; set; }
    public DateTime updateAt { get; set; }
}

public class Program
{
    public static List<SaveEntry> LoadIndex(string indexPath)
    {
        if (File.Exists(indexPath))
        {
            string json = File.ReadAllText(indexPath);
            List<SaveEntry> entries =JsonConvert.DeserializeObject<List<SaveEntry>>(json);

            return entries;
        }

        Console.WriteLine("Файл не найден!");
        return null;
    }

    public static SaveData LoadSave(string savesDirectory, SaveEntry entry)
    {
        string path = savesDirectory + "/" + entry.file;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData save = JsonConvert.DeserializeObject<SaveData>(json);

            return save;
        }

        Console.WriteLine("Файл не найден!");
        return null;
    }

    public static void PrintSave(SaveEntry entry, SaveData save)
    {
        Console.WriteLine($"Вот данные по {entry.name} сохранению");

        Console.WriteLine(
            $"Имя персонажа: {save.Character}\n" +
            $"Карта: {save.Map}\n" +
            $"Координаты в формате(x,y): ({save.Position.x},{save.Position.y})\n");

        Console.Write($"Здоровье: {save.Health}\nИнвентарь: ");

        foreach (string item in save.Inventory)
        {
            Console.Write(item + " ");
        }

        Console.WriteLine("\n");
    }

    public static SaveEntry FindByName(List<SaveEntry> entries, string name)
    {
        foreach (SaveEntry entry in entries)
        {
            if (string.Equals(
                entry.name, name, StringComparison.OrdinalIgnoreCase))
            {
                return entry;
            }
        }

        Console.WriteLine("С таким именем ничего не нашлось");
        return null;
    }

    public static void SaveGame(
        string savesDirectory, SaveEntry entry, SaveData save)
    {
        string path = savesDirectory + "/" + entry.file;
        string json = JsonConvert.SerializeObject(save, Formatting.Indented);

        File.WriteAllText(path, json);
    }

    public static void SaveIndex(string indexPath, List<SaveEntry> entries)
    {
        string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
        File.WriteAllText(indexPath, json);
    }

    public static void CreateBackup(
        string savesDirectory, string backupsDirectory, SaveEntry entry)
    {
        string source = savesDirectory + "/" + entry.file;
        string destination = backupsDirectory + "/" + entry.file;

        if (!File.Exists(source))
        {
            Console.WriteLine("Файл сохранения не найден!");
            return;
        }

        Directory.CreateDirectory(backupsDirectory);
        File.Copy(source, destination, true);

        Console.WriteLine("Резервная копия создана!");
    }

    public static void RestoreBackup(
        string savesDirectory, string backupsDirectory, SaveEntry entry)
    {
        string backupPath = Path.Combine(backupsDirectory, entry.file);
        string savePath = Path.Combine(savesDirectory, entry.file);

        if (!File.Exists(backupPath))
        {
            Console.WriteLine($"Резервная копия не найдена: {backupPath}");
            return;
        }

        Directory.CreateDirectory(savesDirectory);
        File.Copy(backupPath, savePath, overwrite: true);

        Console.WriteLine($"Сохранение восстановлено: {entry.name}");
    }

    public static void DeleteSave(
        string savesDirectory,
        string indexPath,
        List<SaveEntry> entries,
        SaveEntry entry)
    {
        string savePath = Path.Combine(savesDirectory, entry.file);

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Console.WriteLine($"Файл сохранения удалён: {entry.name}");
        }
        else
        {
            Console.WriteLine($"Файл сохранения не найден: {savePath}");
        }

        entries.Remove(entry);
        SaveIndex(indexPath, entries);

        Console.WriteLine("Индекс сохранений обновлён.");
    }

    static void Main()
    {
        string indexPath = "index.json";
        string savesDirectory = "saves";
        string backupsDirectory = "backups";

        Directory.CreateDirectory(savesDirectory);
        Directory.CreateDirectory(backupsDirectory);

        List<SaveEntry> entries = LoadIndex(indexPath);

        if (entries == null)
        {
            Console.WriteLine(
                "Не удалось загрузить индекс. Программа завершена.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\n===== МЕНЕДЖЕР СОХРАНЕНИЙ =====");
            Console.WriteLine("1. Показать список сохранений");
            Console.WriteLine("2. Посмотреть данные сохранения");
            Console.WriteLine("3. Изменить данные персонажа");
            Console.WriteLine("4. Создать резервную копию");
            Console.WriteLine("5. Восстановить резервную копию");
            Console.WriteLine("6. Удалить сохранение");
            Console.WriteLine("7. Выйти");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (entries.Count == 0)
                    {
                        Console.WriteLine("Сохранений нет.");
                        break;
                    }

                    Console.WriteLine("\nСписок сохранений:");

                    foreach (SaveEntry entry in entries)
                    {
                        Console.WriteLine(
                            $"Название: {entry.name}, файл: {entry.file}, " +
                            $"дата изменения: {entry.updateAt:yyyy-MM-ddTHH:mm:ss}");
                    }

                    break;

                case "2":
                {
                    Console.Write("Введите имя сохранения: ");
                    string name = Console.ReadLine();

                    SaveEntry entry = FindByName(entries, name);

                    if (entry == null)
                        break;

                    SaveData save = LoadSave(savesDirectory, entry);

                    if (save != null)
                        PrintSave(entry, save);

                    break;
                }

                case "3":
                {
                    Console.Write("Введите имя сохранения: ");
                    string name = Console.ReadLine();

                    SaveEntry entry = FindByName(entries, name);

                    if (entry == null)
                        break;

                    SaveData save = LoadSave(savesDirectory, entry);

                    if (save == null)
                        break;

                    Console.WriteLine(
                        "Оставьте поле пустым, если не хотите его менять.");

                    Console.Write($"Новая карта (сейчас {save.Map}): ");
                    string map = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(map))
                        save.Map = map;

                    Console.Write(
                        $"Новая координата X (сейчас {save.Position.x}): ");
                    string xInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(xInput))
                    {
                        if (int.TryParse(xInput, out int x))
                            save.Position.x = x;
                        else
                            Console.WriteLine(
                                "Некорректная координата X. Оставлено прежнее значение.");
                    }

                    Console.Write(
                        $"Новая координата Y (сейчас {save.Position.y}): ");
                    string yInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(yInput))
                    {
                        if (int.TryParse(yInput, out int y))
                            save.Position.y = y;
                        else
                            Console.WriteLine(
                                "Некорректная координата Y. Оставлено прежнее значение.");
                    }

                    Console.Write($"Новое здоровье (сейчас {save.Health}): ");
                    string healthInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(healthInput))
                    {
                        if (int.TryParse(healthInput, out int health))
                            save.Health = health;
                        else
                            Console.WriteLine(
                                "Некорректное здоровье. Оставлено прежнее значение.");
                    }

                    Console.WriteLine(
                        "Введите предметы инвентаря через запятую.");
                    Console.WriteLine(
                        "Оставьте поле пустым, чтобы ничего не менять.");

                    Console.Write("Инвентарь: ");
                    string inventoryInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(inventoryInput))
                    {
                        save.Inventory = new List<string>();

                        foreach (string item in inventoryInput.Split(','))
                        {
                            if (item.Trim() != "")
                                save.Inventory.Add(item.Trim());
                        }
                    }

                    try
                    {
                        SaveGame(savesDirectory, entry, save);

                        entry.updateAt = DateTime.Now;
                        SaveIndex(indexPath, entries);

                        Console.WriteLine("Сохранение успешно изменено!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка записи: {ex.Message}");
                    }

                    break;
                }

                case "4":
                {
                    Console.Write("Введите имя сохранения: ");
                    string name = Console.ReadLine();

                    SaveEntry entry = FindByName(entries, name);

                    if (entry != null)
                        CreateBackup(savesDirectory, backupsDirectory, entry);

                    break;
                }

                case "5":
                {
                    Console.Write("Введите имя сохранения: ");
                    string name = Console.ReadLine();

                    SaveEntry entry = FindByName(entries, name);

                    if (entry != null)
                    {
                        RestoreBackup(savesDirectory, backupsDirectory, entry);

                        entry.updateAt = DateTime.Now;
                        SaveIndex(indexPath, entries);
                    }

                    break;
                }

                case "6":
                {
                    Console.Write("Введите имя сохранения: ");
                    string name = Console.ReadLine();

                    SaveEntry entry = FindByName(entries, name);

                    if (entry != null)
                    {
                        Console.Write(
                            $"Удалить сохранение \"{entry.name}\"? (да/нет): ");

                        string confirm = Console.ReadLine();

                        if (confirm.Equals(
                            "да", StringComparison.OrdinalIgnoreCase))
                        {
                            DeleteSave(
                                savesDirectory, indexPath, entries, entry);
                        }
                        else
                        {
                            Console.WriteLine("Удаление отменено.");
                        }
                    }

                    break;
                }

                case "7":
                    Console.WriteLine("Выход из программы.");
                    return;

                default:
                    Console.WriteLine(
                        "Неверный пункт меню. Попробуйте ещё раз.");
                    break;
            }
        }
    }
}
