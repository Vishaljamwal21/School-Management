using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class StudentRepository:Repository<Student>,IStudentRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public StudentRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;  
        }
    }
}
