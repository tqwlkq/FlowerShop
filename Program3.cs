using System;
using System.Text;

struct Product
{
    public string name;
    public double price;
    public int quantity;
    public string category;

    public Product(string name, double price, int quantity, string category)
    {
        this.name = name;
        this.price = price;
        this.quantity = quantity;
        this.category = category;
    }
}

class Program
{
    static Product[] products = new Product[5];

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // ===== СИСТЕМА ВХОДУ =====
        string correctLogin = "meow";
        string correctPassword = "ShadowFiend228";
        int attempts = 3;
        bool accessGranted = false;

        do
        {
            Console.Write("Введіть логін: ");
            string login = Console.ReadLine();
            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            if (login == correctLogin && password == correctPassword)
            {
                accessGranted = true;
                break;
            }

            attempts--;
            if (attempts > 0) Console.WriteLine("\nНеправильний пароль! Спробуйте ще раз.");
            else
            {
                Console.WriteLine("\nВи вичерпали всі спроби.");
                Console.WriteLine("Натисніть будь-яку клавішу щоб вийти.");
                Console.ReadKey();
                return;
            }
        }
        while (attempts > 0);

        // ===== ЗАПОВНЕННЯ МАСИВУ =====
        products[0] = new Product("Троянди", 45.5, 10, "Квіти");
        products[1] = new Product("Тюльпани", 30.0, 15, "Квіти");
        products[2] = new Product("Ромашки", 25.25, 20, "Квіти");
        products[3] = new Product("Лілії", 560.0, 5, "Квіти");
        products[4] = new Product("Півонії", 520.0, 3, "Квіти");

        while (true)
        {
            ShowMenu();
            Console.Write("Оберіть пункт меню: ");
            string? input = Console.ReadLine();

            if (input is null || !int.TryParse(input, out int choice))
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
                    ShowStatistics();
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
        Console.WriteLine("ГОЛОВНЕ МЕНЮ");
        Console.WriteLine("1 - Розрахунок покупки");
        Console.WriteLine("2 - Перегляд товарів");
        Console.WriteLine("3 - Інформація про магазин");
        Console.WriteLine("4 - Статистика");
        Console.WriteLine("0 - Вихід");
    }

    static void ShowStatistics()
    {
        Console.Clear();

        double totalSum = 0;
        double minPrice = double.MaxValue;
        double maxPrice = double.MinValue;
        int expensiveCount = 0;

        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].price <= 0)
                continue;

            totalSum += products[i].price * products[i].quantity;

            if (products[i].price < minPrice)
                minPrice = products[i].price;

            if (products[i].price > maxPrice)
                maxPrice = products[i].price;

            if (products[i].price > 500)
                expensiveCount++;
        }

        double average = totalSum / products.Length;


        Console.WriteLine($"Загальна сума: {totalSum} грн");
        Console.WriteLine($"Кількість товарів дорожче 500 грн: {expensiveCount}");
        Console.WriteLine($"Мінімальна ціна: {minPrice}");
        Console.WriteLine($"Максимальна ціна: {maxPrice}");
        Console.WriteLine($"Середня ціна: {average}");
    }

    static void CalculatePurchase()
    {
        Console.Clear();
        Console.WriteLine("Ласкаво просимо до квіткового магазину");

        // Показати асортимент з ціною та наявністю
        Console.WriteLine("\nАсортимент:");
        for (int i = 0; i < products.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {products[i].name} — {products[i].price} грн (в наявності: {products[i].quantity})");
        }

        int[] buyQuantities = new int[products.Length];

        // Запитати кількості для кожного товару
        for (int i = 0; i < products.Length; i++)
        {
            Console.Write($"Скільки купити {products[i].name}: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int q) || q < 0)
            {
                Console.WriteLine($"Помилка введення кількості для {products[i].name}");
                return;
            }

            if (q > products[i].quantity)
            {
                Console.WriteLine($"У нас є тільки {products[i].quantity} шт. {products[i].name}. Буде куплено {products[i].quantity} шт.");
                q = products[i].quantity;
            }

            buyQuantities[i] = q;
        }

        // Розрахунок сум
        double totalSum = 0;
        int totalFlowers = 0;

        for (int i = 0; i < products.Length; i++)
        {
            totalSum += buyQuantities[i] * products[i].price;
            totalFlowers += buyQuantities[i];
        }

        Random r = new Random();
        int discount = r.Next(1, 11);

        double totalWithDiscount = Math.Round(totalSum - totalSum * discount / 100.0, 2);

        Console.WriteLine("\nРЕЗУЛЬТАТ");

        bool any = false;
        for (int i = 0; i < products.Length; i++)
        {
            if (buyQuantities[i] > 0)
            {
                any = true;

                double sum = buyQuantities[i] * products[i].price;
                Console.WriteLine($"{products[i].name}: {buyQuantities[i]} шт. x {products[i].price} грн = {sum} грн");
            }
        }

        if (!any)
        {
            Console.WriteLine("Ви нічого не обрали.");
            return;
        }

        // Зменшення кількості товару після покупки
        for (int i = 0; i < products.Length; i++)
        {
            products[i].quantity -= buyQuantities[i];
        }

        Console.WriteLine($"Загальна кількість квітів: {totalFlowers} шт.");
        Console.WriteLine($"Загальна сума: {totalSum} грн");
        Console.WriteLine($"Знижка: {discount}%");
        Console.WriteLine($"Сума після знижки: {totalWithDiscount} грн");
    }

    static void ShowProducts()
    {
        Console.Clear();
        Console.WriteLine("Асортимент магазину:");

        for (int i = 0; i < products.Length; i++)
        {
            var p = products[i];
            Console.WriteLine($"{i + 1}. {p.name} - {p.price} грн | В наявності: {p.quantity} | Категорія: {p.category}");
        }
    }

    static void ShowShopInfo()
    {
        Console.Clear();
        Console.WriteLine("Інформація про магазин:");
        Console.WriteLine("Квітковий магазин Весна");
        Console.WriteLine("Працюємо щодня з 9:00 до 20:00");
    }

    static void Pause()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню");
        Console.ReadKey();
        Console.Clear();
    }
}