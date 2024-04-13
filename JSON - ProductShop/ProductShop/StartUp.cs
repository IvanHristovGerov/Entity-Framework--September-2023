using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new();

            //1.Import Data
            //string userJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, userJson));

            //2.Import Products
            //string productJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productJson));

            //3.Import Categories
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesJson));

            //4.Import Categories - Products
            //string categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));

            //5.Export products in range
            //Console.WriteLine(GetProductsInRange(context));

            //6.Export sold products
            //Console.WriteLine(GetSoldProducts(context));

            //7. Export Categories by Product Count
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //8. Export Users and Products
            Console.WriteLine(GetUsersWithProducts(context));


        }


        //1. Import Data
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            //Without AutoMapper
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            if (products != null)
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            return $"Successfully imported {products.Length}";
        }

        //3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            //Some of the names can be null so we must ignore them
            var allCategories = JsonConvert.DeserializeObject<Category[]>(inputJson);

            var validCategories = allCategories?
                .Where(c => c.Name != null)
                .ToArray();

            if (validCategories != null)
            {
                context.Categories.AddRange(validCategories);
                context.SaveChanges();
                return $"Successfully imported {validCategories.Length}";
            }

            return $"Successfully imported 0";
        }

        //4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        //5. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}"

                })
                .OrderBy(p => p.price)
                .ToArray();

            var json = JsonConvert.SerializeObject(productsInRange, Formatting.Indented);

            return json;
        }

        //6. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var userWithSoldProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer!.FirstName,
                            buyerLastName = p.Buyer.LastName
                        }).ToArray()
                })
                .ToArray();

            string jsonOutput = JsonConvert.SerializeObject(userWithSoldProducts, Formatting.Indented);
            return jsonOutput;
        }

        //7. Export Categories by Product Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoriesByProductCount = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = c.CategoriesProducts
                        .Average(cp => cp.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoriesProducts
                        .Sum(cp => cp.Product.Price).ToString("f2")

                }).OrderByDescending(x => x.productsCount)
                .ToArray();

            return JsonConvert.SerializeObject(categoriesByProductCount, Formatting.Indented);
        }

        //8. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        }).ToArray()

                }).OrderByDescending(u => u.soldProducts.Count())
                .ToArray();

            var output = new
            {
                usersCount = usersWithProducts.Count(),
                users = usersWithProducts.Select(u => new
                {
                    u.firstName,
                    u.lastName,
                    u.age,
                    soldProducts = new
                    {
                        count = u.soldProducts.Count(),
                        products = u.soldProducts
                    }
                })
            };

            string jsonOutput = JsonConvert.SerializeObject(output, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            });

            return jsonOutput;
        }
    }
}