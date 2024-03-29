﻿using System.Collections.Generic;
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
                                .Include(recipe => recipe.Brewer)
                                .Include(recipe => recipe.Ingredients)
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

        public async Task<List<Recipe>> GetAllRecipesWithPotionIngredients(long potionId)
        {
            var potion = await Context.Potions.Include(potion => potion.Ingredients).FirstAsync(potion => potion.ID == potionId);
            return await Context.Recipes
                .Include(recipe => recipe.Brewer)
                .Include(recipe => recipe.Ingredients)
                .Where(recipe => recipe.Ingredients.Any(ingredient => potion.Ingredients.Contains(ingredient)))
                .AsNoTracking()
                .ToListAsync();  
        }

        public async Task ChangePotionStatus(Potion potion)
        {
            var recipes = await GetAllRecipes();
            foreach (var recipe in recipes)
            {
                if (recipe == potion.Ingredients)
                {
                    potion.Status = BrewingStatus.Replica;
                    potion.Recipe = recipe;
                    await Context.SaveChangesAsync();
                }
            }
            potion.Status = BrewingStatus.Discovery;
            var newRecipe = new Recipe() { Brewer=potion.Brewer, Ingredients=potion.Ingredients, Name=$"{potion.Brewer.Name}'s discovery" };
            await AddRecipe(newRecipe);
            potion.Recipe = newRecipe;
            await Context.SaveChangesAsync();
        }
    }
}
