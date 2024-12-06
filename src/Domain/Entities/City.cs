namespace Domain.Entities;

public class City(string name)
{
    public string Name { get; } = name;
    public ICollection<City> Neighbors { get; } = [];
}
