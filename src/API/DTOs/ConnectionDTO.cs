namespace API.DTOs;

public record ConnectionDTO(long id, CityDTO Origin, CityDTO Destination, decimal Value);
