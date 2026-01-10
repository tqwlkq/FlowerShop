using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
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
                    Console.WriteLine("Вихід з програми");
                    return;
                default:
                    Console.WriteLine("Невірний пункт меню");
                    Pause();
                    break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("\nГОЛОВНЕ МЕНЮ");
        Console.WriteLine("1 - Розрахунок покупки");
        Console.WriteLine("2 - Перегляд товарів");
        Console.WriteLine("3 - Інформація про магазин");
        Console.WriteLine("4 - Налаштування");
        Console.WriteLine("0 - Вихід");
    }

    static void CalculatePurchase()
    {
        Console.Clear();
        Console.WriteLine("\nЛаскаво просимо до квіткового магазину");

        double priceRoses = 45.5;
        double priceTulips = 30.0;
        double priceChamomiles = 25.25;

        Console.Write("Скільки купити троянд: ");
        if (!int.TryParse(Console.ReadLine(), out int qRoses) || qRoses < 0)
        {
            Console.WriteLine("Помилка введення кількості троянд");
            return;
        }

        Console.Write("Скільки купити тюльпанів: ");
        if (!int.TryParse(Console.ReadLine(), out int qTulips) || qTulips < 0)
        {
            Console.WriteLine("Помилка введення кількості тюльпанів");
            return;
        }

        Console.Write("Скільки купити ромашок: ");
        if (!int.TryParse(Console.ReadLine(), out int qChamomiles) || qChamomiles < 0)
        {
            Console.WriteLine("Помилка введення кількості ромашок");
            return;
        }

        double sumRoses = qRoses * priceRoses;
        double sumTulips = qTulips * priceTulips;
        double sumChamomiles = qChamomiles * priceChamomiles;

        double total = sumRoses + sumTulips + sumChamomiles;
        int totalFlowers = qRoses + qTulips + qChamomiles;

        Random r = new Random();
        int discount = r.Next(1, 11);

        double totalWithDiscount = Math.Round(total - total * discount / 100, 2);

        Console.WriteLine("\nРЕЗУЛЬТАТ");
        Console.WriteLine($"Троянди: {qRoses} шт. x {priceRoses} грн = {sumRoses} грн");
        Console.WriteLine($"Тюльпани: {qTulips} шт. x {priceTulips} грн = {sumTulips} грн");
        Console.WriteLine($"Ромашки: {qChamomiles} шт. x {priceChamomiles} грн = {sumChamomiles} грн");
        Console.WriteLine($"Загальна кількість квітів: {totalFlowers} шт.");
        Console.WriteLine($"Загальна сума: {total} грн");
        Console.WriteLine($"Знижка: {discount}%");
        Console.WriteLine($"Сума після знижки: {totalWithDiscount} грн");
    }


    static void ShowProducts()
    {
        Console.Clear();
        Console.WriteLine("\nАсортимент магазину:");
        Console.WriteLine("Троянди");
        Console.WriteLine("Тюльпани");
        Console.WriteLine("Ромашки");
    }

    static void ShowShopInfo()
    {
        Console.Clear();
        Console.WriteLine("\nІнформація про магазин:");
        Console.WriteLine("Квітковий магазин Весна");
        Console.WriteLine("Працюємо щодня з 9:00 до 20:00");
    }

    static void SettingsStub()
    {
        Console.Clear();
        Console.WriteLine("\nФункція в розробці");
    }

    static void Pause()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню");
        Console.ReadKey();
        Console.Clear();
    }
}

