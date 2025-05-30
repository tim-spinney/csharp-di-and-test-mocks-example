using System.Data;
using Microsoft.Data.Sqlite;

namespace DependencyInjection;

public class Database
{
    private IDbConnectionWrapper _dbConnectionWrapper;

    public Database(IDbConnectionWrapper dbConnectionWrapper)
    {
        _dbConnectionWrapper = dbConnectionWrapper;
    }
    
    public void FindInStock()
    {
        using IDataReader reader = _dbConnectionWrapper.ExecuteQuery("SELECT * FROM products WHERE quantity > 0");
        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(0) + "\t" + reader.GetString(1) + ": " + reader.GetInt32(3) +
                              "@" + reader.GetInt32(2));
        }
    }
    
    public void FindByPrice(int price)
    {
        using IDataReader reader = _dbConnectionWrapper.ExecuteQuery(
            "SELECT * FROM products WHERE price <= $price",
            new Dictionary<string, object> { { "$price", price } }
        );
        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(0) + "\t" + reader.GetString(1) + ": " + reader.GetInt32(3) +
                              "@" + reader.GetInt32(2));
        }
    }

    public void PurchaseProduct(int productId, int quantity)
    {
        int numRowsAffected = _dbConnectionWrapper.ExecuteStatement(
            "UPDATE products SET quantity = quantity - $quantity WHERE id = $id and quantity >= $quantity",
            new Dictionary<string, object> { { "$quantity", quantity }, { "$id", productId } }
        );
        if (numRowsAffected == 0)
        {
            Console.WriteLine("We're sorry, we do not have enough of product " + productId + " to complete your transaction.");
        }
    }

    public void GetBalance(int userId)
    {
        using IDataReader reader = _dbConnectionWrapper.ExecuteQuery(
            "SELECT balance FROM wallets WHERE user_id = $user_id",
            new Dictionary<string, object> { { "$user_id", userId } }
        );
        if (reader.Read())
        {
            Console.WriteLine("Current balance: " + reader.GetString(0));
        }
    }

    public void UpdateShippingAddress(int userId, string shippingAddress)
    {
        _dbConnectionWrapper.ExecuteStatement(
            "UPDATE users SET shipping_address = $shippingAddress WHERE id = $userId",
            new Dictionary<string, object> { { "$shipping_address", shippingAddress }, { "$userId", userId } }
        );
    }

}