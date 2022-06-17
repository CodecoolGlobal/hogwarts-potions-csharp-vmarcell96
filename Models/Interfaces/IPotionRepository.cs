using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IPotionRepository
{
    public Task<List<Potion>> GetAllPotions();
    public Task DeletePotion(long id);
    public Task<Potion> GetPotion(long potionId);

    public Task AddPotion(Potion potion);

}