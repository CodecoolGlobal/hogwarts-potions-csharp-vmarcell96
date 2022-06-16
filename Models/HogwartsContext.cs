using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Models
{
    public class HogwartsContext : DbContext
    {
        public const int MaxIngredientsForPotions = 5;

        public HogwartsContext(DbContextOptions<HogwartsContext> options) : base(options)
        {
        }

        public async Task AddRoom(Room room)
        {
            await Rooms.AddAsync(room);
        }

        public async Task<Room> GetRoom(long roomId)
        {
            return await Rooms.Include(room => room.Residents).AsNoTracking().FirstAsync(m => m.ID == roomId);
        }

        public async Task<List<Room>> GetAllRooms()
        {
            return await Rooms.Include(room => room.Residents).AsNoTracking().ToListAsync();
        }

        public async Task UpdateRoom(long id, Room room)
        {
            var roomToUpdate = await Rooms.FirstAsync(m => m.ID == id);
            roomToUpdate.Capacity = room.Capacity;
            if (room.Residents.Count > 0)
            {
                roomToUpdate.Residents = room.Residents;
            }
            room.ID = id;  
        }

        public async Task DeleteRoom(long id)
        {
            var roomToDelete = await Rooms.FirstAsync(m => m.ID == id);
            Rooms.Remove(roomToDelete);
        }

        public async Task<List<Room>> GetRoomsForRatOwners()
        {
            return await Rooms.Include(room => room.Residents)
                              .AsNoTracking()
                              .Where(r => !r.Residents.Any(stu => stu.PetType == PetType.Cat || stu.PetType == PetType.Owl))
                              .ToListAsync();
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Student>().ToTable("Student");

        }
    }
}
