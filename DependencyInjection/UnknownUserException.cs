namespace DependencyInjection;

public class UnknownUserException : Exception
{
    public UnknownUserException(int userId) : base("Unknown User " + userId) { }
}