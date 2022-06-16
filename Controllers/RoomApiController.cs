using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HogwartsPotions.Controllers
{
    [ApiController, Route("rooms")]
    public class RoomApiController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<RoomApiController> _logger;

        public RoomApiController(HogwartsContext context, ILogger<RoomApiController> logger, IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            try
            {
                return Ok(await _roomRepository.GetAllRooms());
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
                    await _roomRepository.AddRoom(room);
                    return CreatedAtAction("AddRoom", room);
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
            var room = await _roomRepository.GetRoom(id);

            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoomById(long id, [Bind("Capacity, Residents")] Room updatedRoom)
        {
            var roomToUpdate = await _roomRepository.GetRoom(id);
            if (roomToUpdate == null)
            {
                return NotFound();
            }
            updatedRoom.ID = roomToUpdate.ID;
            _roomRepository.UpdateRoom(updatedRoom);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoomById(long id)
        {
            var room = await _roomRepository.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }
            try
            {
                await _roomRepository.DeleteRoom(id);
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
            return Ok(await _roomRepository.GetRoomsForRatOwners());
        }
    }
}
