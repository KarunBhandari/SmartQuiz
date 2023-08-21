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


            return View(userResult);
        }

        
    }
}
