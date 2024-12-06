using Domain.Entities;
using Domain.Repositories.Interfaces;

namespace Infrastructure.Repositories.Strategies;

public class MemoryRoutesRepository : IRoutesRepository
{
    private readonly List<Connection> _connections = [];

    public Task DeleteConnectionAsync(long id)
    {
        _connections.RemoveAll(connection => connection.Id == id);

        return Task.CompletedTask;
    }

    public Task<Connection?> GetConnectionAsync(long id)
    {
        return Task.FromResult(_connections.FirstOrDefault(connection => connection.Id == id));
    }

    public Task<Connection?> GetConnectionAsync(string originName, string destinationName)
    {
        var connection = _connections.FirstOrDefault(connection =>
            connection.Origin.Name == originName
            && connection.Destination.Name == destinationName);

        return Task.FromResult(connection);
    }

    public Task<IEnumerable<Connection>> GetConnectionsByOriginAsync(string from)
    {
        return Task.FromResult(_connections.Where(connection => connection.Origin.Name == from));
    }

    public Task<IEnumerable<Connection>> GetConnectionsByDestinationAsync(string to)
    {
        return Task.FromResult(_connections.Where(connection => connection.Destination.Name == to));
    }

    public Task InsertConnectionsAsync(IEnumerable<Connection> connections)
    {
        foreach (var connection in connections)
        {
            var foundConnection = FindSingleConnection(connection.Origin, connection.Destination);
            if (foundConnection != null)
            {
                continue;
            }

            connection.Id = _connections.Count + 1;
            _connections.Add(connection);
        }

        return Task.CompletedTask;
    }

    public Task UpdateConnectionsAsync(IEnumerable<Connection> connections)
    {
        foreach (var connection in connections)
        {
            var foundConnection = FindSingleConnection(connection.Origin, connection.Destination);
            if (foundConnection != null)
            {
                foundConnection.Value = connection.Value;
            }
        }

        return Task.CompletedTask;
    }

    private Connection? FindSingleConnection(City origin, City destination)
    {
        return _connections.FirstOrDefault(c => c.Origin.Name == origin.Name && c.Destination.Name == destination.Name);
    }

    public Task<IEnumerable<Connection>> GetConnectionsAsync()
    {
        return Task.FromResult(_connections.ToArray() as IEnumerable<Connection>);
    }
}
