using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Task7_UpdateEF.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Range(1,150)]
        public int Age { get; set; }
        public string City { get; set; }
    }
}
