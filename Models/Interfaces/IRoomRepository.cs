using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IRoomRepository
{
    public Task AddRoom(Room room);
    public Task<Room> GetRoom(long roomId);
    public Task<List<Room>> GetAllRooms();
    public void UpdateRoom(Room room);
    public Task DeleteRoom(long id);
    public Task<List<Room>> GetRoomsForRatOwners();
}