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

        public Task<List<Ingredient>> GetAllIngredients()
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteIngredient(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Ingredient> GetIngredient(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task AddIngredient(Ingredient ingredient)
        {
            throw new System.NotImplementedException();
        }
    }
}
