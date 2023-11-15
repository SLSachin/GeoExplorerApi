using GeoExplorerApi.Dtos;
using GeoExplorerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeoExplorerApi.Interfaces{
public interface ILocationService
{
    Task<ActionResult<IEnumerable<StateDto>>> GetStatesAsync();
    Task<IActionResult> AddStateAsync(StateDto stateDto);
    Task<ActionResult> DeleteStateAsync(int id);
    Task<ActionResult<IEnumerable<LocationDto>>> GetLocationsByStateIdAsync(int id);
    Task<ActionResult<LocationDto>> AddLocationAsync(LocationDto locationDto);
    Task<ActionResult<LocationDto>> UpdateLocationAsync(int id, LocationDto locationDto);
    Task<ActionResult> DeleteLocationAsync(int id);
}
}
