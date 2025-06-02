using System.Data;

namespace DependencyInjection;

public class ProductService
{
    private IDbConnectionWrapper _dbConnectionWrapper;

    public ProductService(IDbConnectionWrapper dbConnectionWrapper)
    {
        _dbConnectionWrapper = dbConnectionWrapper;
    }
    
    public IEnumerable<Product> FindInStock()
    {
        using IDataReader reader = _dbConnectionWrapper.ExecuteQuery("SELECT * FROM products WHERE quantity > 0");
        List<Product> products = new List<Product>();
        while (reader.Read())
        {
            products.Add(ReadProduct(reader));
        }
        return products;
    }

    public IEnumerable<Product> FindByPrice(int price)
    {
        using IDataReader reader = _dbConnectionWrapper.ExecuteQuery(
            "SELECT * FROM products WHERE price <= $price",
            new Dictionary<string, object> { { "$price", price } }
        );
        List<Product> products = new List<Product>();
        while (reader.Read())
        {
            products.Add(ReadProduct(reader));
        }
        return products;
    }

    public void PurchaseProduct(int productId, int quantity)
    {
        int numRowsAffected = _dbConnectionWrapper.ExecuteStatement(
            "UPDATE products SET quantity = quantity - $quantity WHERE id = $id and quantity >= $quantity",
            new Dictionary<string, object> { { "$quantity", quantity }, { "$id", productId } }
        );
        if (numRowsAffected == 0)
        {
            throw new TransactionFailedException("We're sorry, we do not have enough of product " + productId + " to complete your transaction.");
        }
    }

    private Product ReadProduct(IDataReader reader)
    {
        return new Product(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3));
    }
}