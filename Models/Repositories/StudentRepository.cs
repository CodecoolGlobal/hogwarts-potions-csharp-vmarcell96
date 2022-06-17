using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        public HogwartsContext Context { get; set; }
        public StudentRepository(HogwartsContext context)
        {
            Context = context;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await Context.Students
                            .Include(s => s.Room)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<Student> GetStudentById(long id)
        {
            return await Context.Students
                .Include(s => s.Room)
                .FirstAsync(s => s.ID == id);
        }


        //public async Task DeletePotion(long id)
        //{
        //    var potion = await GetPotion(id);
        //    if (potion != null)
        //    {
        //        Context.Potions.Remove(potion);
        //        await Context.SaveChangesAsync();
        //    }
        //}

        //public async Task AddPotion(Potion potion)
        //{
        //    await Context.Potions.AddAsync(potion);
        //    await Context.SaveChangesAsync();
        //}
    }
}
