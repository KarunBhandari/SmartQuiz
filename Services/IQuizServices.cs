using IQMania.Models.Quiz;

namespace IQMania.Repository
{
    public interface IQuizServices
    {
        List<QuestionOptions> GetQuestions();
        List<Questions> ReadIq(string dropdownValue);

        ResponseResult AddMCQ(AddQuiz addQuiz);

        //responseResult AddUserTable(HttpContext httpContext);

        QuestionOptions TestResult(QuizRequestModel quizRequestModel, HttpContext httpContext);

        Task<SearchResult> SearchQuestionsAsync(string query);


    }
}
