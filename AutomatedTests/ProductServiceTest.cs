using DependencyInjection;
using Moq;

namespace AutomatedTests;

[TestClass]
public class ProductServiceTest
{
    [TestMethod("Purchasing a product decreases its quantity")]
    public void PurchasingProductDecreasesQuantity()
    {
        // Arrange
        Mock<IDbConnectionWrapper> mockDbConnectionWrapper = new Mock<IDbConnectionWrapper>();
        mockDbConnectionWrapper
            .Setup(m => m.ExecuteStatement(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(1);
        ProductService productService = new ProductService(mockDbConnectionWrapper.Object);
        
        // Act
        productService.PurchaseProduct(4, 1);
        
        // Assert
        Dictionary<string, object> paramsMatcher = It.Is<Dictionary<string, object>>(
            p => (int)p["$id"] == 4 && (int)p["$quantity"] == 1
        );
        mockDbConnectionWrapper
            .Verify(m => m.ExecuteStatement(It.IsAny<string>(), paramsMatcher));
    }

    [TestMethod("Purchasing a non-existent product results in an exception")]
    public void PurchasingNonExistentProductResultsInAnException()
    {
        // Arrange
        Mock<IDbConnectionWrapper> mockDbConnectionWrapper = new Mock<IDbConnectionWrapper>();
        mockDbConnectionWrapper
            .Setup(m => m.ExecuteStatement(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(0);
        ProductService productService = new ProductService(mockDbConnectionWrapper.Object);
        
        // Act and Assert - Act needs to be wrapped in Assert when expecting an exception 
        Assert.ThrowsException<TransactionFailedException>(() => productService.PurchaseProduct(1, 1));
    }
}