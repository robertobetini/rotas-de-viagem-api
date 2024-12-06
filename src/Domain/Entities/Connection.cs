namespace Domain.Entities;

public class Connection(City origin, City destination, decimal value)
{
    public long Id { get; set; } = 0;
    public City Origin { get; } = origin;
    public City Destination { get; } = destination;
    public decimal Value { get; set; } = value;

    public Connection(string originName, string destinationName, decimal value)
        : this(new City(originName), new City(destinationName), value) { }
}
