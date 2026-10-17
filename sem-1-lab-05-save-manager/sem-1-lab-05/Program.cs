
using System;
using System.ComponentModel;
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
        public string name {get; set; }
        public string file{get; set;}
        public DateTime updateAt {get; set;}
    }

public class Program
{
    
    public static List<SaveEntry> LoadIndex(string indexPath)
    {
        if (File.Exists(indexPath))
        {
            string json = File.ReadAllText(indexPath);
            List<SaveEntry> entries = JsonConvert.DeserializeObject<List<SaveEntry>>(json);
            return entries;
        }
        else
        {
            Console.WriteLine("Файл не найден!");
            return null;
        }
    }

    public static SaveData LoadSave(string savesDirectory, SaveEntry entry)
    {
        string path = savesDirectory+"/"+entry.file;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData save = JsonConvert.DeserializeObject<SaveData>(json);
            return save;
        }
        else
        {
            Console.WriteLine("Файл не найден!");
            return null;
        }
    }

    public static void PrintSave(SaveEntry entry, SaveData save)
    {
        Console.WriteLine($"Вот данные по {entry.name} сохранению");
        Console.WriteLine($"Имя персонажа: {save.Character}\nКарта: {save.Map}\nКоординаты в формате(x,y): ({save.Position.x},{save.Position.y})\n");
        Console.Write($"Здоровье: {save.Health}\nИнвентарь: ");
        foreach(string x in save.Inventory)
        {
            Console.Write(x+" ");
        }
        Console.WriteLine("\n");
    }

    public static SaveEntry FindByName(List<SaveEntry> entries, string name){
        foreach(SaveEntry x in entries){
            if(string.Equals(x.name, name, StringComparison.OrdinalIgnoreCase)){
                return x;
            }
        }
        Console.WriteLine("С таким именем ничего не нашлось");
        return null;
    }

    public static void SaveGame(string savesDirectory, SaveEntry entry, SaveData save){
        string path = savesDirectory+"/"+entry.file;
        string json =  JsonConvert.SerializeObject(save,Formatting.Indented);
        File.WriteAllText(path,json);
    }

    public static void SaveIndex(string indexPath, List<SaveEntry> entries){
    string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
    File.WriteAllText(indexPath, json);
    }


    public static void CreateBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
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



public static void RestoreBackup(string savesDirectory,string backupsDirectory,SaveEntry entry)
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

public static void DeleteSave(string savesDirectory,string indexPath,List<SaveEntry> entries,SaveEntry entry)
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
    Console.WriteLine($"Индекс сохранений обновлён.");
}
    static void Main(){
        //SaveData save = JsonConvert.DeserializeObject<SaveData>(json);
        // 1. Читаем файл как обычный текст
        string json = File.ReadAllText("index.json");

        

    }
}