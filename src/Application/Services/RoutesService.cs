using Application.Exceptions;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;

namespace Application.Services;

public class RoutesService(IRoutesRepository routesRepository) : IRoutesService
{
    private readonly IRoutesRepository _routesRepository = routesRepository;

    public async Task<Connection?> GetConnectionAsync(long id)
    {
        return await _routesRepository.GetConnectionAsync(id);
    }

    public async Task<IEnumerable<Connection>> GetConnectionsAsync(string from, string to)
    {
        if (string.IsNullOrWhiteSpace(from) && string.IsNullOrWhiteSpace(to))
        {
            return await _routesRepository.GetConnectionsAsync();
        }

        var fromConnections = (await _routesRepository.GetConnectionsByOriginAsync(from)).ToList();
        var toConnections = await _routesRepository.GetConnectionsByDestinationAsync(to);

        fromConnections.AddRange(toConnections);

        return fromConnections.DistinctBy(x => x.Id);
    }

    public async Task InsertConnectionsAsync(IEnumerable<Connection> connections)
    {
        await _routesRepository.InsertConnectionsAsync(connections);
    }

    public async Task UpdateConnectionsAsync(IEnumerable<Connection> connections)
    {
        await _routesRepository.UpdateConnectionsAsync(connections);
    }

    public async Task DeleteConnectionAsync(long id)
    {
        await _routesRepository.DeleteConnectionAsync(id);
    }

    public async Task<Route?> GetOptimalRouteAsync(string from, string to)
    {
        if (from == to)
        {
            throw new SameOriginAndDestinationException();
        }

        var root = new City(from);
        var tree = new RouteTree(root);
        var target = new City(to);

        return await MemoizedDepthFirstSearch(root, target, tree, new Dictionary<City, Route>());
    }

    private async Task<Route?> MemoizedDepthFirstSearch(City city, City target, RouteTree tree, IDictionary<City, Route> memo)
    {
        if (city.Name == target.Name)
        {
            return default;
        }

        // se tem cache hit quer dizer que já calculamos rota a partir daqui, apenas retorna o que já foi calculado
        if (memo.TryGetValue(city, out var record))
        {
            return record;
        }

        record = new Route(city, target, decimal.MaxValue);
        
        await PopulateNeighbors(city, tree); // lazy load dos vizinhos, só carregamos o que precisamos
        foreach (var neighbor in city.Neighbors)
        {
            var currentRoute = new Route(city, target, 0);

            var connection = await _routesRepository.GetConnectionAsync(city.Name, neighbor.Name)
                ?? throw new NullConnectionException(city.Name, target.Name);
            var newConnection = new Connection(city, neighbor, connection.Value);
            currentRoute.AddConnection(newConnection);

            // Acho que verificar se a rota atual já está maior/igual ao recorde pode evitar processamento desnecessário
            if (currentRoute.Value >= record.Value)
            {
                continue;
            }

            var localRecord = await MemoizedDepthFirstSearch(neighbor, target, tree, memo);

            // se localRecord é nulo, quer dizer que o vizinho da cidade atual é a cidade que estamos buscando
            if (localRecord != null)
            {
                currentRoute.AddConnections(localRecord.Connections);
            }

            if (currentRoute.Value < record.Value)
            {
                record.ReplaceConnections(currentRoute.Connections);
                memo.Add(city, currentRoute);
            }
        }

        return record;
    }

    private async Task PopulateNeighbors(City city, RouteTree tree)
    {
        var connections = await _routesRepository.GetConnectionsByOriginAsync(city.Name);
        if (!connections.Any())
        {
            throw new OriginDoesNotExistException(city.Name);
        }

        foreach (var connection in connections)
        {
            var existingCity = tree.GetCity(connection.Destination.Name);
            var n = existingCity == default ? connection.Destination : existingCity;
            city.Neighbors.Add(n);
        }
    }
}
