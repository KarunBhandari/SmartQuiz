using System.ComponentModel.DataAnnotations;

namespace IQMania.Models.Quiz
{
    public class AddQuiz
    {
        [Required]
        public string QuizQuestion { get; set; }
        [Required]
        public string QuizAnswer { get; set;}
        [Required]
        public string Category { get; set;}
        [Required]
        public string? Option1 { get; set;}
        [Required]
        public string Option2 { get; set;}
        [Required]
        public string? Option3 { get; set;}
        [Required]
        public string Option4 { get; set; }
    }
}
