using Microsoft.AspNetCore.Mvc;
using GeoExplorerApi.Models;
using Microsoft.AspNetCore.Authorization;
using GeoExplorerApi.Dtos;
using GeoExplorerApi.Interfaces;

namespace GeoExplorerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [Route("state")]
        public async Task<ActionResult<IEnumerable<StateDto>>> GetStates()
        {
            try
            {
                var okObjectResult = await _locationService.GetStatesAsync();
                if (okObjectResult == null || okObjectResult.Result == null)
                {
                    return NotFound();
                }
                return okObjectResult.Result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("state")]
        public async Task<IActionResult> AddState(StateDto stateDto)
        {
            try
            {
                var result = await _locationService.AddStateAsync(stateDto);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return StatusCode(500, "Internal Server Error");
            }
        }

        // [HttpDelete("{id}")]
        // [Route("state")]
        // [Authorize(Roles = "Admin")]
        // public async Task<ActionResult> DeleteState(int id)
        // {
        //     try
        //     {
        //         var result = await _locationService.DeleteStateAsync(id);
        //         return result;
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception or handle it in a way appropriate for your application
        //         return StatusCode(500, "Internal Server Error");
        //     }
        // }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocationsByStateId(int id)
        {
            try
            {
                var result = await _locationService.GetLocationsByStateIdAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<LocationDto>> AddLocation([FromBody] LocationDto locationDto)
        {
            try
            {
                var result = await _locationService.AddLocationAsync(locationDto);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<LocationDto>> UpdateLocation(int id, [FromBody] LocationDto locationDto)
        {
            try
            {
                var result = await _locationService.UpdateLocationAsync(id, locationDto);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            try
            {
                var result = await _locationService.DeleteLocationAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
