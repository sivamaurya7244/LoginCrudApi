using LoginCrudApi.Models;

namespace LoginCrudApi.Repository
{
    public interface IloginRepository
    {
        Task<int> RegisterAsync(LoginModel model);

        Task<LoginModel> LoginAsync(LoginModel model);

        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetAllAsync(UserInputModel inputModel);

        Task<User?> GetByIdAsync(int id);

        Task<int> CreateAsync(User user);

        Task<int> UpdateAsync(User user);

        Task<int> DeleteAsync(int id);
    }
}