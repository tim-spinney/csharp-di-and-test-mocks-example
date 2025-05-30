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
        while (option != 4)
        {
            Console.WriteLine("""
                              Options:
                              1. List in-stock products
                              2. List all products below a specific price
                              3. Purchase a product by id
                              4. Exit
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
            }
        }
    }
}