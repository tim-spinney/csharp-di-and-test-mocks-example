namespace DependencyInjection;

public class CommandProcessor
{
    private bool _printPrompt;
    private TextReader _in;
    private TextWriter _out;
    private ProductService _productService;
    private UserService _userService;
    private bool _exitRequested = false;
    private int? _userId;

    public CommandProcessor(bool printPrompt, TextReader reader, TextWriter writer, ProductService productService, UserService userService)
    {
        _printPrompt = printPrompt;
        _in = reader;
        _out = writer;
        _productService = productService;
        _userService = userService;
    }

    public void Process()
    {
        while (!_exitRequested)
        {
            if (_printPrompt)
            {
                _out.WriteLine("""
                               Options:
                               1. List in-stock products
                               2. List all products below a specific price
                               3. Purchase a product by id
                               4. Switch user
                               5. Get current wallet balance
                               6. Update shipping address
                               7. Exit
                               """);
            }

            String[] inputs = _in.ReadLine()!.Split(' ');
            int option = int.Parse(inputs[0]);
            switch (option)
            {
                case 1:
                {
                    foreach (Product product in _productService.FindInStock())
                    {
                        _out.WriteLine(product);
                    }

                    break;
                }
                case 2:
                {
                    int price = int.Parse(inputs[1]);
                    foreach (Product product in _productService.FindByPrice(price))
                    {
                        _out.WriteLine(product);
                    }
                    break;
                }
                case 3:
                {
                    int productId = int.Parse(inputs[1]);
                    int quantity = int.Parse(inputs[2]);
                    try
                    {
                        _productService.PurchaseProduct(productId, quantity);
                    }
                    catch (TransactionFailedException ex)
                    {
                        _out.WriteLine(ex.Message);
                    }
                    break;
                }
                case 4:
                {
                    _userId = int.Parse(inputs[1]);
                    break;
                }
                case 5:
                {
                    if (_userId == null)
                    {
                        _out.WriteLine("Please log in with 'switch user' first.");
                        return;
                    }

                    try
                    {
                        int balance = _userService.GetBalance((int)_userId);
                        _out.WriteLine("Your balance is: " + balance);
                    }
                    catch (UnknownUserException ex)
                    {
                        _out.WriteLine(ex.Message);
                    }

                    break;
                }
                case 6:
                {
                    if (_userId == null)
                    {
                        _out.WriteLine("Please log in with 'switch user' first.");
                        return;
                    }
                    bool succeeded = _userService.UpdateShippingAddress((int)_userId, inputs[1]);
                    if (succeeded)
                    {
                        _out.WriteLine("Your shipping address has been updated.");
                    }
                    else
                    {
                        _out.WriteLine("We're sorry, something went wrong. Please try again later even though that probably won't do anything.");
                    }
                    break;
                }
                case 7:
                {
                    _exitRequested = true;
                    break;
                }
            }
        }
    }
    
}