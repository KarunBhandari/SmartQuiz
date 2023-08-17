using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IQMania.Models.Quiz
{
    public class Questions
    {

        
        public int QID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
 
    public class SearchResult: ResponseResult
    {

        public List<Questions> QuestionList { get; set;}
    }
}
