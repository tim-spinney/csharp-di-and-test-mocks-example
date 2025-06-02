namespace DependencyInjection;

public record Product(
    int Id,
    string Description,
    int Price,
    int Quantity
);