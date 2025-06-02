namespace DependencyInjection;

public class TransactionFailedException : Exception
{
    public TransactionFailedException()
    {
    }

    public TransactionFailedException(string? message) : base(message)
    {
    }

    public TransactionFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}