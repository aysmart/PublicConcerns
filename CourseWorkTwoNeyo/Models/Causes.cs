using System.ComponentModel.DataAnnotations;

namespace CourseWorkTwoNeyo.Models
{
    public class Causes
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string CauseBanner { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Targeted { get; set; }
        [Required]
        public string Focus { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string PlainContent { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int SigCount { get; set; }
        [Required]
        public int ViewCount { get; set; }
        public int ShareCount { get; set; }
        [Required]
        public int SigGoal { get; set; }
        [Required]
        public DateTime PostDate { get; set; } = DateTime.Now;
    }
}
