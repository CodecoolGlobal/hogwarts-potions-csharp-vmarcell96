using HogwartsPotions.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HogwartsPotions.Models
{
    public static class DbInitializer
    {
        public static void Initialize(HogwartsContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Rooms.Any())
            {
                return;   // DB has been seeded
            }

            var rooms = new Room[]
            {
            new Room{ Capacity=3,Residents=new HashSet<Student>() { new Student { Name = "TestStudent1", HouseType = Enums.HouseType.Gryffindor, PetType = Enums.PetType.Rat } } },
            new Room{ Capacity=3,Residents=new HashSet<Student>() { new Student { Name = "TestStudent2", HouseType = Enums.HouseType.Hufflepuff, PetType = Enums.PetType.Cat } } }
            };
            foreach (Room room in rooms)
            {
                context.Rooms.Add(room);
            }
            context.SaveChanges();
        }
    }
}
