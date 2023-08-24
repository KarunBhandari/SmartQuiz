using Repository;
  using IQMania.Models.Quiz;
using IQMania.Models.Account;
using System.Data.SqlClient;
using System.Data;
using IQMania.Helper;

namespace IQMania.Repository
{
    public class AccountServices : IAccountServices
    {
       readonly Dao connection1;
      

        public AccountServices()
        {
            connection1 = new Dao();
        }
        public ResponseResult Signup(Signup signup)
        {
            ResponseResult responseResult = new();
            string Sql = "Exec spcreateUser @flag = 'Signup'";
            Sql += " ,@Fullname= " + Dao.FilterString(signup.Name);
            Sql += " ,@Email= " + Dao.FilterString(signup.Email);
            Sql += " ,@Phone= " + Dao.FilterString(signup.Phonenumber);
            Sql += " ,@Password= " + Dao.FilterString(signup.Password);
            try {

                var response = connection1.ExecuteDataRow(Sql);
                if(response.Table.Rows != null )
                {
                    var dbRes = response.Table.Rows[0];
                    responseResult.ResponseCode = Convert.ToInt32(dbRes["ResponseCode"]);
                    responseResult.ResponseDescription = Convert.ToString((dbRes["ResponseDescription"]).ToString());
                }
             
                
            }
            catch (Exception ex) {
                responseResult.ResponseDescription = ex.Message;
            }

            return responseResult;
        }

        public Account Login(Login login)
        {
            
            Account account = new();
            string Sql = "Exec spGetLogininfo @flag='AuthLogin',";
            Sql += " @Email = " + Dao.FilterString(login.Email);
            Sql += ", @Password = " + Dao.FilterString(login.Password);
            
            try{

                var reader = connection1.ExecuteDataset(Sql);
                if (reader.Tables[1] != null) {
                    var dbRes = reader.Tables[1];
                    account.ResponseCode = Convert.ToInt32(dbRes.Rows[0]["ResponseCode"]);
                    account.ResponseDescription = (dbRes.Rows[0]["ResponseDescription"]).ToString();
                        }

                
                    if (account.ResponseCode == 302)
                    {
                    var dbRes1 = reader.Tables[0];

                        account.UId = (dbRes1.Rows[0]["Id"].ToString());
                        account.Email = (dbRes1.Rows[0]["Email"].ToString());
                        account.Name = (dbRes1.Rows[0]["FullName"].ToString());
                        account.Phonenumber = (dbRes1.Rows[0]["Phone"].ToString());
                        account.Role = (dbRes1.Rows[0]["Role"].ToString());
                        
                    }
                
            }
            catch(Exception ex) {
                account.ResponseDescription = ex.Message.ToString();
                
            }
            return account;

        }

        public ResponseResult ChangePassword(Signup signup)
        {
            ResponseResult result = new()
            {
                ResponseCode = 500,
                ResponseDescription = "Error Something went wrong"
            };
            try
            {
                string sql = "Exec spEditprofile @flag = 'changepass'";
                sql += ",@Fullname=" + Dao.FilterString(signup.Name);
                sql += ",@Email=" + Dao.FilterString(signup.Email);
                sql += ",@Phone=" + Dao.FilterString(signup.Phonenumber);
                sql += ",@Password=" + Dao.FilterString(signup.Password);
                var dbRes = connection1.ExecuteDataTable(sql);
                if (dbRes != null)
                {
                    result.ResponseCode = Convert.ToInt32(dbRes.Rows[0]["ResponseCode"]);
                    result.ResponseDescription =dbRes.Rows[0]["ResponseDescription"].ToString();
                }
            }
            catch (Exception ex)
            {
                result.ResponseDescription=ex.Message;
                
            }
              
            return result;
        }
    }
}
