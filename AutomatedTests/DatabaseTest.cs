using DependencyInjection;
using Moq;

namespace AutomatedTests;

[TestClass]
public class DatabaseTest
{
    [TestMethod("Purchasing a product decreases its quantity")]
    public void PurchasingProductDecreasesQuantity()
    {
        // Arrange
        Mock<IDbConnectionWrapper> mockDbConnectionWrapper = new Mock<IDbConnectionWrapper>();
        mockDbConnectionWrapper
            .Setup(m => m.ExecuteStatement(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(1);
        Database database = new Database(mockDbConnectionWrapper.Object);
        
        // Act
        database.PurchaseProduct(4, 1);
        
        // Assert
        Dictionary<string, object> paramsMatcher = It.Is<Dictionary<string, object>>(
            p => (int)p["$id"] == 4 && (int)p["$quantity"] == 1
        );
        mockDbConnectionWrapper
            .Verify(m => m.ExecuteStatement(It.IsAny<string>(), paramsMatcher));
    }
}