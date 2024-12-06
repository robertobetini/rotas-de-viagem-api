using API.DTOs;
using Domain.Entities;

namespace API.Mappers;

public static class DTOToDomainMappingExtensions
{
    public static City ToDomainEntity(this CityDTO dto)
    {
        return new City(dto.Name);
    }

    public static Connection ToDomainEntity(this ConnectionDTO dto)
    {
        return new Connection(dto.Origin.ToDomainEntity(), dto.Destination.ToDomainEntity(), dto.Value);
    }
}
