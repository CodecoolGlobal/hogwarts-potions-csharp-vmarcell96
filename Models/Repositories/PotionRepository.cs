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
                            .Include(potion => potion.Recipe)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<Potion> GetPotionById(long potionId)
        {
            return await Context.Potions
                .Include(p => p.Ingredients)
                .Include(p => p.Brewer)
                .Include(p => p.Recipe)
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
            await Context.Potions.AddAsync(potion);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Potion>> GetAllPotionsCreatedByStudent(long studentId)
        {
            return await Context.Potions
                            .Include(potion => potion.Brewer)
                            .Include(potion => potion.Ingredients)
                            .Include(potion => potion.Recipe)
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

        public async Task AddIngredientToPotion(long potionId, Ingredient ingred)
        {
            var potion = await GetPotionById(potionId);
            if (potion != null)
            {
                foreach (var ingredient in potion.Ingredients)
                {
                    if (ingredient.Name == ingred.Name)
                    {
                        return;
                    }
                }
                potion.Ingredients.Add(ingred);
                await Context.SaveChangesAsync();
            }
        }

    }
}
