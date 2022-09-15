using System.ComponentModel.DataAnnotations;

namespace CourseWorkTwoNeyo.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CauseId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int LikeCount { get; set; }
        [Required]
        public DateTime PostDate { get; set; } = DateTime.Now;
    }
}
