using Microsoft.AspNetCore.Mvc;
using IQMania.Models;
using IQMania.Repository.Completion;

namespace IQMania.Controllers
{
    public class CompletionController : Controller
    {
        private readonly ICompletionRepository _repository;
        public CompletionController(ICompletionRepository repository)
        {
            _repository = repository;
            
        }

       
        public IActionResult ExamFinished()
        {
            return View();
        }
        public IActionResult ViewResult()
        {
            Marksheet userResult = _repository.ViewResult(HttpContext);
            if (userResult.QuestionResult != null)
            {

                return View(userResult);
            }
            ViewBag.UserResult = "No datas to Display for user";
            return View();
        }

        
    }
}
