namespace Domain.Entities;

public class Route(City origin, City destination, decimal value)
{
    public City Origin { get; } = origin;
    public City Destination { get; } = destination;
    public decimal Value { get; set; } = value;
    public List<Connection> Connections { get; set; } = [];

    public void AddConnection(Connection connection)
    {
        Connections.Add(connection);
        Value += connection.Value;
    }

    public void AddConnections(IEnumerable<Connection> connections)
    {
        foreach (var connection in connections)
        {
            Connections.Add(connection);
            Value += connection.Value;
        }
    }

    public void RemoveConnection(Connection connection)
    {
        Connections.Remove(connection);
        Value -= connection.Value;
    }

    public void ReplaceConnections(ICollection<Connection> connections)
    {
        Value = connections.Sum(connection => connection.Value);
        Connections = new List<Connection>(connections);
    }
}
