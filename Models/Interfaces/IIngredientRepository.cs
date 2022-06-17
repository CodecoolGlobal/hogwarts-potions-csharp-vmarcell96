using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IIngredientRepository
{
    public Task<List<Ingredient>> GetAllIngredients();
    public Task DeleteIngredient(long id);
    public Task<Ingredient> GetIngredient(long id);

    public Task AddIngredient(Ingredient ingredient);
    //public Task<bool> CheckIfPotionAlreadyExists(Potion potion);

}