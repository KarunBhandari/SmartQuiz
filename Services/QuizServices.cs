using IQMania.Models.Quiz;
using IQMania.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using System.Net.Http;
using Repository;

namespace IQMania.Repository
{
    public class QuizServices : IQuizServices
    {
       readonly Dao connection1;
        private readonly IHttpContextAccessor _contextAccessor;
        private string Constr { get; set; }
        
        public IConfiguration configuration;

        public QuizServices(IConfiguration _configuration, IHttpContextAccessor accessor)
        {
            _contextAccessor = accessor;
            configuration = _configuration;
            Constr = configuration.GetConnectionString("DefaultConnection");
            connection1 = new Dao();
        }
       
        public List<QuestionOptions> GetQuestions()
        {
            List<QuestionOptions> questionOptions = new();


            using (SqlConnection con = new(Constr))
            {
                string storp = "spGetMCQs";
                SqlCommand cmd = new(storp, con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@flag", "Qstbycat");
                
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    QuestionOptions questions1 = new()
                    {
                        QNO = Convert.ToInt32(rdr["Question_Number"]),
                        Questions = rdr["Questions"].ToString(),
                        Option1 = rdr["Option1"].ToString(),
                        Option2 = rdr["Option2"].ToString(),
                        Option3 = rdr["Option3"].ToString(),
                        Option4 = rdr["Option4"].ToString()
                    };
                    questionOptions.Add(questions1);

                }

            }

            return questionOptions;
        }

        public List<Questions> ReadIq(string dropdownValue)
        {
            List<Questions> questions = new();
            using (SqlConnection con = new(Constr))
            {
                SqlCommand cmd = new("spGetQuestions", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@flag","GetMCQs");
                cmd.Parameters.AddWithValue("@Category",dropdownValue);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Questions questions1 = new()
                    {
                        QID = Convert.ToInt32(rdr["Question_Number"]),
                        Question = rdr["Questions"].ToString(),
                        Answer = rdr["Answer"].ToString()
                    };
                    questions.Add(questions1);
                }
            }
            return questions;
        }

        public ResponseResult AddMCQ(AddQuiz addQuiz)
        {
            ResponseResult response = new();
            try
            {
                using (SqlConnection con = new(Constr))
            {
                SqlCommand command = new("spAddMCQ", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@flag", "AddminUser");
                    command.Parameters.AddWithValue("@Question", addQuiz.QuizQuestion);
                    command.Parameters.AddWithValue("@Answer", addQuiz.QuizAnswer);
                    command.Parameters.AddWithValue("@Category", addQuiz.Category);
                    command.Parameters.AddWithValue("@Option1", addQuiz.Option1);
                    command.Parameters.AddWithValue("@Option2", addQuiz.Option2);
                    command.Parameters.AddWithValue("@Option3", addQuiz.Option3);
                    command.Parameters.AddWithValue("@Option4", addQuiz.Option4);


                    con.Open();
                    int effect = command.ExecuteNonQuery();


                    response.ResponseCode = 200;
                    response.ResponseDescription = "Successfully inserted the QuestionSet";

                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 404;
                response.ResponseDescription = "Error Occured " + ex.Message;
            }
            return response;
        }

        

        public QuestionOptions TestResult(QuizRequestModel quizRequestModel,HttpContext httpContext)
        {
            QuestionOptions result = new();
            try {
                    using (SqlConnection con = new(Constr))
                    {
                        string?
                        claimUserID = httpContext.User.FindFirst("UserId")?.Value.ToString();
                        int UserID = 0;
                         if(int.TryParse(claimUserID, out int parsedUserID))
                        { 
                        UserID = parsedUserID;

                         SqlCommand cmd = new("spMainTestResult", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                        var usertoken = _contextAccessor.HttpContext.Session.GetInt32("UserToken");
                         cmd.Parameters.AddWithValue("@QuestionId", quizRequestModel.QuestionNo);
                         cmd.Parameters.AddWithValue("@Selectedanswer", quizRequestModel.Answer);
                         cmd.Parameters.AddWithValue("@UID", UserID);
                        cmd.Parameters.AddWithValue("@Usertoken", usertoken);

                         con.Open();
                         int status = cmd.ExecuteNonQuery();

             
                         result.ResponseCode = 200;
                         result.ResponseDescription = "OK";
                         return result;
                         }
                    }      
              }
            catch (Exception ex)
            {
                result.ResponseCode = 500;
                result.ResponseDescription = ex.Message;
            }
            return result;
        }

        public async Task<SearchResult> SearchQuestions(string query)
        {
            Questions qstn = new();
            SearchResult questions = new();
            string sql = "Exec spSearchquestiontext @flag = 'Search'";
            sql += ", @@inputtext = " + query.ToString();
            try
            {
                var response = connection1.ExecuteDataset(sql);
                if(response != null)
                {
                    var dbres = response.Tables[1];
                    questions.ResponseCode = Convert.ToInt32(dbres.Rows[0]["ResponseCode"]);
                    questions.ResponseDescription = (dbres.Rows[1]["ResponseDescription"]).ToString();

                    if(questions.ResponseCode == 200)
                    {
                        var dbres1 = response.Tables[0];
                        int i = 0;
                        foreach(DataRow row in dbres1.Rows) {
                            i= i++;
                            questions.Questions.Add(new Questions()
                            {
                                QID = i,
                                Question = (row["Questions"]).ToString(),
                                Answer = (row["Questions"]).ToString(),
                                
                            });

                        }
                        
                    }
                    
                }
                
            }
            catch(Exception ex) {
                  questions.ResponseDescription = ex.Message; 
            
            }
            
            return questions;
        }
    }
}
