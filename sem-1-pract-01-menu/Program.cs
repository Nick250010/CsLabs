namespace MainProject;

public class Program
{

    public static void Main()
    {
        Console.WriteLine("Приветствую вас в ресторане OnlineFood!\nDля того, чтобы сделать заказ, выберите категорию блюд:");
        Console.WriteLine("1. Блюда\n2. Напитки\n3. Доступ администратора\n");
        Console.Write("Введите номер категории: ");
        var categoryInput = Convert.ToInt32(Console.ReadLine());
        List<string> order = new List<string>();
        int totalPrice = 0;
        switch (categoryInput)
        {
            case 1:
                Console.WriteLine("Вы выбрали категорию 'Блюда'\nСамое время ознакомиться с нашим меню!\n");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("1. Пицца - 500 руб (500г).\n2. Суши - 700 руб (200г).\n3. Бургер - 300 руб (100г).\n4. Салат - 200 руб (90г).");
                Console.Write("Введите номер блюда: ");
                var dishInput = Convert.ToInt32(Console.ReadLine());
                switch (dishInput)
                {
                    case 1:
                        order.Add("Пицца");
                        totalPrice += 500;
                        break;
                    case 2:
                        order.Add("Суши");
                        totalPrice += 700;
                        break;
                    case 3:
                        order.Add("Бургер");
                        totalPrice += 300;
                        break;
                    case 4:
                        order.Add("Салат");
                        totalPrice += 200;
                        break;
                }
                break;
            case 2:
                Console.WriteLine("Вы выбрали категорию 'Напитки'\nСамое время ознакомиться с нашими напитками!\n");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("1. Кола - 100 руб (500мл).\n2. Сок - 150 руб (300мл).\n3. Вода - 50 руб (500мл).\n4. Чай - 80 руб (200мл).");
                Console.Write("Введите номер напитка: ");
                var drinks = Convert.ToInt32(Console.ReadLine());
                switch (drinks)
                {
                    case 1:
                        order.Add("Кола");
                        totalPrice += 100;
                        break;
                    case 2:
                        order.Add("Сок");
                        totalPrice += 150;
                        break;
                    case 3:
                        order.Add("Вода");
                        totalPrice += 50;
                        break;
                    case 4:
                        order.Add("Чай");
                        totalPrice += 80;
                        break;
                }
                break;
        }
        Console.WriteLine("Ваш заказ:");
        foreach (var item in order)
        {
            Console.WriteLine(item," ");
        }
        Console.WriteLine($"Общая стоимость заказа: {totalPrice} руб.");
            
    }
}