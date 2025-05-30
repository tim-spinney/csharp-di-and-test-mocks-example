using System.Data;
using Microsoft.Data.Sqlite;

namespace DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        Database db = new Database();
        
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
                    db.FindInStock();
                    break;
                }
                case 2:
                {
                    db.FindByPrice(inputs);
                    break;
                }
                case 3:
                {
                    db.PurchaseProduct(inputs);
                    break;
                }
                case 4:
                {
                    userId = int.Parse(inputs[1]);
                    break;
                }
                case 5:
                {
                    db.GetBalance(userId);
                    break;
                }
                case 6:
                {
                    db.UpdateShippingAddress(userId, inputs);
                    break;
                }
            }
        }
    }

}