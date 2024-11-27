using Dashboard.BLL.Services.TestService;
using Dashboard.DAL.ViewModels.Tests;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/tests")]
    public class TestController : BaseController
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync()
        {
            return Ok();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetListAsync(string page, string pageSize)
        {
            var response = await _testService.GetListAsync(page, pageSize);
            return await GetResultAsync(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]string? userId)
        {
            userId = Request.Query[nameof(userId)];

            if (userId == null)
            {
                var response = await _testService.GetAllAsync();
                return await GetResultAsync(response);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTestVM model)
        {
            var response = await _testService.CreateAsync(model);
            return await GetResultAsync(response);
        }
    }
}
