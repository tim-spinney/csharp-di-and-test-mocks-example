using System.Data;

namespace DependencyInjection;

public interface IDbConnectionWrapper
{
    public int ExecuteStatement(string query, Dictionary<string, object>? parameters = null);

    public IDataReader ExecuteQuery(string query, Dictionary<string, object>? parameters = null);
}