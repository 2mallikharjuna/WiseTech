
using System;

class Discount
{
    public string Index;
    public int Type;
    public int Value;
    public Discount(string index, int type, int value)
    {
        this.Index = index;
        this.Type = type;
        this.Value = value;
    }
}

class Result
{

    /*
     * Complete the 'findLowestPrice' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts following parameters:
     *  1. 2D_STRING_ARRAY products
     *  2. 2D_STRING_ARRAY discounts
     */
    static int CalcDiscountAmount(int taggedPrice, Discount discount)
    {

        if (discount.Type == 0)
        {
            taggedPrice = discount.Value;
        }
        else if (discount.Type == 1)
        {
            taggedPrice = (int)Math.Round(taggedPrice * (100.0 - discount.Value) / 100);
        }
        else if (discount.Type == 2)
        {
            taggedPrice -= discount.Value;
        }

        return taggedPrice;

    }

    public static int findLowestPrice(List<List<string>> products, List<List<string>> discounts)
    {
        int result = 0;
        Dictionary<string, Discount> productDiscount = new Dictionary<string, Discount>();
        foreach (var discount in discounts)
        {
            string[] d = discount.ToArray();
            int type = int.Parse(d[1]);
            int value = int.Parse(d[2]);
            if (!productDiscount.ContainsKey(d[0]))
                productDiscount.Add(d[0], new Discount(d[0], type, value));
        }

        foreach (var product in products)
        {
            int price = int.Parse(product[0]);
            int min = price;
            for (int i = 0; i < product.Count; i++)
            {
                int currentPrice = price;
                string index = product[i];
                if (!string.Equals(index, "EMPTY", StringComparison.OrdinalIgnoreCase))
                {
                    if (productDiscount.ContainsKey(index))
                    {
                        Discount discount = productDiscount[index];

                        min = Math.Min(min, CalcDiscountAmount(currentPrice, discount));
                    }
                }

            }
            result += min;
        }
        return result;
    }


}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int productsRows = Convert.ToInt32(Console.ReadLine().Trim());
        int productsColumns = Convert.ToInt32(Console.ReadLine().Trim());

        List<List<string>> products = new List<List<string>>();

        for (int i = 0; i < productsRows; i++)
        {
            products.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
        }

        int discountsRows = Convert.ToInt32(Console.ReadLine().Trim());
        int discountsColumns = Convert.ToInt32(Console.ReadLine().Trim());

        List<List<string>> discounts = new List<List<string>>();

        for (int i = 0; i < discountsRows; i++)
        {
            discounts.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
        }

        int result = Result.findLowestPrice(products, discounts);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}

