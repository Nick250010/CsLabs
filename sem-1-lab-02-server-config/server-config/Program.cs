namespace MainProject;

public class Program
{
    public static String CheckConfiguration(int people, int ram, bool hz, bool password)
    {
        string result = "";
        float ramForPeople = 6.25f;
        bool ANram = ((people/ram) > ramForPeople) ? false : true;
        bool ppl = people >0 ? true : false;
        bool ps = password ? false : true;
        List<string> errorCritical = [];
        List<string> errorNonCritical = [];
        if(!ANram){errorNonCritical.Add("для такого количества игроков рекомендуется больше оперативной памяти.");}
        if(!ppl){errorCritical.Add("количество игроков должно быть больше нуля.");}
        if(!ps){errorNonCritical.Add("публичный сервер защищён паролем.");}
        if(errorCritical.Count != 0)
        {
            result+="Запуск невозможен: ";
            for(int i = 0; i < errorCritical.Count; i++)
            {
                result+= $"{errorCritical[i]}";
            }
            return result;
        }else if (errorNonCritical.Count != 0)
        {
            result+="Запуск возможен, но есть следующие замечания: ";
            for(int i = 0; i < errorNonCritical.Count; i++)
            {
                result+= $"{errorNonCritical[i]}";
            }
            return result;
        }
        return result;
    }
    

    public static void Main()
    {
        Console.WriteLine(CheckConfiguration(50, 8, true, false));
        Console.WriteLine(CheckConfiguration(0, 8, true, false));
        Console.WriteLine(CheckConfiguration(50, 1, true, false));
        Console.WriteLine(CheckConfiguration(50, 8, true, true));
        Console.WriteLine(CheckConfiguration(150, 4, true, false));
        Console.WriteLine(CheckConfiguration(0, 8, true, true));
    }
}