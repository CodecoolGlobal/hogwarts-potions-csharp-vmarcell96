using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        public HogwartsContext Context { get; set; }
        public RecipeRepository(HogwartsContext context)
        {
            Context = context;
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await Context.Recipes
                                .Include(potion => potion.Brewer)
                                .Include(potion => potion.Ingredients)
                                .AsNoTracking()
                                .ToListAsync();
        }

        public Task DeleteRecipe(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Recipe> GetRecipe(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddRecipe(Recipe recipe)
        {
            await Context.Recipes.AddAsync(recipe);
            await Context.SaveChangesAsync();
        }

        //public async Task<bool> CheckIfRecipeAlreadyExists(Potion potion)
        //{
        //    var recipes = await GetAllRecipes();
        //    foreach (var recipe in recipes)
        //    {
        //        if (pot.Ingredients == potion.Ingredients)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
