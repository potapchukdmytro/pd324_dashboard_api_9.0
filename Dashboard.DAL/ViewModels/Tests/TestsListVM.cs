namespace Dashboard.DAL.ViewModels.Tests
{
    public class TestsListVM
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalSize { get; set; }
        public List<TestVM> Tests { get; set; } = [];
    }
}
