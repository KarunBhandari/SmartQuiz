using System.ComponentModel.DataAnnotations;

namespace IQMania.Models.Quiz
{
    public class AddQuiz
    {
        
        public int QID { get; set; }
        [Required]
        public string QuizQuestion { get; set; }
        [Required]
        public string QuizAnswer { get; set;}
        [Required]
        public string Category { get; set;}
        [Required]
        public string Option1 { get; set;}
        [Required]
        public string Option2 { get; set;}
        [Required]
        public string Option3 { get; set;}
        [Required]
        public string Option4 { get; set; }
    }
    public class Getaddquiz : ResponseResult
    {
        public List<AddQuiz> userAddedQuizzes { get; set;}
    }
}
