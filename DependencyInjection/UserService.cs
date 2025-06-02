using System.Data;

namespace DependencyInjection;

public class UserService
{
    private IDbConnectionWrapper _dbConnectionWrapper;

    public UserService(IDbConnectionWrapper dbConnectionWrapper)
    {
        _dbConnectionWrapper = dbConnectionWrapper;
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