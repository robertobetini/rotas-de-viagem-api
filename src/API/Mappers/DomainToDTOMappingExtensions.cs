using API.DTOs;
using Domain.Entities;
using Route = Domain.Entities.Route;

namespace API.Mappers;

public static class DomainToDTOMappingExtensions
{
    public static CityDTO ToDTO(this City city)
    {
        return new CityDTO(city.Name);
    }

    public static ConnectionDTO ToDTO(this Connection connection)
    {
        return new ConnectionDTO(connection.Id, connection.Origin.ToDTO(), connection.Destination.ToDTO(), connection.Value);
    }

    public static RouteDTO ToDTO(this Route route)
    {
        return new RouteDTO(
            route.Origin.ToDTO(), 
            route.Destination.ToDTO(), 
            route.Value, 
            route.Connections.Select(connection => connection.ToDTO()));
    }
}
