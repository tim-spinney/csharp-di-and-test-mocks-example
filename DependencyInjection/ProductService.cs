using System.Data;

namespace DependencyInjection;

public class ProductService
{
    private IDbConnectionWrapper _dbConnectionWrapper;

    public ProductService(IDbConnectionWrapper dbConnectionWrapper)
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

}