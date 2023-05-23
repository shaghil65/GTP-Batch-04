using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Task7_UpdateEF.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [Range(1,150)]
        public int Age { get; set; }
    }
}
