using System.Collections;

namespace CoreRDLCReportViewer
{
    public class ProductList
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string OrderId { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Ingredients { get; set; }
        public int PreparationTimeMinutes { get; set; } // Add this line for the new property
        public bool Availability { get; set; } // Add this line for the new property
        public string DietaryInformation { get; set; } // Add this line for the new property

        public static IList<ProductList> GetData()
        {
            List<ProductList> datas = new List<ProductList>();
            ProductList data = null;
            data = new ProductList()
            {
                ProductId = 1,
                ProductName = "Baked Chicken and Cheese",
                OrderId = "323B60",
                Price = 55,
                Category = "Non-Veg",
                Ingredients = "grilled chicken, corn and olives.",
                PreparationTimeMinutes = 40,
                Availability = true,
                DietaryInformation = "Contains dairy",
            };
            datas.Add(data);

            data = new ProductList()
            {
                ProductId = 2,
                ProductName = "Chicken Delite",
                OrderId = "323B61",
                Price = 100,
                Category = "Non-Veg",
                Ingredients = "cheese, chicken chunks, onions & pineapple chunks.",
                PreparationTimeMinutes = 35,
                Availability = true,
                DietaryInformation = "Pineapple allergen",
            };
            datas.Add(data);

            data = new ProductList()
            {
                ProductId = 3,
                ProductName = "Chicken Tikka",
                OrderId = "323B62",
                Price = 64,
                Category = "Non-Veg",
                Ingredients = "onions, grilled chicken, chicken salami & tomatoes.",
                PreparationTimeMinutes = 45,
                Availability = true,
                DietaryInformation = "Gluten-free",
            };
            datas.Add(data);
            data = new ProductList()
            {
                ProductId = 4,
                ProductName = "Vegetarian Supreme",
                OrderId = "323B63",
                Price = 80,
                Category = "Vegetarian",
                Ingredients = "bell peppers, mushrooms, black olives & sweet corn.",
                PreparationTimeMinutes = 30,
                Availability = true,
                DietaryInformation = "Vegetarian",
            };
            datas.Add(data);

            data = new ProductList()
            {
                ProductId = 5,
                ProductName = "Margherita Pizza",
                OrderId = "323B64",
                Price = 70,
                Category = "Vegetarian",
                Ingredients = "tomato sauce, fresh mozzarella & basil leaves.",
                PreparationTimeMinutes = 25,
                Availability = true,
                DietaryInformation = "Vegetarian",
            };
            datas.Add(data);
            Console.WriteLine("Data retrieved: " + datas.Count);

            return datas;
        }
    }
}