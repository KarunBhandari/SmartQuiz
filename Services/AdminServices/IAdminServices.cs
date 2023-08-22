using IQMania.Models.Account;
using IQMania.Models.Quiz;

namespace IQMania.Repository.AdminRepository
{
    public interface IAdminServices
    {
        Messages GetAdminMessages();

        Getaddquiz Getuseraddedquestions();
        ResponseResult AddMCQ(AddQuiz addQuiz);
        ResponseResult RemoveMCQ();

    }
}
