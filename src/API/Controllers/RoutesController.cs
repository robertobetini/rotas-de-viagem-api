using API.DTOs;
using API.Mappers;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[Controller]")]
public class RoutesController(IRoutesService routesService) : Controller
{
    private readonly IRoutesService _routesService = routesService;

    [HttpGet]
    public async Task<IActionResult> GetRoutes([FromQuery] string from, [FromQuery] string to)
    {
        var connections = await _routesService.GetConnectionsAsync(from, to);
        var response = connections.Select(connection => connection.ToDTO());

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> InsertRoutes([FromBody] IEnumerable<ConnectionDTO> connectionDTOs)
    {
        if (!connectionDTOs.Any())
        {
            return BadRequest();
        }

        var connections = connectionDTOs.Select(dto => dto.ToDomainEntity());
        await _routesService.InsertConnectionsAsync(connections);

        return Created();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteRoutes(long id)
    {
        var existingConnection = await _routesService.GetConnectionAsync(id);
        if (existingConnection is null)
        {
            return NotFound();
        }

        await _routesService.DeleteConnectionAsync(id);

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoutes([FromBody] IEnumerable<ConnectionDTO> connectionDTOs)
    {
        if (!connectionDTOs.Any())
        {
            return BadRequest();
        }

        var connections = connectionDTOs.Select(dto => dto.ToDomainEntity());
        await _routesService.UpdateConnectionsAsync(connections);

        return NoContent();
    }

    [HttpGet]
    [Route("optimal")]
    public async Task<IActionResult> GetOptimalRoute([FromQuery] string from, [FromQuery] string to)
    {
        var route = await _routesService.GetOptimalRouteAsync(from, to);
        if (route is null)
        {
            return NotFound();
        }

        var response = route.ToDTO();

        return Ok(response);
    }
}
