﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IPotionRepository
{
    public Task<List<Potion>> GetAllPotions();
    public Task DeletePotion(long id);
    public Task<Potion> GetPotionById(long potionId);

    public Task AddPotion(Potion potion);
    public Task<List<Potion>> GetAllPotionsCreatedByStudent(long studentId);
    public Task<Potion> AddEmptyPotion(Student student);
    public Task<Potion> AddIngredientToPotion(long potionId, Ingredient ingred);

}