using IQMania.Models.Account;

namespace IQMania.Models
{
    public class UserResult
    {
       
        public int QId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string SubmittedAnswer { get; set; }
        public string IsCorrect { get; set; }
        
    }

    public class Marksheet
    {
        public IEnumerable<UserResult> QuestionResult { get; set; }
        public int Percentage { get; set; }
        public int Accuracy { get; set; }
        public double Result { get; set; }
    }
}
