using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class ClassRepository:Repository<Class>,IClassRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public ClassRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;  
        }
    }
}
