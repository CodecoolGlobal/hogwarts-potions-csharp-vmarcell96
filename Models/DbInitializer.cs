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
            if (context.Rooms.Any() && context.Potions.Any())
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

            var ingredients = new Ingredient[] { new Ingredient() { Name = "Bauxit" }, new Ingredient() { Name = "Főzelék" } };
            Recipe recipe = new Recipe() { Brewer= new Student { Name = "TestStudentBrewer", HouseType = Enums.HouseType.Hufflepuff, PetType = Enums.PetType.Cat }, Name="Bauxit főzelék", Ingredients=ingredients };
             
            var potions = new Potion[]
            {
            new Potion{ Name="TestPotion1", Recipe=recipe, Brewer=new Student { Name = "TestStudentPotionBrewer", HouseType = Enums.HouseType.Hufflepuff, PetType = Enums.PetType.Cat }, Ingredients=ingredients },
            new Potion{ Name="TestPotion2" }
            };
            foreach (Potion potion in potions)
            {
                context.Potions.Add(potion);
            }
            context.SaveChanges();
        }
    }
}
