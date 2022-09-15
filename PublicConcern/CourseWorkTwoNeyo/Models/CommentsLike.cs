using System.ComponentModel.DataAnnotations;

namespace CourseWorkTwoNeyo.Models
{
    public class CommentsLike
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CommentId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
