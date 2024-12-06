using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface IRoutesRepository
{
    Task<Connection?> GetConnectionAsync(long id);
    Task<Connection?> GetConnectionAsync(string from, string to);
    Task<IEnumerable<Connection>> GetConnectionsAsync();
    Task<IEnumerable<Connection>> GetConnectionsByOriginAsync(string name);
    Task<IEnumerable<Connection>> GetConnectionsByDestinationAsync(string name);
    Task InsertConnectionsAsync(IEnumerable<Connection> connections);
    Task UpdateConnectionsAsync(IEnumerable<Connection> connections);
    Task DeleteConnectionAsync(long id);
}
