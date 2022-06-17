using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IRecipeRepository
{
    public Task<List<Recipe>> GetAllRecipes();
    public Task DeleteRecipe(long id);
    public Task<Recipe> GetRecipe(long id);

    public Task AddRecipe(Recipe recipe);
    public Task<List<Recipe>> GetAllRecipesWithPotionIngredients(long potionId);
    

}