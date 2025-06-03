namespace DependencyInjection;

public interface IProductService
{
    public IEnumerable<Product> FindInStock();
    IEnumerable<Product> FindByPrice(int price);
    public void PurchaseProduct(int productId, int quantity);
}