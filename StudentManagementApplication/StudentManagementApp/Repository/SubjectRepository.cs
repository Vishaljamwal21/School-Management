using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class SubjectRepository:Repository<Subject>,ISubjectRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public SubjectRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
