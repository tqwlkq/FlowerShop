using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        bool exit = false;

        while (!exit)
        {
            ShowMenu();
            Console.Write("Оберіть пункт меню: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Помилка: введіть числове значення.");
                Pause();
                continue;
            }

            switch (choice)
            {
                case 1:
                    CalculatePurchase();
                    Pause();
                    break;
                case 2:
                    ShowProducts();
                    Pause();
                    break;
                case 3:
                    ShowShopInfo();
                    Pause();
                    break;
                case 4:
                    SettingsStub();
                    Pause();
                    break;
                case 0:
                    exit = true;
                    Console.WriteLine("Вихід з програми");
                    break;
                default:
                    Console.WriteLine("Невірний пункт меню");
                    Pause();
                    break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\nГОЛОВНЕ МЕНЮ");
        Console.WriteLine("1 - Розрахунок покупки");
        Console.WriteLine("2 - Перегляд товарів");
        Console.WriteLine("3 - Інформація про магазин");
        Console.WriteLine("4 - Налаштування");
        Console.WriteLine("0 - Вихід");
    }

    static void CalculatePurchase()
    {
        Console.WriteLine("\nЛаскаво просимо до квіткового магазину");

        double price1 = 45.5;
        double price2 = 30.0;
        double price3 = 25.25;

        try
        {
            Console.Write("Скільки купити троянд: ");
            double q1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Скільки купити тюльпанів: ");
            double q2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Скільки купити ромашок: ");
            double q3 = Convert.ToDouble(Console.ReadLine());

            double sum1 = q1 * price1;
            double sum2 = q2 * price2;
            double sum3 = q3 * price3;

            double total = sum1 + sum2 + sum3;

            Random r = new Random();
            int discount = r.Next(1, 11);

            double totalWithDiscount = total - (total * discount / 100);
            double beauty = Math.Round(Math.Sqrt(total), 2);
            double result = Math.Round(totalWithDiscount, 2);

            Console.WriteLine("\nРЕЗУЛЬТАТ");
            Console.WriteLine("Троянди: " + sum1 + " грн");
            Console.WriteLine("Тюльпани: " + sum2 + " грн");
            Console.WriteLine("Ромашки: " + sum3 + " грн");
            Console.WriteLine("Загальна сума: " + total + " грн");
            Console.WriteLine("Знижка: " + discount + " відсотків");
            Console.WriteLine("Сума після знижки: " + result + " грн");
            Console.WriteLine("Коефіцієнт краси: " + beauty);
        }
        catch
        {
            Console.WriteLine("Помилка введення даних");
        }
    }

    static void ShowProducts()
    {
        Console.WriteLine("\nАсортимент магазину:");
        Console.WriteLine("Троянди");
        Console.WriteLine("Тюльпани");
        Console.WriteLine("Ромашки");
    }

    static void ShowShopInfo()
    {
        Console.WriteLine("\nІнформація про магазин:");
        Console.WriteLine("Квітковий магазин Весна");
        Console.WriteLine("Працюємо щодня з 9:00 до 20:00");
    }

    static void SettingsStub()
    {
        Console.WriteLine("\nФункція в розробці");
    }

    static void Pause()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню");
        Console.ReadKey();
    }
}


