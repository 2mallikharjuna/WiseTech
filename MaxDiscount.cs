using System;
using System.Collections.Generic;
using System.Linq;

class Discount
{
    public string Code;
    public int Type;
    public int Value;

    public Discount(string code, int type, int value)
    {
        this.Code = code;
        this.Type = type;
        this.Value = value;
    }
}

class Result
{
    // Method to calculate the discounted price based on the type of discount
    static int ApplyDiscount(int price, Discount discount)
    {
        if (discount.Type == 0) // Set fixed price
            return discount.Value;
        else if (discount.Type == 1) // Percentage discount
            return (int)Math.Round(price * (100.0 - discount.Value) / 100);
        else if (discount.Type == 2) // Flat amount discount
            return price - discount.Value;
        
        return price; // If no discount is applied
    }

    public static int findLowestPrice(List<List<string>> products, List<List<string>> discounts)
    {
        // Store discount information in a dictionary for quick access
        Dictionary<string, Discount> discountMap = new Dictionary<string, Discount>();
        foreach (var discount in discounts)
        {
            string code = discount[0];
            int type = int.Parse(discount[1]);
            int value = int.Parse(discount[2]);
            discountMap[code] = new Discount(code, type, value);
        }

        int totalMinPrice = 0;

        // Iterate over each product
        foreach (var product in products)
        {
            // First element is the original price of the product
            int originalPrice = int.Parse(product[0]);
            int minPrice = originalPrice; // Start with the original price as the minimum

            // Check all associated discount codes for the product
            for (int i = 1; i < product.Count; i++)
            {
                string discountCode = product[i];

                // If discount code exists and is valid
                if (!string.Equals(discountCode, "EMPTY", StringComparison.OrdinalIgnoreCase) && discountMap.ContainsKey(discountCode))
                {
                    Discount discount = discountMap[discountCode];
                    // Apply discount and update the minimum price
                    minPrice = Math.Min(minPrice, ApplyDiscount(originalPrice, discount));
                }
            }

            // Add the minimum price of this product to the total
            totalMinPrice += minPrice;
        }

        return totalMinPrice; // Return the total minimum price across all products
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        int productsRows = Convert.ToInt32(Console.ReadLine().Trim());
        List<List<string>> products = new List<List<string>>();

        for (int i = 0; i < productsRows; i++)
        {
            products.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
        }

        int discountsRows = Convert.ToInt32(Console.ReadLine().Trim());
        List<List<string>> discounts = new List<List<string>>();

        for (int i = 0; i < discountsRows; i++)
        {
            discounts.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
        }

        int result = Result.findLowestPrice(products, discounts);
        Console.WriteLine(result);
    }
}
