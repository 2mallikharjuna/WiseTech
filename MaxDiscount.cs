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
        // Step 1: Store discount information in a dictionary for quick access
        Dictionary<string, List<Discount>> discountMap = new Dictionary<string, List<Discount>>();
        foreach (var discount in discounts)
        {
            string code = discount[0];
            int type = int.Parse(discount[1]);
            int value = int.Parse(discount[2]);
            
            // If multiple discounts can apply for the same code, store them all
            if (!discountMap.ContainsKey(code))
                discountMap[code] = new List<Discount>();
            
            discountMap[code].Add(new Discount(code, type, value));
        }

        int totalMinPrice = 0;

        // Step 2: Iterate over each product
        foreach (var product in products)
        {
            // First element is the original price of the product
            int originalPrice = int.Parse(product[0]);
            int minPrice = originalPrice; // Start with the original price as the minimum

            // Step 3: Use a priority queue (min-heap) to store all applicable discounted prices
            List<int> discountedPrices = new List<int> { originalPrice };

            // Check all associated discount codes for the product
            for (int i = 1; i < product.Count; i++)
            {
                string discountCode = produ
