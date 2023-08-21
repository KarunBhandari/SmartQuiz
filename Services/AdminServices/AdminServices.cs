using Repository;
using IQMania.Models.Account;
using IQMania.Helper;
using System.Data.SqlClient;
using IQMania.Models.Quiz;
using System.Data;

namespace IQMania.Repository.AdminRepository
{
    public class AdminServices: IAdminServices
    {
      Dao connection;
        private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<QuizServices>();
        public AdminServices() {
            connection = new Dao();
            
        }

        public Messages GetAdminMessages()
        {
            var messages = new Messages();
            string sql = "Exec spcountrows @flag = 'AdminUser'";
            try
            {
                var response = connection.ExecuteDataset(sql);
                if (response.Tables[0] != null)
                {
                    var dbRes = response.Tables[0];
                    messages.QuizMessages = Convert.ToInt32(dbRes.Rows[0]["row_count"]);
                    messages.ResponseCode = 200;
                    messages.ResponseDescription = "Successfully retrived admin messages";

                }
                
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                messages.ResponseCode = 400;
                messages.ResponseDescription = message;
                
               Log.Error("Exception"+message+ "occured");
            }
            
            return messages;

        }

        public ResponseResult AddMCQ(AddQuiz addQuiz)
        {
            ResponseResult response = new();
            string sql = "Exec spAddMCQ @flag='AddminUser'";
            sql += " ,@Question=" + addQuiz.QuizQuestion;
            sql += " ,@Answer=" + addQuiz.QuizQuestion;
            sql += " ,@Category= " + addQuiz.QuizQuestion;
            sql += " ,@Option1= " + addQuiz.Option1;
            sql += " ,@Option2= " + addQuiz.Option2;
            sql += " ,@Option3= " + addQuiz.Option3;
            sql += " ,@Option4= " + addQuiz.Option4;
            try
            {
                var dbRes = connection.ExecuteDataTable(sql);
                if(dbRes != null)
                {
                    response.ResponseCode = Convert.ToInt32(dbRes.Rows[0]["ResponseCode"]);
                    response.ResponseDescription = (dbRes.Rows[0]["ResponseDescription"]).ToString();

                }

                    response.ResponseCode = 200;
                    response.ResponseDescription = "Successfully inserted the QuestionSet";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 404;
                response.ResponseDescription = "Error Occured " + ex.Message;
            }
            return response;
        }


    }
}
