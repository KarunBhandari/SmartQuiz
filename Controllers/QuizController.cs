using IQMania.Models.Quiz;
using IQMania.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IQMania.Controllers
{
    public class QuizController : Controller
    {

        public readonly IQuizServices _quizRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public QuizController(IQuizServices quizRepository, IHttpContextAccessor httpContext)
        {
            _contextAccessor = httpContext;
            _quizRepository = quizRepository;
        }


       readonly List<Category> categories = new()
       {
                new Category{ Id = 0, category = "History" },
                new Category{ Id = 1, category = "Geography" },
                new Category{ Id = 2, category = "Economy" },
                new Category{ Id = 3, category = "Politics" },
                new Category{ Id = 4, category = "Time and Events" },
            };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayQuestions()
        {
            ViewBag.Categories = new SelectList(categories, "Id", "category");
            return View();
        }


        public IActionResult CategoryWiseQuestions(int id)
        {

            Category categoryy = categories[id];
            //var questions  = new List<Questions>();
            string dropdownValue = categoryy.category;
            List<Questions> questions = _quizRepository.ReadIq(dropdownValue).ToList();

            return View(questions);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddQuiz()
        {
            
            return View();
        }


        public JsonResult AddQuiz(AddQuiz addQuiz)
        {
            ResponseResult response = new();
            if (ModelState.IsValid)
            {

                response = _quizRepository.AddMCQ(addQuiz);
                return Json(response);
            }
            response = new ResponseResult() { ResponseCode = 400, ResponseDescription = "Bad Request! Invalid Information provided" };
            return Json(response); ;
        }

        [Authorize]
        public IActionResult PlayQuiz()
        {
            int uniqueRandomNumber = new Random().Next();
            _contextAccessor.HttpContext.Session.SetInt32("UserToken", uniqueRandomNumber);
            List<QuestionOptions> playquiz = _quizRepository.GetQuestions().ToList();
            return View(playquiz);

        }


        public JsonResult SetResult(QuizRequestModel request)
        {

            QuestionOptions response = _quizRepository.TestResult(request, HttpContext);


            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            SearchResult qstn = await _quizRepository.SearchQuestions(query);
           if(qstn.ResponseCode != 200)
            {ViewBag.Message= qstn.ResponseDescription.ToString(); }

            return View(qstn);
        }


    }


}
