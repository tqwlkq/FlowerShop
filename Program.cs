using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.WriteLine(" Ласкаво просимо до квіткового магазину ");

        double price1 = 45.5;
        double price2 = 30.0;
        double price3 = 25.25;

        Console.Write("Скільки купити троянд (45.5 грн): ");
        double q1 = Convert.ToDouble(Console.ReadLine());
        Console.Write("Скільки купити тюльпанів (30 грн): ");
        double q2 = Convert.ToDouble(Console.ReadLine());
        Console.Write("Скільки купити ромашок (25.25 грн): ");
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

        Console.WriteLine("\n===== РЕЗУЛЬТАТ =====");
        Console.WriteLine("Троянди: " + sum1 + " грн");
        Console.WriteLine("Тюльпани: " + sum2 + " грн");
        Console.WriteLine("Ромашки: " + sum3 + " грн");
        Console.WriteLine("Загальна сума: " + total + " грн");
        Console.WriteLine("Знижка: " + discount + "%");
        Console.WriteLine("Сума після знижки: " + result + " грн");
        Console.WriteLine("Коефіцієнт краси (√суми): " + beauty);
        Console.WriteLine("\nДякуємо за покупку ");
    }
}
