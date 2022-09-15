using System.ComponentModel.DataAnnotations;

namespace CourseWorkTwoNeyo.Models
{
    public class Signatures
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CauseId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime SignDate { get; set; } = DateTime.Now;
    }
}
