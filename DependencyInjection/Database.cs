using System.Data;
using Microsoft.Data.Sqlite;

namespace DependencyInjection;

public class Database
{
    private IDbConnection _connection;

    public Database()
    {
        _connection = new SqliteConnection("Data Source=db.sqlite");
        _connection.Open();
    }
    
    public void FindInStock()
    {
        IDbCommand inStock = _connection.CreateCommand();
        inStock.CommandText = "SELECT * FROM products WHERE quantity > 0";
        using IDataReader reader = inStock.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(0) + "\t" + reader.GetString(1) + ": " + reader.GetInt32(3) +
                              "@" + reader.GetInt32(2));
        }
    }
    
    public void FindByPrice(string[] inputs)
    {
        IDbCommand belowPrice = _connection.CreateCommand();
        int price = int.Parse(inputs[1]);
        belowPrice.CommandText = "SELECT * FROM products WHERE price <= $price";
        belowPrice.Parameters.Add(new SqliteParameter("$price", price));
        using IDataReader reader = belowPrice.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(0) + "\t" + reader.GetString(1) + ": " + reader.GetInt32(3) +
                              "@" + reader.GetInt32(2));
        }
    }

    public void PurchaseProduct(string[] inputs)
    {
        IDbCommand purchase = _connection.CreateCommand();
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
    }

    public void GetBalance(int? userId)
    {
        if (userId == null)
        {
            Console.WriteLine("Please log in with 'switch user' first.");
            return;
        }
        IDbCommand getBalance = _connection.CreateCommand();
        getBalance.CommandText = "SELECT balance FROM wallets WHERE user_id = $user_id";
        getBalance.Parameters.Add(new SqliteParameter("$user_id", userId));
        using IDataReader reader = getBalance.ExecuteReader();
        if (reader.Read())
        {
            Console.WriteLine("Current balance: " + reader.GetString(0));
        }
    }

    public void UpdateShippingAddress(int? userId, string[] inputs)
    {
        if (userId == null)
        {
            Console.WriteLine("Please log in with 'switch user' first.");
            return;
        }
        IDbCommand updateShippingAddress = _connection.CreateCommand();
        updateShippingAddress.CommandText = "UPDATE users SET shipping_address = $shippingAddress WHERE id = $userId";
        updateShippingAddress.Parameters.Add(new SqliteParameter("$shippingAddress", inputs[1]));
        updateShippingAddress.Parameters.Add(new SqliteParameter("$userId", userId));
        updateShippingAddress.ExecuteNonQuery();
    }

}