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
                messages.ResponseCode = 400;
                messages.ResponseDescription = ex.Message;
            }
            
            return messages;

        }

        public ResponseResult AddMCQ(AddQuiz addQuiz)
        {
            ResponseResult response = new();
            string sql = "Exec spAddUserQuestion @flag=''";
            try
            {
                
                {
                    SqlCommand command = new("spAddMCQ")
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


    }
}
