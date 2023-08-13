using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IQMania.Models.Quiz
{
    public class Questions
    {

        
        public int QID { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
 
    public class Options
    {
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
    }
}
