using Domain.Entities;

namespace Domain.Tests.Entities;

public class RouteTreeTests
{
    private readonly RouteTree _sut = new(new City("ROOT"));

    public RouteTreeTests()
    {
        for (var i = 0; i < 10; i++)
        {
            var city = new City(i.ToString());
            _sut.Root.Neighbors.Add(city);
        }

        // Root tem 10 vizinhos, e cada vizinho tem mais 10 vizinhos
        foreach (var neighbor in _sut.Root.Neighbors)
        {
            for (var i = 0; i < 10; i++)
            {
                var city = new City(neighbor.Name + i.ToString());
                neighbor.Neighbors.Add(city);
            }
        }
    }

    [Fact]
    public void Given_NodeIsInTree_When_SearchingRouteTree_Then_ReturnExistingNode()
    {
        // Act
        var city = _sut.GetCity("15");

        // Assert
        Assert.NotNull(city);
    }

    [Fact]
    public void Given_NodeIsNotInTree_When_SearchingRouteTree_Then_ReturnExistingNode()
    {
        // Act
        var city = _sut.GetCity("non-existent node");

        // Assert
        Assert.Null(city);
    }
}
