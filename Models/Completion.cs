using IQMania.Models.Account;

namespace IQMania.Models
{
    public class UserResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int QId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string SubmittedAnswer { get; set; }
        public string IsCorrect { get; set; }
        
    }

    public class Marksheet
    {
        public IEnumerable<UserResult> QuestionResult { get; set; }
            public double Result { get; set; }
    }
}
