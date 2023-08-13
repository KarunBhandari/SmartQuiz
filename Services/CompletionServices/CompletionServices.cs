using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Data.SqlClient;
using IQMania.Repository;
using System.Net.Http;
using IQMania.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using IQMania.Models.Quiz;

namespace IQMania.Repository.Completion
{
    public class CompletionServices: ICompletionRepository
    {
        private readonly IHttpContextAccessor _contextAccessor;



        private string Constr { get; set; }
        public IConfiguration configuration;

        public CompletionServices(IConfiguration _configuration, IHttpContextAccessor accessor)
        {
            _contextAccessor = accessor;
            configuration = _configuration;
            Constr = configuration.GetConnectionString("DefaultConnection");

        }

        public List<UserResult> ViewResult(HttpContext httpContext)
        {
            List<UserResult> results = new List<UserResult>();
         
            
            using (SqlConnection sqlConnection = new(Constr))
            {
                string claimUserID = httpContext.User.FindFirst("UserId")?.Value.ToString();
                var usertoken = _contextAccessor.HttpContext.Session.GetInt32("UserToken");
                int UserID = 0;
                if (int.TryParse(claimUserID, out int parsedUserID))
                {
                    UserID = parsedUserID;
                    SqlCommand cmd = new("spViewResult", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                    cmd.Parameters.AddWithValue("@session", usertoken);
                cmd.Parameters.AddWithValue("@UID", UserID);
                sqlConnection.Open();
                 _ = cmd.ExecuteNonQuery();
                SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var status = Convert.ToInt32(rdr["IsCorrect"]);
                        UserResult account = new()
                        {
                            QId = Convert.ToInt32(rdr["QID"]),
                            Question = rdr["Questions"].ToString(),
                            Answer = rdr["Answer"].ToString(),
                            SubmittedAnswer = rdr["SubmittedAnswer"].ToString(),
                            IsCorrect = (status == 0) ? "Incorrect" : "Correct",
                        };



                        results.Add(account);
                    }
                }
                return results;
            }
        }
    }

    
}
