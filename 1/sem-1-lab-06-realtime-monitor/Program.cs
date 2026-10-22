using System;

internal class Program
{
    public static double[] CpuHistory = new double[20];
    public static double[] ramHistory = new double[20];
    public static double[] temperatureHistory = new double[20];
    public static int i = 0;

    public static void clean(int strok)
    {
        Console.SetCursorPosition(0, strok);
        Console.Write(new string(' ', 34));
        Console.SetCursorPosition(0, strok);
    }

    public static void Grafik(double index,int strok,string info)
    {
        clean(strok);
        string Graph;
        double kol = index/5;
        if (index % 5 == 0.0)
        {
            
            Graph = new string('#',(int)kol) + new string('.',20-(int)kol);
        }
        else
        {
            Graph = new string('#',(int)kol+1) + new string('.',19-(int)kol);
        }
        Console.Write(info+Graph);
    }


    public static void Table(double Nowcpu, double Nowram, double Nowtemperature)
    {
        clean(4);

        Console.Write("Cpu: ");
        foreach (double cpu in CpuHistory)
        {
            Console.Write(cpu);
            Console.Write(" ");
        }

        clean(5);

        Console.Write("Ram: ");
        foreach (double ram in ramHistory)
        {
            Console.Write(ram);
            Console.Write(" ");
        }

        clean(6);

        Console.Write("Temp: ");
        foreach (double t in temperatureHistory)
        {
            Console.Write(t);
            Console.Write(" ");
        }
        Grafik(Nowcpu,7,"CPU: ");
        Grafik(Nowram,8,"RAM: ");
        Grafik(Nowtemperature,9,"TEMPERATURE: ");
        clean(10);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"CPU: {Nowcpu}");

        clean(11);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"RAM: {Nowram}");

        clean(12);
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Temperature: {Nowtemperature}");

        Console.ResetColor();
        
    }

    private static void Main()
    {
        var monitor = new ServerMonitor();

        monitor.DataReceived += OnDataReceived;

        Console.WriteLine("Мониторинг запущен.");
        Console.WriteLine("Для завершения программы нажмите Enter.");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        monitor.Start();

        Console.ReadLine();

        monitor.Stop();
    }

    private static void OnDataReceived(double cpu, double ram, double temperature)
    {
        for (var j = CpuHistory.Length - 1; j > 0; j--)
        {
        CpuHistory[j] = CpuHistory[j - 1];
        ramHistory[j] = ramHistory[j - 1];
        temperatureHistory[j] = temperatureHistory[j - 1];
        }

        CpuHistory[0] = cpu;
        ramHistory[0] = ram;
        temperatureHistory[0] = temperature;

        Table(cpu,ram,temperature);
    }
}