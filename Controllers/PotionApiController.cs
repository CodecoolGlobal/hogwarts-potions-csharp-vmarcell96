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
                    return NotFound($"Student with id:{studentId} not found!");
                }

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

        [HttpPost("brew/{studentId:long}")]
        public async Task<ActionResult<List<Potion>>> BrewPotion(long studentId)
        {
            try
            {
                var student = await _studentRepository.GetStudentById(studentId);
                var potion = await _potionRepository.AddEmptyPotion(student);
                return CreatedAtAction("BrewPotion", potion);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting the list of potions created by student: {studentId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPut("{potionId}/add")]
        public async Task<ActionResult> AddIngredientToPotion(long potionId, [FromBody] Ingredient ingredient)
        {
            try
            {
                var potion = await _potionRepository.GetPotionById(potionId);   

                if (potion.Ingredients.Count == 5)
                {
                    return StatusCode(500, $"The potion is already finished.");
                }

                var persistedIngredients = await _ingredientRepository.GetAllIngredients();
                foreach (var ingred in persistedIngredients)
                {
                    if (ingred.Name == ingredient.Name)
                    {
                        await _potionRepository.AddIngredientToPotion(potionId, ingred);
                        if (potion.Ingredients.Count == 5)
                        {
                            await _recipeRepository.ChangePotionStatus(potion);
                        }
                        return Ok(potion);
                    }
                }
                await _ingredientRepository.AddIngredient(ingredient);
                await _potionRepository.AddIngredientToPotion(potionId, ingredient);
                if (potion.Ingredients.Count == 5)
                {
                    await _recipeRepository.ChangePotionStatus(potion);
                }
                return Ok(potion);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while adding ingredient.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{potionId:long}/help")]
        public async Task<ActionResult<List<Recipe>>> GetAllRecipesWithPotionIngredients(long potionId)
        {
            try
            {
                var recipeList = await _recipeRepository.GetAllRecipesWithPotionIngredients(potionId);
                return Ok(recipeList);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting all recipes which contain ingredients in potion.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}