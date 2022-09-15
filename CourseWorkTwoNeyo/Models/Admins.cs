using System.ComponentModel.DataAnnotations;

namespace CourseWorkTwoNeyo.Models
{
    public class Admins
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AdminPhoto { get; set; }
        [Required]
        public string AdminEmail { get; set; }
        [Required]
        public string AdminPassword { get; set; }
        
    }
}
