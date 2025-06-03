using System.Data;

namespace DependencyInjection;

public class UserService : IUserService
{
    private IDbConnectionWrapper _dbConnectionWrapper;

    public UserService(IDbConnectionWrapper dbConnectionWrapper)
    {
        _dbConnectionWrapper = dbConnectionWrapper;
    }
    
    public int GetBalance(int userId)
    {
        using IDataReader reader = _dbConnectionWrapper.ExecuteQuery(
            "SELECT balance FROM wallets WHERE user_id = $user_id",
            new Dictionary<string, object> { { "$user_id", userId } }
        );
        if (reader.Read())
        {
            return reader.GetInt32(0);
        }
        throw new UnknownUserException(userId);
    }

    public bool UpdateShippingAddress(int userId, string shippingAddress)
    {
        int rowsAffected = _dbConnectionWrapper.ExecuteStatement(
            "UPDATE users SET shipping_address = $shippingAddress WHERE id = $userId",
            new Dictionary<string, object> { { "$shipping_address", shippingAddress }, { "$userId", userId } }
        );
        return rowsAffected > 0;
    }
}