using System.Data;
using Microsoft.Data.Sqlite;

namespace DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        
        IDbConnection connection = new SqliteConnection("DataSource=db.sqlite");
        connection.Open();
        DbConnectionWrapper dbConnectionWrapper = new DbConnectionWrapper(connection);
        ProductService productService = new ProductService(dbConnectionWrapper);
        UserService userService = new UserService(dbConnectionWrapper);

        CommandProcessor commandProcessor = new CommandProcessor(
            true,
            Console.In,
            Console.Out,
            productService,
            userService
        );
        commandProcessor.Process();
    }

}