using GeoExplorerApi.Dtos;
using GeoExplorerApi.Interfaces;
using GeoExplorerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeoExplorerApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly GeoExplorerContext _context;

        public LocationService(GeoExplorerContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<StateDto>>> GetStatesAsync()
        {
            var states = await _context.States.ToListAsync();
            if (states == null || !states.Any())
            {
                return new NotFoundObjectResult("No states found");
            }

            var stateDtos = states.Select(s => new StateDto
            {
                Id = s.Id,
                Name = s.Name
            });
            return new OkObjectResult(stateDtos);
        }


        public async Task<IActionResult> AddStateAsync(StateDto stateDto)
        {
            if (!ValidateStateDto(stateDto))
            {
                return new BadRequestResult();
            }

            var existingState = await _context.States.FirstOrDefaultAsync(s => s.Name == stateDto.Name);

            if (existingState != null)
            {
                return new BadRequestObjectResult("State already exists");
            }

            var newState = new State
            {
                Name = stateDto.Name
            };

            _context.States.Add(newState);
            await _context.SaveChangesAsync();

            return new OkObjectResult(newState);
        }


        public async Task<ActionResult> DeleteStateAsync(int id)
        {
            try
            {
                var stateToDelete = await _context.States.FindAsync(id);

                if (stateToDelete == null)
                {
                    return new NotFoundObjectResult($"State with Id {id} not found");
                }

                _context.States.Remove(stateToDelete);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocationsByStateIdAsync(int id)
        {
            try
            {
                var state = await _context.States
                    .Include(s => s.Locations) // Include Locations for eager loading
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (state == null || state.Locations == null || !state.Locations.Any())
                {
                    return new NotFoundObjectResult($"No locations found for state with Id {id}");
                }

                var locationDtos = state.Locations.Select(l => new LocationDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Address = l.Address,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    StateId = l.StateId
                });

                return new OkObjectResult(locationDtos);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<LocationDto>> AddLocationAsync(LocationDto locationDto)
        {
            try
            {
                // Assuming locationDto is a DTO (Data Transfer Object) for input validation
                if (locationDto == null)
                {
                    return new BadRequestResult();
                }

                // Check if the specified state exists
                var state = await _context.States.FindAsync(locationDto.StateId);

                if (state == null)
                {
                    return new BadRequestObjectResult($"State with Id {locationDto.StateId} not found");
                }

                // Map the DTO to the Location model
                var newLocation = new Location
                {
                    Title = locationDto.Title,
                    Address = locationDto.Address,
                    Latitude = locationDto.Latitude,
                    Longitude = locationDto.Longitude,
                    StateId = locationDto.StateId
                };

                // Add the new location to the database
                _context.Locations.Add(newLocation);
                await _context.SaveChangesAsync();

                // Return the newly created location
                return new OkObjectResult(newLocation);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return new StatusCodeResult(500);
            }
        }


        public async Task<ActionResult<LocationDto>> UpdateLocationAsync(int id, LocationDto locationDto)
        {
            try
            {
                // Assuming LocationDto is a DTO (Data Transfer Object) for input validation
                if (locationDto == null)
                {
                    return new BadRequestResult();
                }

                // Check if the specified location exists
                var existingLocation = await _context.Locations.FindAsync(id);

                if (existingLocation == null)
                {
                    return new NotFoundObjectResult($"Location with Id {id} not found");
                }

                // Check if the specified state exists
                var state = await _context.States.FindAsync(locationDto.StateId);

                if (state == null)
                {
                    return new BadRequestObjectResult($"State with Id {locationDto.StateId} not found");
                }

                // Update the existing location properties
                existingLocation.Title = locationDto.Title;
                existingLocation.Address = locationDto.Address;
                existingLocation.Latitude = locationDto.Latitude;
                existingLocation.Longitude = locationDto.Longitude;
                existingLocation.StateId = locationDto.StateId;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the updated location
                return new OkObjectResult(locationDto);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult> DeleteLocationAsync(int id)
        {
            try
            {
                // Check if the specified location exists
                var locationToDelete = await _context.Locations.FindAsync(id);

                if (locationToDelete == null)
                {
                    return new NotFoundObjectResult($"Location with Id {id} not found");
                }

                // Remove the location from the database
                _context.Locations.Remove(locationToDelete);
                await _context.SaveChangesAsync();

                // Return a 204 No Content response, indicating successful deletion
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a way appropriate for your application
                return new StatusCodeResult(500);
            }
        }

        private bool ValidateStateDto(StateDto stateDto)
        {
            return stateDto != null && !string.IsNullOrEmpty(stateDto.Name);
        }
    }
}