using HogwartsPotions.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HogwartsPotions.Models.Entities
{
    public class Potion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Name { get; set; }

        public Student Brewer { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; } = new HashSet<Ingredient>();

        public BrewingStatus Status { get; set; } = BrewingStatus.Brew;

        public Recipe Recipe { get; set; }
    }
}