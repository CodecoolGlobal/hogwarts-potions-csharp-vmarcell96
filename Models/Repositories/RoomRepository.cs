using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        public HogwartsContext Context { get; set; }
        public RoomRepository(HogwartsContext context)
        {
            Context = context;
        }
        public async Task AddRoom(Room room)
        {
            await Context.Rooms.AddAsync(room);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteRoom(long id)
        {
            var roomToDelete = await Context.Rooms.FirstAsync(m => m.ID == id);
            if (roomToDelete != null)
            {
                Context.Students.Where(s => s.Room == roomToDelete).Load();
                Context.Rooms.Remove(roomToDelete);
                await Context.SaveChangesAsync();
            }
            
        }

        public async Task<List<Room>> GetAllRooms()
        {
            return await Context.Rooms.Include(room => room.Residents).AsNoTracking().ToListAsync();
        }

        public async Task<Room> GetRoom(long roomId)
        {
            return await Context.Rooms.Include(room => room.Residents).AsNoTracking().FirstAsync(m => m.ID == roomId);
        }

        public async Task<List<Room>> GetRoomsForRatOwners()
        {
            return await Context.Rooms.Include(room => room.Residents)
                              .AsNoTracking()
                              .Where(r => !r.Residents.Any(stu => stu.PetType == PetType.Cat || stu.PetType == PetType.Owl))
                              .ToListAsync();
        }

        public void UpdateRoom(Room room)
        {
            Context.Rooms.Update(room);
            Context.SaveChangesAsync();
        }
    }
}
