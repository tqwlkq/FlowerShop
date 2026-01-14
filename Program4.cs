using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

class Product
{
    public string Name;
    public double Price;
    public int Quantity;
    public string Category;

    public Product(string name, double price, int quantity, string category)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        Category = category;
    }
}

class Program
{
    static List<Product> products = new List<Product>();

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string correctLogin = "meow";
        string correctPassword = "ShadowFiend228";
        int attempts = 3;

        do
        {
            Console.Write("Введіть логін: ");
            string login = Console.ReadLine();
            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            if (login == correctLogin && password == correctPassword) break;

            attempts--;
            if (attempts > 0)
                Console.WriteLine("\nНеправильний пароль! Спробуйте ще раз.");
            else
            {
                Console.WriteLine("\nВи вичерпали всі спроби.");
                Console.WriteLine("Натисніть будь-яку клавішу щоб вийти.");
                Console.ReadKey();
                return;
            }
        } while (attempts > 0);

        products.Add(new Product("Троянди", 45.5, 10, "Квіти"));
        products.Add(new Product("Тюльпани", 30.0, 15, "Квіти"));
        products.Add(new Product("Ромашки", 25.25, 20, "Квіти"));
        products.Add(new Product("Лілії", 560.0, 5, "Квіти"));
        products.Add(new Product("Півонії", 520.0, 3, "Квіти"));

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
                    break;
                case 2:
                    ShowProducts();
                    break;
                case 3:
                    ShowShopInfo();
                    break;
                case 4:
                    ShowStatistics();
                    break;
                case 5:
                    AddProduct();
                    break;
                case 6:
                    SearchProduct();
                    break;
                case 7:
                    DeleteProduct();
                    break;
                case 8:
                    SortProducts();
                    break;
                case 0:
                    Console.WriteLine("Вихід з програми");
                    return;
                default:
                    Console.WriteLine("Невірний пункт меню");
                    break;
            }

            Pause();
        }
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("ГОЛОВНЕ МЕНЮ");
        Console.WriteLine("1 - Розрахунок покупки");
        Console.WriteLine("2 - Вивести всі елементи");
        Console.WriteLine("3 - Інформація про магазин");
        Console.WriteLine("4 - Статистика");
        Console.WriteLine("5 - Додати елемент(и)");
        Console.WriteLine("6 - Пошук");
        Console.WriteLine("7 - Видалення");
        Console.WriteLine("8 - Сортування");
        Console.WriteLine("0 - Вихід");
    }

    static void Pause()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню");
        Console.ReadKey();
        Console.Clear();
    }


    static void CalculatePurchase()
    {
        Console.Clear();
        if (products.Count == 0)
        {
            Console.WriteLine("Асортимент порожній.");
            return;
        }

        Console.WriteLine("Ласкаво просимо до квіткового магазину\n");
        Console.WriteLine("Асортимент:");
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {products[i].Name} — {products[i].Price:F2} грн (в наявності: {products[i].Quantity})");
        }

        int[] buyQuantities = new int[products.Count];

        for (int i = 0; i < products.Count; i++)
        {
            while (true)
            {
                Console.Write($"Скільки купити {products[i].Name}: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int q) || q < 0)
                {
                    Console.WriteLine("Некоректне число. Спробуйте ще раз.");
                    continue;
                }

                if (q > products[i].Quantity)
                {
                    Console.WriteLine($"У нас є тільки {products[i].Quantity} шт. {products[i].Name}. Буде куплено {products[i].Quantity} шт.");
                    q = products[i].Quantity;
                }

                buyQuantities[i] = q;
                break;
            }
        }

        double totalSum = 0;
        int totalFlowers = 0;

        for (int i = 0; i < products.Count; i++)
        {
            totalSum += buyQuantities[i] * products[i].Price;
            totalFlowers += buyQuantities[i];
            products[i].Quantity -= buyQuantities[i];
        }

        if (totalFlowers == 0)
        {
            Console.WriteLine("Ви нічого не обрали.");
            return;
        }

        Random r = new Random();
        int discount = r.Next(1, 11);
        double totalWithDiscount = Math.Round(totalSum - totalSum * discount / 100.0, 2);

        Console.WriteLine("\nРЕЗУЛЬТАТ");
        for (int i = 0; i < products.Count; i++)
        {
            if (buyQuantities[i] > 0)
            {
                double sum = buyQuantities[i] * products[i].Price;
                Console.WriteLine($"{products[i].Name}: {buyQuantities[i]} шт. x {products[i].Price:F2} грн = {sum:F2} грн");
            }
        }

        Console.WriteLine($"Загальна кількість квітів: {totalFlowers} шт.");
        Console.WriteLine($"Загальна сума: {totalSum:F2} грн");
        Console.WriteLine($"Знижка: {discount}%");
        Console.WriteLine($"Сума після знижки: {totalWithDiscount:F2} грн");
    }

    static void ShowProducts()
    {
        Console.Clear();
        if (products.Count == 0)
        {
            Console.WriteLine("Асортимент порожній.");
            return;
        }

        Console.WriteLine("Асортимент магазину:");
        Console.WriteLine("{0,-4} {1,-20} {2,10} {3,8} {4,-15}", "№", "Назва", "Ціна(грн)", "К-ть", "Категорія");
        Console.WriteLine(new string('-', 60));
        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            Console.WriteLine("{0,-4} {1,-20} {2,10:F2} {3,8} {4,-15}", i + 1, Truncate(p.Name, 20), p.Price, p.Quantity, Truncate(p.Category, 15));
        }
    }

    static string Truncate(string s, int max)
    {
        if (s == null) return "";
        return s.Length <= max ? s : s.Substring(0, max - 3) + "...";
    }

    static void ShowShopInfo()
    {
        Console.Clear();
        Console.WriteLine("Інформація про магазин:");
        Console.WriteLine("Квітковий магазин Весна");
        Console.WriteLine("Працюємо щодня з 9:00 до 20:00");
    }

    static void ShowStatistics()
    {
        Console.Clear();
        if (products.Count == 0)
        {
            Console.WriteLine("Колекція пуста. Немає даних для статистики.");
            return;
        }


        double totalValue = 0; 
        double minPrice = double.MaxValue;
        double maxPrice = double.MinValue;
        int totalQuantity = 0;
        double sumPrices = 0; 

        foreach (var p in products)
        {
            if (p.Price <= 0) continue;
            totalValue += p.Price * p.Quantity;
            if (p.Price < minPrice) minPrice = p.Price;
            if (p.Price > maxPrice) maxPrice = p.Price;
            totalQuantity += p.Quantity;
            sumPrices += p.Price;
        }

        double averagePricePerProduct = sumPrices / products.Count;
        double averagePriceWeighted = totalQuantity > 0 ? totalValue / totalQuantity : 0;

        int expensiveCount = 0;
        foreach (var p in products)
            if (p.Price > 500) expensiveCount++;

        Console.WriteLine($"Кількість позицій: {products.Count}");
        Console.WriteLine($"Загальна вартість всього на складі (Price * Quantity): {totalValue:F2} грн");
        Console.WriteLine($"Кількість товарів дорожче 500 грн: {expensiveCount}");
        Console.WriteLine($"Мінімальна ціна: {(minPrice == double.MaxValue ? 0 : minPrice):F2}");
        Console.WriteLine($"Максимальна ціна: {(maxPrice == double.MinValue ? 0 : maxPrice):F2}");
        Console.WriteLine($"Середня ціна по позиціях: {averagePricePerProduct:F2} грн");
        Console.WriteLine($"Середня ціна по одиниці (враховуючи кількість): {averagePriceWeighted:F2} грн");
    }

    static void AddProduct()
    {
        Console.Clear();
        Console.Write("Скільки товарів додати? (введіть ціле число): ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Некоректна кількість. Скасовано.");
            return;
        }

        for (int k = 0; k < count; k++)
        {
            Console.WriteLine($"\nТовар #{k + 1}:");
            Console.Write("Введіть назву товару: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Назва не може бути порожньою! Пропускаю цей товар.");
                continue;
            }

            Console.Write("Ціна: ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price < 0)
            {
                Console.WriteLine("Некоректна ціна! Пропускаю цей товар.");
                continue;
            }

            Console.Write("Кількість: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Некоректна кількість! Пропускаю цей товар.");
                continue;
            }

            Console.Write("Категорія: ");
            string category = Console.ReadLine() ?? "Без категорії";

            products.Add(new Product(name, price, quantity, category));
            Console.WriteLine("Товар додано!");
        }
    }

    static void SearchProduct()
    {
        Console.Clear();
        if (products.Count == 0)
        {
            Console.WriteLine("Колекція пуста.");
            return;
        }

        Console.WriteLine("Пошук за: 1 - Назва, 2 - Категорія");
        Console.Write("Оберіть опцію: ");
        if (!int.TryParse(Console.ReadLine(), out int option) || (option != 1 && option != 2))
        {
            Console.WriteLine("Некоректний вибір.");
            return;
        }

        Console.Write("Введіть рядок для пошуку (частина або повністю): ");
        string query = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Порожній запит.");
            return;
        }
        query = query.ToLower();

        bool found = false;
        Console.WriteLine("{0,-4} {1,-20} {2,10} {3,8} {4,-15}", "№", "Назва", "Ціна(грн)", "К-ть", "Категорія");
        Console.WriteLine(new string('-', 60));
        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            if ((option == 1 && p.Name.ToLower().Contains(query)) ||
                (option == 2 && p.Category.ToLower().Contains(query)))
            {
                Console.WriteLine("{0,-4} {1,-20} {2,10:F2} {3,8} {4,-15}", i + 1, Truncate(p.Name, 20), p.Price, p.Quantity, Truncate(p.Category, 15));
                found = true;
            }
        }

        if (!found) Console.WriteLine("Товар не знайдено.");
    }

    static void DeleteProduct()
    {
        Console.Clear();
        if (products.Count == 0)
        {
            Console.WriteLine("Колекція пуста. Немає що видаляти.");
            return;
        }

        Console.WriteLine("Видалення: 1 - за індексом, 2 - за назвою або категорією");
        Console.Write("Оберіть опцію: ");
        if (!int.TryParse(Console.ReadLine(), out int option) || (option != 1 && option != 2))
        {
            Console.WriteLine("Некоректний вибір.");
            return;
        }

        if (option == 1)
        {
            Console.Write("Введіть індекс товару для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > products.Count)
            {
                Console.WriteLine("Некоректний індекс!");
                return;
            }
            products.RemoveAt(index - 1);
            Console.WriteLine("Товар видалено!");
        }
        else
        {
            Console.Write("Введіть назву або категорію для видалення (точне співпадіння): ");
            string key = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(key))
            {
                Console.WriteLine("Порожній запит.");
                return;
            }

            key = key.ToLower();
            List<int> toRemove = new List<int>();
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Name.ToLower() == key || products[i].Category.ToLower() == key)
                    toRemove.Add(i);
            }

            if (toRemove.Count == 0)
            {
                Console.WriteLine("Елементів для видалення не знайдено.");
                return;
            }

          
            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                products.RemoveAt(toRemove[i]);
            }

            Console.WriteLine($"Видалено {toRemove.Count} елемент(ів).");
        }
    }

    static void SortProducts()
    {
        Console.Clear();
        if (products.Count < 2)
        {
            Console.WriteLine("Недостатньо елементів для сортування.");
            return;
        }

        Console.WriteLine("1 - За назвою");
        Console.WriteLine("2 - За ціною");
        Console.Write("Оберіть спосіб сортування: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Некоректний вибір!");
            return;
        }

      
        List<Product> copyForBuiltin = new List<Product>(products);

    
        for (int i = 0; i < products.Count - 1; i++)
        {
            for (int j = 0; j < products.Count - i - 1; j++)
            {
                bool swap = false;
                if (choice == 1 && string.Compare(products[j].Name, products[j + 1].Name, StringComparison.CurrentCulture) > 0)
                    swap = true;
                if (choice == 2 && products[j].Price > products[j + 1].Price)
                    swap = true;

                if (swap)
                {
                    var temp = products[j];
                    products[j] = products[j + 1];
                    products[j + 1] = temp;
                }
            }
        }

     
        if (choice == 1)
        {
            copyForBuiltin.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.CurrentCulture));
        }
        else
        {
            copyForBuiltin.Sort((a, b) => a.Price.CompareTo(b.Price));
        }

   
        bool same = true;
        for (int i = 0; i < products.Count; i++)
        {
            if (choice == 1)
            {
                if (!string.Equals(products[i].Name, copyForBuiltin[i].Name, StringComparison.CurrentCulture))
                {
                    same = false;
                    break;
                }
            }
            else
            {
                if (products[i].Price != copyForBuiltin[i].Price || products[i].Name != copyForBuiltin[i].Name)
                {
                    same = false;
                    break;
                }
            }
        }

        Console.WriteLine("Сортування виконано (власний алгоритм: бульбашка).");
        Console.WriteLine($"Результати однакові з List.Sort(): {(same ? "так" : "ні")}");
    }
}