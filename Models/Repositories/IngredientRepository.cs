using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        public HogwartsContext Context { get; set; }
        public IngredientRepository(HogwartsContext context)
        {
            Context = context;
        }

        public async Task<List<Ingredient>> GetAllIngredients()
        {
            return await Context.Ingredients
                            .AsNoTracking()
                            .ToListAsync();
        }

        public Task DeleteIngredient(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Ingredient> GetIngredient(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
            await Context.Ingredients.AddAsync(ingredient);
            await Context.SaveChangesAsync();
        }
    }
}
