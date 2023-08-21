using IQMania.Models;

namespace IQMania.Repository.Completion
{
    public interface ICompletionRepository
    {
        Marksheet ViewResult(HttpContext httpContext);
    }
}
