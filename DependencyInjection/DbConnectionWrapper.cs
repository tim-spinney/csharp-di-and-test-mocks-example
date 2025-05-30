using System.Data;
using Microsoft.Data.Sqlite;

namespace DependencyInjection;

public class DbConnectionWrapper : IDbConnectionWrapper
{
    private IDbConnection _connection;

    public DbConnectionWrapper(IDbConnection connection)
    {
        _connection = connection;
    }

    public int ExecuteStatement(string query, Dictionary<string, object>? parameters = null)
    {
        IDbCommand command = CreateCommand(query, parameters);
        return command.ExecuteNonQuery();
    }

    public IDataReader ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
    {
        IDbCommand command = CreateCommand(query, parameters);
        return command.ExecuteReader();
    }

    private IDbCommand CreateCommand(string query, Dictionary<string, object>? parameters = null)
    {
        IDbCommand command = _connection.CreateCommand();
        command.CommandText = query;
        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new SqliteParameter(parameter.Key, parameter.Value));
            }
        }

        return command;
    }
}