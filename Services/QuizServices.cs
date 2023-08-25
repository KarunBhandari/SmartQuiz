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
using NuGet.Protocol.Plugins;
using System.Runtime.CompilerServices;

namespace IQMania.Repository
{
    public class QuizServices : IQuizServices
    {
        private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<QuizServices>();
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
            try
            {
                using (SqlConnection con = new(Constr))
                {
                    SqlCommand cmd = new("spGetQuestions", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@flag", "GetMCQs");
                    cmd.Parameters.AddWithValue("@Category", dropdownValue);
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
                    return questions;
                }
            }
            catch (Exception ex) 
            {
                Log.Error("Error occured while retriving catgory wise questions"+ ex.ToString());
                return questions;
            }
            
        }

        public ResponseResult AddMCQ(AddQuiz addQuiz)
        {
            ResponseResult response = new();
            string sql = "Exec spAddUserQuestion @flag='User'";
            sql += " ,@Questions=" + Dao.FilterString(addQuiz.QuizQuestion);
            sql += " ,@Answer=" + Dao.FilterString(addQuiz.QuizAnswer);
            sql += " ,@Category=" + Dao.FilterString(addQuiz.Category);
            sql += " ,@Option1=" + Dao.FilterString(addQuiz.Option1);
            sql += " ,@Option2=" + Dao.FilterString(addQuiz.Option2);
            sql += " ,@Option3=" + Dao.FilterString(addQuiz.Option3);
            sql += " ,@Option4=" + Dao.FilterString(addQuiz.Option4);
            try
            {

                var dbRes = connection1.ExecuteDataTable(sql);
                if (dbRes != null)
                {
                    response.ResponseCode = Convert.ToInt32(dbRes.Rows[0]["ResponseCode"]);
                    response.ResponseDescription = (dbRes.Rows[0]["ResponseDescription"]).ToString();
                    return response;
                }
                response.ResponseCode = 304;
                response.ResponseDescription = "Could not insert into database";
                return response;

            }
            catch (Exception ex)
            {
                Log.Error("Exception occured at adding questions by generaluser" + ex.Message, ex);
                response.ResponseCode = 500;
                response.ResponseDescription = "Internal Server Error";

            }
            return response;
            
        }



        public QuestionOptions TestResult(QuizRequestModel quizRequestModel, HttpContext httpContext)
        {
            QuestionOptions result = new();
            try
            {
                using (SqlConnection con = new(Constr))
                {
                    string
                    claimUserID = httpContext.User.FindFirst("UserId")?.Value.ToString();
                    int UserID = 0;
                    if (int.TryParse(claimUserID, out int parsedUserID))
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

        public async Task<SearchResult> SearchQuestionsAsync(string query)
        {
            Questions qstn = new();
            SearchResult questions = new();
            string sql = "Exec spSearchquestiontext @flag = 'Search'";
            sql += ", @inputtext = " + Dao.FilterString(query);
            try
            {
                var response = await Task.Run(() => connection1.ExecuteDataset(sql));
                if (response != null)
                {
                    var dbres = response.Tables[0];
                    questions.ResponseCode = Convert.ToInt32(dbres.Rows[0]["ResponseCode"]);
                    questions.ResponseDescription = (dbres.Rows[0]["ResponseDescription"]).ToString();

                    if (questions.ResponseCode == 200)
                    {
                        var dbres1 = response.Tables[1];
                        int i = 0;

                        var listData = new List<Questions>();
                        foreach (DataRow row in dbres1.Rows)
                        {
                            

                            listData.Add(new Questions
                            {
                                QID = i+1,
                                Question = (row["Questions"]).ToString(),
                                Answer = (row["Answer"]).ToString()
                            });
                            i++;
                        }
                        questions.QuestionList = listData;

                    }

                }

            }
            catch (Exception ex)
            {
                questions.ResponseDescription = ex.Message;

            }

            return questions;
        }
    }
}
