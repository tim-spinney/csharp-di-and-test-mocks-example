namespace DependencyInjection;

public interface IUserService
{
    public int GetBalance(int userId);
    public bool UpdateShippingAddress(int userId, string shippingAddress);
}