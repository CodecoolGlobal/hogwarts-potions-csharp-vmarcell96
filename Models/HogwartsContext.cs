using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public Task<Room> GetRoom(long roomId)
        {
            return Rooms.FirstAsync(m => m.ID == roomId);
        }

        public async Task<List<Room>> GetAllRooms()
        {
            Task<List<Room>> roomList = Rooms.ToListAsync();
            return await roomList;
        }

        public async Task UpdateRoom(Room room)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteRoom(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Room>> GetRoomsForRatOwners()
        {
            throw new NotImplementedException();
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
