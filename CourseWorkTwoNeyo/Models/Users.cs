using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CourseWorkTwoNeyo.Models
{
    [Index(nameof(UserEmail), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]

    public class Users
    {
        public string UserPhone { get; set; }
        public string AboutMe { get; set; }
        public int Disabled { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        
        public string UserPhoto { get; set; }
        [Required]
        public string UserEmail { get; set; }
        
        public string UserPassword { get; set; }
        [Required]
        public int EmailConfirm { get; set; }
        [Required]
        public string UserCity { get; set; }
        [Required]
        public string UserState { get; set; }
        [Required]
        public string UserCountry { get; set; }
        [Required]
        public DateTime RegDate { get; set; } = DateTime.Now;





    }
}
