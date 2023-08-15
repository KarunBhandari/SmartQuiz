using System.ComponentModel.DataAnnotations;

namespace IQMania.Models.Quiz
{
    public class QuestionOptions: ResponseResult
    {
        public int QNO { get; set; }

      
        public string Questions { get; set; }
    
        public string Option1 { get; set; }
       
        public string Option2 { get; set; }
       
        public string Option3 { get; set; }
        
        public string Option4 { get; set; }
    }

    public class ResponseResult
    {
        public int ResponseCode { get; set; }
        public string ResponseDescription { get; set; }

    }

    public class QuizRequestModel
    {
        public int QuestionNo { get; set; }
        public string Answer { get; set; }
    }
}
