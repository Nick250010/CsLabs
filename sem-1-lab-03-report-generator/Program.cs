
using System;

class Program
{

    static string GetValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Не указано";

        return value.Trim();
    }

    static string CreateShortReport(
        string eventName,
        DateTime eventDate,
        int participants,
        string winner)
    {
        return $"**Игровое событие завершено!**\n" +
               $"Название: {GetValue(eventName)}\n" +
               $"Дата: {eventDate:dd.MM.yyyy}\n" +
               $"Участников: {participants}\n" +
               $"Победитель: {GetValue(winner)}";
    }

    static string CreateDetailedReport(
        string eventName,
        DateTime eventDate,
        int participants,
        string winner,
        TimeSpan duration,
        string prize,
        string comment)
    {
        string result =
            $"**ОТЧЁТ ОБ ИГРОВОМ СОБЫТИИ**\n" +
            $"━━━━━━━━━━━━━━━━━━━━\n" +
            $"Название: {GetValue(eventName)}\n" +
            $"Дата проведения: {eventDate:dd.MM.yyyy}\n" +
            $"Количество участников: {participants}\n" +
            $"Победитель: {GetValue(winner)}\n" +
            $"Продолжительность: {duration.Hours} ч. {duration.Minutes} мин.\n" +
            $"Призовой фонд: {GetValue(prize)}\n";

        if (!string.IsNullOrWhiteSpace(comment))
        {
            result += $"Комментарий: {comment.Trim()}\n";
        }

        result += $"━━━━━━━━━━━━━━━━━━━━\n" +
                  $"Спасибо всем за участие!";

        return result;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Генератор отчётов для Discord ===");
        Console.WriteLine();

        Console.Write("Название события: ");
        string eventName = Console.ReadLine();

        Console.Write("Количество участников: ");
        int participants = int.Parse(Console.ReadLine());

        Console.Write("Победитель (можно оставить пустым): ");
        string winner = Console.ReadLine();

        Console.Write("Призовой фонд (можно оставить пустым): ");
        string prize = Console.ReadLine();

        Console.Write("Комментарий (можно оставить пустым): ");
        string comment = Console.ReadLine();

        Console.Write("Продолжительность в минутах: ");
        int durationMinutes = int.Parse(Console.ReadLine());

        Console.WriteLine();
        Console.WriteLine("Выберите шаблон:");
        Console.WriteLine("1 — Краткий");
        Console.WriteLine("2 — Подробный");
        Console.Write("Ваш выбор: ");

        string template = Console.ReadLine();
        DateTime eventDate = DateTime.Now;

        TimeSpan duration = TimeSpan.FromMinutes(durationMinutes);

        string report;

        if (template == "1")
        {
            report = CreateShortReport(
                eventName,
                eventDate,
                participants,
                winner);
        }
        else
        {
            report = CreateDetailedReport(
                eventName,
                eventDate,
                participants,
                winner,
                duration,
                prize,
                comment);
        }

        Console.WriteLine();
        Console.WriteLine("=== Готовый отчёт ===");
        Console.WriteLine();
        Console.WriteLine(report);
    }
}