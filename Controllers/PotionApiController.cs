using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HogwartsPotions.Controllers
{
    [ApiController, Route("potions")]
    public class PotionApiController : ControllerBase
    {
        private readonly IPotionRepository _potionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly ILogger<PotionApiController> _logger;

        public PotionApiController(ILogger<PotionApiController> logger,
                                   IPotionRepository potionRepository,
                                   IStudentRepository studentRepository,
                                   IRecipeRepository recipeRepository,
                                   IIngredientRepository ingredientRepository)
        {
            _potionRepository = potionRepository;
            _recipeRepository = recipeRepository;
            _studentRepository = studentRepository;
            _ingredientRepository = ingredientRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<List<Potion>>> GetAllPotions()
        {
            try
            {
                return Ok(await _potionRepository.GetAllPotions());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting the list of potions.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPost("{studentId:long}")]
        public async Task<ActionResult<Potion>> AddPotion(long studentId, [FromBody] Potion potion)
        {
            try
            {
                var student = await _studentRepository.GetStudentById(studentId);

                if (student == null)
                {
                    return NotFound();
                }

                //Check if ingredients match any potion
                //if (!await _potionRepository.CheckIfPotionAlreadyExists(potion))
                //{
                //    potion.Status = Models.Enums.BrewingStatus.Discovery;
                //    var recipe = new Recipe() { Brewer = student, Name = $"{student.Name}'s discovery", Ingredients = potion.Ingredients };
                //    await _recipeRepository.AddRecipe(recipe);
                //}
                //else
                //{
                //    potion.Status = Models.Enums.BrewingStatus.Replica;
                //}
                //

                if (ModelState.IsValid)
                {
                    potion.Brewer = student;
                    await _potionRepository.AddPotion(potion);
                }

                return CreatedAtAction("AddPotion", potion);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogCritical(
                    $"Exception while adding potion.", ex);
            }
            return NoContent();
        }

        [HttpGet("{studentId:long}")]
        public async Task<ActionResult<List<Potion>>> GetAllPotionsCreatedByStudent(long studentId)
        {
            try
            {
                return Ok(await _potionRepository.GetAllPotionsCreatedByStudent(studentId));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting the list of potions created by student: {studentId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        //[HttpPut("{potionId:long}/add-ingredient")]
        //public async Task<Potion> AddIngredientToPotion(long potionId, [FromBody] Ingredient ingredient)
        //{
        //    var potion = await _potionRepository.GetPotion(potionId);

        //    _ingredientRepository.AddIngredient(ingredient);
        //    await _potionRepository.ModifyPotion(potion, ingredient);

        //    return await Task.Run(() => potion);
        //}

        //[HttpPut("{potionId:long}/add-ingredient/{ingredientId:long}")]
        //public async Task<Potion> AddIngredientToPotion(long potionId, long ingredientId)
        //{
        //    var potion = await _potionRepository.GetPotion(potionId);
        //    var ingredient = await _ingredientRepository.GetIngredient(ingredientId);

        //    await _potionRepository.ModifyPotion(potion, ingredient);

        //    return await Task.Run(() => potion);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Room>> GetRoomById(long id)
        //{
        //    var room = await _potionRepository.GetRoom(id);

        //    if (room == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(room);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> UpdateRoomById(long id, [Bind("Capacity, Residents")] Room updatedRoom)
        //{
        //    var roomToUpdate = await _potionRepository.GetRoom(id);
        //    if (roomToUpdate == null)
        //    {
        //        return NotFound();
        //    }
        //    updatedRoom.ID = roomToUpdate.ID;
        //    _potionRepository.UpdateRoom(updatedRoom);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteRoomById(long id)
        //{
        //    var room = await _potionRepository.GetRoom(id);
        //    if (room == null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        await _potionRepository.DeleteRoom(id);
        //        return NoContent();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        _logger.LogCritical(
        //            $"Exception while deleting room.", ex);
        //    }
        //    return NoContent();
        //}

        //[HttpGet("rat-owners")]
        //public async Task<ActionResult<List<Room>>> GetRoomsForRatOwners()
        //{
        //    return Ok(await _potionRepository.GetRoomsForRatOwners());
        //}
    }
}