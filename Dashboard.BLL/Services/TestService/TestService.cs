using AutoMapper;
using Dashboard.DAL.Models.Tests;
using Dashboard.DAL.Repositories.TestRepository;
using Dashboard.DAL.ViewModels.Tests;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.BLL.Services.TestService
{
    public class TestService : ITestService
    {
        private readonly IMapper _mapper;
        private readonly ITestRepository _testRepository;

        public TestService(IMapper mapper, ITestRepository testRepository)
        {
            _mapper = mapper;
            _testRepository = testRepository;
        }

        public async Task<ServiceResponse> CreateAsync(CreateTestVM model)
        {
            var test = _mapper.Map<Test>(model);
            test.UserId = Guid.Parse(model.UserId);

            foreach (var q in model.Test.Questions)
            {
                var question = _mapper.Map<Question>(q);
                question.TestId = test.Id;

                foreach (var a in q.Answers)
                {
                    var answer = _mapper.Map<Answer>(a);
                    answer.QuestionId = question.Id;
                    question.Answers.Add(answer);
                }
                test.Questions.Add(question);
            }

            var result = await _testRepository.CreateAsync(test);

            if(result)
            {
                return ServiceResponse.GetOkResponse("Тест успішно додано");
            }

            return ServiceResponse.GetBadRequestResponse("Не вдалося створити тест", "Не вдалося створити тест");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var tests = await _testRepository
                .GetAll()
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .ToListAsync();

            var models = _mapper.Map<List<TestVM>>(tests);

            return ServiceResponse.GetOkResponse("Тести отримано", models);
        }

        public Task<ServiceResponse> GetByUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> GetListAsync(string page, string pageSize)
        {
            var tests = _testRepository.GetAll();

            int pageInt = int.Parse(page);
            int size = int.Parse(pageSize);

            if(size < 1)
            {
                return ServiceResponse.GetBadRequestResponse("Incorrect page size");
            };

            decimal totalSize = tests.Count();

            int pageCount = (int)Math.Ceiling(totalSize / size);

            if(pageInt < 1 || pageInt > pageCount)
            {
                return ServiceResponse.GetBadRequestResponse("Incorrect page number");
            }
            
            var data = await tests
                .Skip((pageInt - 1) * size)
                .Take(size)
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .ToListAsync();

            var model = new TestsListVM
            {
                TotalSize = (int)totalSize,
                PageSize = size,
                PageCount = pageCount,
                Page = pageInt,
                Tests = _mapper.Map<List<TestVM>>(data)
            };

            return ServiceResponse.GetOkResponse("Tests loaded", model);
        }
    }
}
