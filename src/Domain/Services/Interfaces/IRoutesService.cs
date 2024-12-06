using Domain.Entities;

namespace Domain.Services.Interfaces;

public interface IRoutesService
{
    Task<Connection?> GetConnectionAsync(long id);
    Task<IEnumerable<Connection>> GetConnectionsAsync(string from, string to);
    Task InsertConnectionsAsync(IEnumerable<Connection> connections);
    Task UpdateConnectionsAsync(IEnumerable<Connection> connections);
    Task DeleteConnectionAsync(long id);
    Task<Route> GetOptimalRouteAsync(string from, string to);
}
