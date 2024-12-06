namespace API.DTOs;

public record RouteDTO(CityDTO Origin, CityDTO Destination, decimal Value, IEnumerable<ConnectionDTO> Connections);
