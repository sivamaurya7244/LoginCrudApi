using LoginCrudApi.Models;

namespace LoginCrudApi.Repository
{
    public interface IloginRepository
    {
        Task<int> RegisterAsync(LoginModel model);

        Task<LoginModel> LoginAsync(LoginModel model);

       
    }
}