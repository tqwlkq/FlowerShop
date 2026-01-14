using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;

class Product
{

    public int Id;
    public string Name;
    public double Price;
    public int Quantity;
    public string Category;


    public Product(string name, double price, int quantity, string category)
    {
        Id = 0; 
        Name = name;
        Price = price;
        Quantity = quantity;
        Category = category;
    }


    public Product(int id, string name, double price, int quantity, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
        Category = category;
    }
}

class Program
{
    static List<Product> products = new List<Product>();


    static readonly string ProductsPath = "products.csv";
    static readonly string UsersPath = "users.csv";


    static readonly string ProductsHeader = "Id,Name,Price,Quantity,Category";
    static readonly string UsersHeader = "Id,Email,PasswordHash";

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        EnsureCsvFile(ProductsPath, ProductsHeader);
        EnsureCsvFile(UsersPath, UsersHeader);

      
        products = ReadProductsFromCsv(ProductsPath);

        if (products.Count == 0)
        {
            SeedDefaultProductsIfEmpty();
            SaveAllProductsToCsv(ProductsPath, products); 
        }


        if (!AuthFlow())
            return;

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
                    SaveAllProductsToCsv(ProductsPath, products);
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
                 
                    SaveAllProductsToCsv(ProductsPath, products);
                    break;

                case 8:
                    SortProducts();

                    SaveAllProductsToCsv(ProductsPath, products);
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
        Console.WriteLine("{0,-4} {1,-6} {2,-20} {3,10} {4,8} {5,-15}", "№", "ID", "Назва", "Ціна(грн)", "К-ть", "Категорія");
        Console.WriteLine(new string('-', 75));

        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            Console.WriteLine(
                "{0,-4} {1,-6} {2,-20} {3,10:F2} {4,8} {5,-15}",
                i + 1,
                p.Id,
                Truncate(p.Name, 20),
                p.Price,
                p.Quantity,
                Truncate(p.Category, 15)
            );
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
            if (!TryParseDouble(Console.ReadLine(), out double price) || price < 0)
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

    
            int newId = GenerateNewIdFromCsv(ProductsPath);

            var p = new Product(newId, name, price, quantity, category);
            products.Add(p);

   
            AppendProductToCsv(ProductsPath, p);

