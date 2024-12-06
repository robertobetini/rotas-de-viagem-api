namespace Domain.Entities;

public class RouteTree(City root)
{
    public City Root { get; } = root;
    private readonly Dictionary<string, City> _cityCache = [];

    public City? GetCity(string name)
    {
        if (_cityCache.TryGetValue(name, out var city))
        {
            return city;
        }

        var visitedCities = new List<City>();
        return SearchCity(Root, name, visitedCities);
    }

    private City? SearchCity(City start, string name, List<City> visitedCities)
    {
        if (visitedCities.Contains(start))
        {
            return default;
        }
        visitedCities.Add(start);

        if (start.Name == name)
        {
            _cityCache.Add(name, start);
            return start;
        }

        if (start.Neighbors == null || !start.Neighbors.Any())
        {
            return default;
        }

        foreach (var neighbor in start.Neighbors)
        {
            var city = SearchCity(neighbor, name, visitedCities);
            if (city != default)
            {
                return city;
            }
        }

        return default;
    }
}
