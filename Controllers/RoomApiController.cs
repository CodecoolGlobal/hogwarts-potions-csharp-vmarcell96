using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models;
using HogwartsPotions.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HogwartsPotions.Controllers
{
    [ApiController, Route("/room")]
    public class RoomApiController : ControllerBase
    {
        private readonly HogwartsContext _context;
        private readonly ILogger<RoomApiController> _logger;

        public RoomApiController(HogwartsContext context, ILogger<RoomApiController> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            try
            {
                return Ok(await _context.GetAllRooms());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting the list of rooms.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
            
        }

        [HttpPost]
        public async  Task AddRoom([FromBody] Room room)
        {
            await _context.AddRoom(room);
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<Room>> GetRoomById(long id)
        {
            if (id == null || await _context.GetAllRooms() == null)
            {
                return NotFound();
            }

            var room = await _context.GetRoom(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        [HttpPut("/{id}")]
        public void UpdateRoomById(long id, [FromBody] Room updatedRoom)
        {
            _context.Update(updatedRoom);
        }

        [HttpDelete("/{id}")]
        public async Task DeleteRoomById(long id)
        {
            await _context.DeleteRoom(id);
        }

        [HttpGet("/rat-owners")]
        public async Task<List<Room>> GetRoomsForRatOwners()
        {
            return await _context.GetRoomsForRatOwners();
        }
    }
}
