using IQMania.Helper;
using IQMania.Models.Account;
using IQMania.Models.Quiz;
using IQMania.Repository;
using IQMania.Repository.AdminRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IQMania.Controllers
{
    public class QuizController : Controller
    {

        public readonly IQuizServices _quizRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAdminServices _adminServices;
        public QuizController(IQuizServices quizRepository, IHttpContextAccessor httpContext, IAdminServices adminServices)
        {
            _contextAccessor = httpContext;
            _quizRepository = quizRepository;
            _adminServices = adminServices;
        }


       readonly List<Category> categories = new()
       {
                new Category{ Id = 1, category = "History" },
                new Category{ Id = 2, category = "Geography" },
                new Category{ Id = 3, category = "Economy" },
                new Category{ Id = 4, category = "Politics" },
                new Category{ Id = 5, category = "Time and Events" },
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
            SearchResult questions = new SearchResult();
            var qstnlist = new List<Questions>();
            if (id == 0)
            { 
                qstnlist.Add(new Questions
                {
                    QID = 0,
                    Question = "",
                    Answer = ""

                }); 
              questions.QuestionList = qstnlist;
                
                return View(questions);
            }
            int cat = id - 1;
            Category categoryy = categories[cat];
            //var questions  = new List<Questions>();
            string dropdownValue = categoryy.category;
            qstnlist = _quizRepository.ReadIq(dropdownValue).ToList();
            questions.QuestionList = qstnlist;

            return View(questions);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddQuiz()
        {
            
            return View();
        }

        [Authorize]
        public JsonResult AddQuiz(AddQuiz addQuiz)
        {
            
            
            ResponseResult response;
            if (ModelState.IsValid)
            {
                var userinfo = HttpContext.GetLoginDetails();
                var role = userinfo?.Role;

                if (userinfo != null && role?.Contains("AdminUser") == true)
                {
                    response = _adminServices.AddMCQ(addQuiz);
                    return Json(response);
                }

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
        public async Task<JsonResult> Search(string query)
        {
            SearchResult qstn = new();
            if (string.IsNullOrEmpty(query))
            {
                qstn.ResponseDescription = "Please Enter Something to Search";
                return Json(qstn);
            }
             qstn = await _quizRepository.SearchQuestionsAsync(query);
           

            return Json(qstn);
        }
    }
}
