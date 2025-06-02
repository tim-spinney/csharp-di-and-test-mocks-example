using System.Data;
using Microsoft.Data.Sqlite;

namespace DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        IDbConnection connection = new SqliteConnection("DataSource=db.sqlite");
        connection.Open();
        DbConnectionWrapper dbConnectionWrapper = new DbConnectionWrapper(connection);
        ProductService productService = new ProductService(dbConnectionWrapper);
        UserService userService = new UserService(dbConnectionWrapper);
        
        int option = -1;
        int? userId = null;
        while (option != 7)
        {
            Console.WriteLine("""
                              Options:
                              1. List in-stock products
                              2. List all products below a specific price
                              3. Purchase a product by id
                              4. Switch user
                              5. Get current wallet balance
                              6. Update shipping address
                              7. Exit
                              """);
            String[] inputs = Console.ReadLine().Split(' ');
            option = int.Parse(inputs[0]);
            switch (option)
            {
                case 1:
                {
                    foreach (Product product in productService.FindInStock())
                    {
                        Console.WriteLine(product);
                    }

                    break;
                }
                case 2:
                {
                    int price = int.Parse(inputs[1]);
                    foreach (Product product in productService.FindByPrice(price))
                    {
                        Console.WriteLine(product);
                    }
                    break;
                }
                case 3:
                {
                    int productId = int.Parse(inputs[1]);
                    int quantity = int.Parse(inputs[2]);
                    try
                    {
                        productService.PurchaseProduct(productId, quantity);
                    }
                    catch (TransactionFailedException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                }
                case 4:
                {
                    userId = int.Parse(inputs[1]);
                    break;
                }
                case 5:
                {
                    if (userId == null)
                    {
                        Console.WriteLine("Please log in with 'switch user' first.");
                        return;
                    }

                    try
                    {
                        int balance = userService.GetBalance((int)userId);
                        Console.WriteLine("Your balance is: " + balance);
                    }
                    catch (UnknownUserException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;
                }
                case 6:
                {
                    if (userId == null)
                    {
                        Console.WriteLine("Please log in with 'switch user' first.");
                        return;
                    }
                    bool succeeded = userService.UpdateShippingAddress((int)userId, inputs[1]);
                    if (succeeded)
                    {
                        Console.WriteLine("Your shipping address has been updated.");
                    }
                    else
                    {
                        Console.WriteLine("We're sorry, something went wrong. Please try again later even though that probably won't do anything.");
                    }
                    break;
                }
            }
        }
    }

}