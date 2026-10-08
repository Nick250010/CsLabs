
namespace MainProject;

public class Program
{
    
    public struct LogEntry()
    {
    public DateTime Timestamp;
    public string Level = "";
    public string Category = "";
    public string Message = "";
    }

    public static List<LogEntry> ParseLog(string[] lines){
        List<LogEntry> logEntries = new List<LogEntry>();
        //2026-09-01 12:23:10.775 [Warning][User] User MiraStone health dropped below 10%
        foreach( string x in lines)
        {
            LogEntry log;
            // время
            int index1 = x.IndexOf(' ');
            int index2 = x.IndexOf(' ', index1+1);
            string datetimeStr = x.Substring(0, index2);
            DateTime dateTime = DateTime.Parse(datetimeStr);
            log.Timestamp = dateTime;

            //level
            index1 = x.IndexOf('[');
            index2 = x.IndexOf(']');
            string level = x.Substring(index1+1,index2-index1);
            log.Level = level;

            //Category
            index1 = x.IndexOf('[',index1+1);
            index2 = x.IndexOf(']',index2+1);
            string category = x.Substring(index1+1,index2-index1);
            log.Category = category;

            //Message
            index1 = index2;
            string message = x.Substring(index2+1,x.Length - index2+1);
            log.Message = message;
            logEntries.Add(log);
        }
        return  logEntries;
    }

    public static List<LogEntry> FilterByDate(List<LogEntry> entries, DateTime date)
    {
        List<LogEntry> dataset = new List<LogEntry>();
        foreach(LogEntry x in entries)
        {
            if(x.Timestamp == date)
            {
                dataset.Add(x);
            }
        }
        return dataset;
        
    }

    public static List<LogEntry> FilterByLevel(List<LogEntry> entries, string level)
    {
        List<LogEntry> lvl = new List<LogEntry>();
        foreach(LogEntry x in entries)
        {
            if(x.Level == level)
            {
                lvl.Add(x);
            }
        }
        return lvl;
    }

    public static List<LogEntry> FilterByCategory(List<LogEntry> entries, string category)
    {
        List<LogEntry> cater = new List<LogEntry>();
        foreach(LogEntry x in entries)
        {
            if(x.Category == category)
            {
                cater.Add(x);
            }
        }
        return cater;
    }

    public static List<LogEntry> Search(List<LogEntry> entries, string text)
    {
        List<LogEntry> txt = new List<LogEntry>();
        foreach(LogEntry x in entries)
        {
            if(x.Message == text)
            {
                txt.Add(x);
            }
        }
        return txt;
    }

    public static int CountByLevel(List<LogEntry> entries, string level)
    {
        List<LogEntry> lvl = new List<LogEntry>();
        foreach(LogEntry x in entries)
        {
            if(x.Level == level)
            {
                lvl.Add(x);
            }
        }
        return lvl.Count;
    }

    public static string GetServerStatus(List<LogEntry> entries)
    {
    foreach (LogEntry x in entries)
    {
        if (x.Level == "Fatal" && x.Category == "Server")
        {
            return "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";
        }
    }

    foreach (LogEntry x in entries)
    {
        if (x.Level == "Error")
        {
            return "Есть ошибки: требуется проверка";
        }
    }

    return "Сервер работает штатно";
    }


    public static void Main()
    {
        string[] lines = File.ReadAllLines("event_server.log");
        //2026-09-01 12:23:10.775 [Warning][User] User MiraStone health dropped below 10%
        List<LogEntry> logEntries = ParseLog(lines);
    }
}