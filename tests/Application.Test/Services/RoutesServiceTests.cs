using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;
using NSubstitute;

namespace Application.Tests.Services;

public class RoutesServiceTests
{
    private readonly IRoutesRepository _routesRepository;

    private readonly IRoutesService _sut;

    public RoutesServiceTests()
    {
        _routesRepository = Substitute.For<IRoutesRepository>();

        var connections = new List<Connection>
        {
            new("GRU", "BRC", 10),
            new("BRC", "SCL", 5 ),
            new("GRU", "CDG", 75),
            new("GRU", "SCL", 20),
            new("GRU", "ORL", 56),
            new("ORL", "CDG", 5 ),
            new("SCL", "ORL", 20)
        };
        foreach (var connection in connections)
        {
            _routesRepository
                .GetConnectionAsync(connection.Origin.Name, connection.Destination.Name)
                .Returns(connection);

            _routesRepository
                .GetConnectionsByOriginAsync(connection.Origin.Name)
                .Returns(connections.Where(c => c.Origin.Name == connection.Origin.Name));
        }

        _sut = new RoutesService(_routesRepository);
    }

    [Fact]
    public async Task When_Executed_Then_ReturnOptimalRouteWithValueAndConnections()
    {
        // Arrange
        var expectedValue = 40;
        var expectedConnections = new Connection[]
        {
            new("GRU", "BRC", 10),
            new("BRC", "SCL", 5),
            new("SCL", "ORL", 20),
            new("ORL", "CDG", 5)
        };

        // Act
        var route = await _sut.GetOptimalRouteAsync("GRU", "CDG");

        // Assert
        Assert.Equal(expectedValue, route.Value);
        Assert.Equal(expectedConnections.Length, route.Connections.Count);
        for (var i = 0; i < expectedConnections.Length; i++)
        {
            var expected = expectedConnections[i];
            var actual = route.Connections[i];

            Assert.Equal(expected.Origin.Name, actual.Origin.Name);
            Assert.Equal(expected.Destination.Name, actual.Destination.Name);
            Assert.Equal(expected.Value, actual.Value);
        }
    }

    [Fact]
    public async Task When_OriginAndDestinationAreTheSame_Then_ThrowsSameOriginAndDestinationException()
    {
        // Act
        var exception = await Record.ExceptionAsync(async () => await _sut.GetOptimalRouteAsync("GRU", "GRU"));

        // Assert
        Assert.IsType<SameOriginAndDestinationException>(exception);
    }

    [Fact]
    public async Task When_OriginDoesNotExist_Then_ThrowsOriginDoesNotExistException()
    {
        // Act
        var exception = await Record.ExceptionAsync(async () => await _sut.GetOptimalRouteAsync("NON-EXISTENT", "GRU"));

        // Assert
        Assert.IsType<OriginDoesNotExistException>(exception);
    }
}
