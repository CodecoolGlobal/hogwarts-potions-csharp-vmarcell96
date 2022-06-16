using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models;
using HogwartsPotions.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HogwartsPotions.Controllers
{
    [ApiController, Route("rooms")]
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
        [ActionName(nameof(AddRoom))]
        public async Task<ActionResult<Room>> AddRoom([Bind("Capacity, Residents")] Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddRoom(room);
                    await _context.SaveChangesAsync();
                    //faulty butt works
                    return CreatedAtRoute("AddRoom", room);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogCritical(
                    $"Exception while adding room.", ex);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
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

            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoomById(long id, [Bind("Capacity, Residents")] Room updatedRoom)
        {
            var roomToUpdate = await _context.GetRoom(id);
            if (roomToUpdate == null)
            {
                return NotFound();
            }
            updatedRoom.ID = roomToUpdate.ID;
            _context.Update(updatedRoom);
            //await _context.UpdateRoom(id, updatedRoom);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoomById(long id)
        {
            var room = await _context.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }
            try
            {
                await _context.DeleteRoom(id);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogCritical(
                    $"Exception while deleting room.", ex);
            }
            return NoContent();
        }

        [HttpGet("rat-owners")]
        public async Task<ActionResult<List<Room>>> GetRoomsForRatOwners()
        {
            return Ok(await _context.GetRoomsForRatOwners());
        }
    }
}
