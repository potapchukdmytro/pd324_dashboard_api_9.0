using Dashboard.DAL.ViewModels.Tests;

namespace Dashboard.BLL.Services.TestService
{
    public interface ITestService
    {
        public Task<ServiceResponse> CreateAsync(CreateTestVM model);
        public Task<ServiceResponse> GetAllAsync();
        public Task<ServiceResponse> GetByUserAsync(string userId);
        public Task<ServiceResponse> GetListAsync(string page, string pageSize);
    }
}
