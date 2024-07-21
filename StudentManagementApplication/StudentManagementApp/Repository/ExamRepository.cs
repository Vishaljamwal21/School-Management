using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class ExamRepository:Repository<Exam>,IExamRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public ExamRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
