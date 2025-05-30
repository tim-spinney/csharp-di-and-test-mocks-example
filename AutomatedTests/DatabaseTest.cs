using DependencyInjection;

namespace AutomatedTests;

[TestClass]
public class DatabaseTest
{
    [TestMethod("Purchasing a product decreases its quantity")]
    public void PurchasingProductDecreasesQuantity()
    {
        // Arrange
        Database database = new Database();
        
        // Act
        database.PurchaseProduct(1, 1);
        
        // Assert
        database.FindInStock();
        // Wait... we can't see the output it wrote to the console
    }

    [TestMethod("Trying to purchase more of a product than is in stock fails")]
    public void PurchasingTooManyFails()
    {
        // Arrange
        Database database = new Database();
        
        // Act
        database.PurchaseProduct(1, 10);
        
        // Uhh... how do we assert?
    }
}