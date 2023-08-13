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
        Dao connection1;
      

        public AccountServices()
        {
            connection1 = new Dao();
        }
        public ResponseResult Signup(Signup signup)
        {
            ResponseResult responseResult = new ResponseResult();
            string Sql = "Exec spcreateUser @flag = 'Signup'";
            Sql += " ,@Fullname= " + connection1.FilterString(signup.Name);
            Sql += " ,@Email= " + connection1.FilterString(signup.Email);
            Sql += " ,@Phone= " + connection1.FilterString(signup.Phonenumber);
            Sql += " ,@Password= " + connection1.FilterString(signup.Password);
            try {

                var response = connection1.ExecuteDataRow(Sql);

             
                //_ = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex) { }

            return new ResponseResult { ResponseCode = 200 };
        }

        public Account Login(Login login)
        {
            Account account = new Account();
            string Sql = "Exec spGetLogininfo ";
            Sql += " @Email = " + connection1.FilterString(login.Email);
            Sql += " ,@Password = " + connection1.FilterString(login.Password);
            
            try{

                var reader = connection1.ExecuteDataRow(Sql);

                
                int rows = 0;
                
                    
                    rows++;
                    if (rows == 1)
                    {
                        account.UId = reader["Id"].ToString();
                        account.Email = reader["Email"].ToString();
                        account.Name = reader["FullName"].ToString();
                        account.Phonenumber = reader["Phone"].ToString();
                        account.Role = reader["Role"].ToString();
                        return account;
                    }
                    
                
                account.ResponseCode = 404;
                account.ResponseDescription = "User Not Found";
                return account;
            }
            catch {
                return account;
            }
            
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
                string? sql = "Exec spEditprofile @flag = 'changepass'";
                sql += ",@Fullname=" + connection1.FilterString(signup.Name);
                sql += ",@Email=" + connection1.FilterString(signup.Email);
                sql += ",@Phone=" + connection1.FilterString(signup.Phonenumber);
                sql += ",@Password=" + connection1.FilterString(signup.Password);
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
