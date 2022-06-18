using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Repositories
{
    public class PotionRepository : IPotionRepository
    {
        public HogwartsContext Context { get; set; }
        public PotionRepository(HogwartsContext context)
        {
            Context = context;
        }

        public async Task<List<Potion>> GetAllPotions()
        {
            return await Context.Potions
                            .Include(potion => potion.Brewer)
                            .Include(potion => potion.Ingredients)
                            .Include(potion => potion.Recipe).ThenInclude(r => r.Ingredients)
                            .Include(potion => potion.Recipe).ThenInclude(r => r.Brewer).ThenInclude(s => s.Room)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<Potion> GetPotionById(long potionId)
        {
            return await Context.Potions
                .Include(p => p.Ingredients)
                .Include(p => p.Brewer)
                .Include(p => p.Recipe).ThenInclude(r => r.Ingredients)
                .Include(p => p.Recipe).ThenInclude(r => r.Brewer).ThenInclude(s => s.Room)
                .FirstAsync(potion => potion.ID == potionId);
        }

        public async Task DeletePotion(long id)
        {
            var potion = await GetPotionById(id);
            if (potion != null)
            {
                Context.Potions.Remove(potion);
                await Context.SaveChangesAsync();
            }
        }

        public async Task AddPotion(Potion potion)
        {
            if (potion.Ingredients.Any())
            {
                var newPotion = new Potion() { Brewer=potion.Brewer, Name=potion.Name };
                //Check if ingredients from posted potion are already in database or not
                foreach (var ingredient in potion.Ingredients)
                {
                    if (!Context.Ingredients.Any(i => i.Name == ingredient.Name))
                    {
                        await Context.Ingredients.AddAsync(ingredient);
                        newPotion.Ingredients.Add(ingredient);
                    }
                    else
                    {
                        var existingIngredient = await Context.Ingredients.Where(i => i.Name == ingredient.Name).FirstAsync();
                        newPotion.Ingredients.Add(existingIngredient);
                    }
                }
                //Check if the potion is ready or not
                if (newPotion.Ingredients.Count == 5)
                {
                    //Check if recipe of potion already exists
                    if (Context.Recipes.Any(r => r.Ingredients == newPotion.Ingredients))
                    {
                        newPotion.Status = BrewingStatus.Replica;
                    }
                    else
                    {
                        //If its a discovery its recipe have to be persisted, and added to the potion's recipe property
                        newPotion.Status = BrewingStatus.Discovery;
                        var discoveredRecipe = new Recipe() { Brewer = newPotion.Brewer, Name = $"{newPotion.Brewer.Name}'s discovery", Ingredients = newPotion.Ingredients };
                        await Context.AddAsync(discoveredRecipe);
                        newPotion.Recipe = discoveredRecipe;
                    }
                }
                await Context.Potions.AddAsync(newPotion);
                await Context.SaveChangesAsync();
            }
            await Context.Potions.AddAsync(potion);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Potion>> GetAllPotionsCreatedByStudent(long studentId)
        {
            return await Context.Potions
                            .Include(potion => potion.Brewer).ThenInclude(s => s.Room)
                            .Include(potion => potion.Ingredients)
                            .Include(p => p.Recipe).ThenInclude(r => r.Ingredients)
                            .Include(p => p.Recipe).ThenInclude(r => r.Brewer).ThenInclude(s => s.Room)
                            .Where(potion => potion.Brewer.ID == studentId)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<Potion> AddEmptyPotion(Student student)
        {
            var potion = new Potion() { Brewer=student, Status=BrewingStatus.Brew };
            await Context.Potions.AddAsync(potion);
            await Context.SaveChangesAsync();
            return potion;
        }

        public async Task<Potion> AddIngredientToPotion(long potionId, Ingredient ingred)
        {
            var potion = await GetPotionById(potionId);
            if (potion != null && potion.Ingredients.Count < 5)
            {
                foreach (var ingredient in potion.Ingredients)
                {
                    if (ingredient.Name == ingred.Name)
                    {
                        return potion;
                    }
                }
                potion.Ingredients.Add(ingred);
                await Context.SaveChangesAsync();
                return potion;
            }
            return null;
        }

    }
}
