using System.Data;
using Microsoft.Data.Sqlite;

namespace DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        using IDbConnection connection = new SqliteConnection("Data Source=db.sqlite");
        connection.Open();
        
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
                    IDbCommand inStock = connection.CreateCommand();
                    inStock.CommandText = "SELECT * FROM products WHERE quantity > 0";
                    using IDataReader reader = inStock.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0) + "\t" + reader.GetString(1) + ": " + reader.GetInt32(3) +
                                          "@" + reader.GetInt32(2));
                    }

                    break;
                }
                case 2:
                {
                    IDbCommand belowPrice = connection.CreateCommand();
                    int price = int.Parse(inputs[1]);
                    belowPrice.CommandText = "SELECT * FROM products WHERE price <= $price";
                    belowPrice.Parameters.Add(new SqliteParameter("$price", price));
                    using IDataReader reader = belowPrice.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0) + "\t" + reader.GetString(1) + ": " + reader.GetInt32(3) +
                                          "@" + reader.GetInt32(2));
                    }
                    break;
                }
                case 3:
                {
                    IDbCommand purchase = connection.CreateCommand();
                    int productId = int.Parse(inputs[1]);
                    int quantity = int.Parse(inputs[2]);
                    purchase.CommandText = "UPDATE products SET quantity = quantity - $quantity WHERE id = $id and quantity >= $quantity";
                    purchase.Parameters.Add(new SqliteParameter("$quantity", quantity));
                    purchase.Parameters.Add(new SqliteParameter("$id", productId));
                    int numRowsAffected = purchase.ExecuteNonQuery();
                    if (numRowsAffected == 0)
                    {
                        Console.WriteLine("We're sorry, we do not have enough of product " + productId + " to complete your transaction.");
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
                        break;
                    }
                    IDbCommand getBalance = connection.CreateCommand();
                    getBalance.CommandText = "SELECT balance FROM wallets WHERE user_id = $user_id";
                    getBalance.Parameters.Add(new SqliteParameter("$user_id", userId));
                    using IDataReader reader = getBalance.ExecuteReader();
                    if (reader.Read())
                    {
                        Console.WriteLine("Current balance: " + reader.GetString(0));
                    }
                    break;
                }
                case 6:
                {
                    if (userId == null)
                    {
                        Console.WriteLine("Please log in with 'switch user' first.");
                        break;
                    }
                    IDbCommand updateShippingAddress = connection.CreateCommand();
                    updateShippingAddress.CommandText = "UPDATE users SET shipping_address = $shippingAddress WHERE id = $userId";
                    updateShippingAddress.Parameters.Add(new SqliteParameter("$shippingAddress", inputs[1]));
                    updateShippingAddress.Parameters.Add(new SqliteParameter("$userId", userId));
                    updateShippingAddress.ExecuteNonQuery();
                    break;
                }
            }
        }
    }
}