            Console.WriteLine($"Товар додано! (ID={newId})");
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
        Console.WriteLine("{0,-4} {1,-6} {2,-20} {3,10} {4,8} {5,-15}", "№", "ID", "Назва", "Ціна(грн)", "К-ть", "Категорія");
        Console.WriteLine(new string('-', 75));

        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];

            if ((option == 1 && p.Name.ToLower().Contains(query)) ||
                (option == 2 && p.Category.ToLower().Contains(query)))
            {
                Console.WriteLine(
                    "{0,-4} {1,-6} {2,-20} {3,10:F2} {4,8} {5,-15}",
                    i + 1,
                    p.Id,
                    Truncate(p.Name, 20),
                    p.Price,
                    p.Quantity,
                    Truncate(p.Category, 15)
                );
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

        Console.WriteLine("Видалення: 1 - за індексом, 2 - за назвою або категорією, 3 - за ID");
        Console.Write("Оберіть опцію: ");
        if (!int.TryParse(Console.ReadLine(), out int option) || (option != 1 && option != 2 && option != 3))
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
        else if (option == 2)
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
                products.RemoveAt(toRemove[i]);

            Console.WriteLine($"Видалено {toRemove.Count} елемент(ів).");
        }
        else
        {
            Console.Write("Введіть ID товару для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некоректний ID.");
                return;
            }

            int removed = 0;
            for (int i = products.Count - 1; i >= 0; i--)
            {
                if (products[i].Id == id)
                {
                    products.RemoveAt(i);
                    removed++;
                }
            }

            Console.WriteLine(removed > 0 ? $"Видалено: {removed}" : "Товар з таким ID не знайдено.");
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
            copyForBuiltin.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.CurrentCulture));
        else
            copyForBuiltin.Sort((a, b) => a.Price.CompareTo(b.Price));

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



    static void EnsureCsvFile(string path, string header)
    {
        try
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, header + Environment.NewLine, Encoding.UTF8);
                return;
            }

            
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]) || lines[0].Trim() != header)
            {
              
                string bak = path + ".bak";
                try { File.Copy(path, bak, true); } catch { }

                File.WriteAllText(path, header + Environment.NewLine, Encoding.UTF8);
            }
        }
        catch
        {
            Console.WriteLine($"Помилка роботи з файлом: {path}");
            Console.WriteLine("Перевірте права доступу або місце на диску.");
            Environment.Exit(0);
        }
    }

    static List<Product> ReadProductsFromCsv(string path)
    {
        List<Product> list = new List<Product>();

        try
        {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length == 0) return list;

 
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

         
                string[] parts = line.Split(',');

             
                if (parts.Length != 5)
                    continue;


                if (!int.TryParse(parts[0], out int id))
                    continue;

                string name = parts[1]?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                if (!TryParseDouble(parts[2], out double price))
                    continue;

                if (!int.TryParse(parts[3], out int qty))
                    continue;

                string cat = parts[4]?.Trim() ?? "Без категорії";

                list.Add(new Product(id, name, price, qty, cat));
            }
        }
        catch
        {
       
            return list;
        }

        return list;
    }

    static void SaveAllProductsToCsv(string path, List<Product> list)
    {
        try
        {
            List<string> lines = new List<string>();
            lines.Add(ProductsHeader);

            for (int i = 0; i < list.Count; i++)
            {
                
                string line = $"{list[i].Id},{SanitizeCsv(list[i].Name)},{list[i].Price.ToString(CultureInfo.InvariantCulture)},{list[i].Quantity},{SanitizeCsv(list[i].Category)}";
                lines.Add(line);
            }

            File.WriteAllLines(path, lines.ToArray(), Encoding.UTF8);
        }
        catch
        {
            Console.WriteLine("Помилка збереження products.csv");
        }
    }

    static void AppendProductToCsv(string path, Product p)
    {
        try
        {
            string line = $"{p.Id},{SanitizeCsv(p.Name)},{p.Price.ToString(CultureInfo.InvariantCulture)},{p.Quantity},{SanitizeCsv(p.Category)}";
            File.AppendAllText(path, line + Environment.NewLine, Encoding.UTF8);
        }
        catch
        {
            Console.WriteLine("Помилка дозапису в products.csv");
        }
    }

    static int GenerateNewIdFromCsv(string path)
    {
        int max = 0;

        try
        {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length < 1) continue;

                if (int.TryParse(parts[0], out int id))
                {
                    if (id > max) max = id;
                }
            }
        }
        catch
        {
            return 1;
        }

        return max + 1;
    }

    static string SanitizeCsv(string s)
    {
        if (s == null) return "";
        s = s.Replace("\r", " ").Replace("\n", " ").Replace(",", " ");
        return s.Trim();
    }

    static bool TryParseDouble(string? input, out double value)
    {
        value = 0;

        if (input == null) return false;
        input = input.Trim();

        input = input.Replace(',', '.');

        return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    static void SeedDefaultProductsIfEmpty()
    {

        int id1 = GenerateNewIdFromCsv(ProductsPath);
        products.Add(new Product(id1, "Троянди", 45.5, 10, "Квіти"));

        int id2 = id1 + 1;
        products.Add(new Product(id2, "Тюльпани", 30.0, 15, "Квіти"));

        int id3 = id2 + 1;
        products.Add(new Product(id3, "Ромашки", 25.25, 20, "Квіти"));

        int id4 = id3 + 1;
        products.Add(new Product(id4, "Лілії", 560.0, 5, "Квіти"));

        int id5 = id4 + 1;
        products.Add(new Product(id5, "Півонії", 520.0, 3, "Квіти"));
    }


    static bool AuthFlow()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1 - Вхід (авторизація)");
            Console.WriteLine("2 - Реєстрація");
            Console.WriteLine("0 - Вихід");
            Console.Write("Оберіть: ");

            string? s = Console.ReadLine();
            if (!int.TryParse(s, out int choice))
            {
                Console.WriteLine("Некоректний вибір.");
                Pause();
                continue;
            }

            if (choice == 0) return false;
            if (choice == 1)
            {
                if (LoginUser())
                    return true;

                Pause();
                continue;
            }
            if (choice == 2)
            {
                RegisterUser();
                Pause();
                continue;
            }

            Console.WriteLine("Невірний пункт.");
            Pause();
        }
    }

    static void RegisterUser()
    {
        Console.Clear();
        Console.WriteLine("РЕЄСТРАЦІЯ");

        Console.Write("Email: ");
        string email = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
        {
            Console.WriteLine("Некоректний email.");
            return;
        }

        if (UserExists(email))
        {
            Console.WriteLine("Такий email вже зареєстрований.");
            return;
        }

        Console.Write("Пароль: ");
        string pass = Console.ReadLine() ?? "";

        if (pass.Length < 4)
        {
            Console.WriteLine("Пароль надто короткий (мінімум 4 символи).");
            return;
        }

        int newId = GenerateNewUserId(UsersPath);
        string hash = HashPassword(pass);

        try
        {
            string line = $"{newId},{SanitizeCsv(email)},{hash}";
            File.AppendAllText(UsersPath, line + Environment.NewLine, Encoding.UTF8);
            Console.WriteLine("Користувача зареєстровано успішно.");
        }
        catch
        {
            Console.WriteLine("Помилка запису в users.csv");
        }
    }

    static bool LoginUser()
    {
        Console.Clear();
        Console.WriteLine("АВТОРИЗАЦІЯ");

        int attempts = 3;
        while (attempts > 0)
        {
            Console.Write("Email: ");
            string email = (Console.ReadLine() ?? "").Trim();

            Console.Write("Пароль: ");
            string pass = Console.ReadLine() ?? "";

            if (CheckCredentials(email, pass))
            {
                Console.WriteLine("Вхід успішний.");
                return true;
            }

            attempts--;
            if (attempts > 0)
                Console.WriteLine("\nНеправильні дані! Спробуйте ще раз.");
            else
                Console.WriteLine("\nВи вичерпали всі спроби.");
        }

        return false;
    }

    static bool UserExists(string email)
    {
        try
        {
            string[] lines = File.ReadAllLines(UsersPath, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length != 3) continue;

                string fileEmail = (parts[1] ?? "").Trim();
                if (string.Equals(fileEmail, email, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    static bool CheckCredentials(string email, string password)
    {
        string passHash = HashPassword(password);

        try
        {
            string[] lines = File.ReadAllLines(UsersPath, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length != 3) continue;

                string fileEmail = (parts[1] ?? "").Trim();
                string fileHash = (parts[2] ?? "").Trim();

                if (string.Equals(fileEmail, email, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(fileHash, passHash, StringComparison.Ordinal))
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    static int GenerateNewUserId(string path)
    {
        int max = 0;

        try
        {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] parts = lines[i].Split(',');
                if (parts.Length < 1) continue;

                if (int.TryParse(parts[0], out int id))
                    if (id > max) max = id;
            }
        }
        catch
        {
            return 1;
        }

        return max + 1;
    }

    static string HashPassword(string password)
    {
       
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
