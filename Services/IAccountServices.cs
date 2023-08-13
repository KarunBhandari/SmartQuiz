using IQMania.Models.Quiz;
using IQMania.Models.Account;

namespace IQMania.Repository

{
    public interface IAccountServices
    {
        ResponseResult Signup(Signup signup);

        Account Login(Login login);

        ResponseResult ChangePassword(Signup signup);
        


    }
}
