using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class TeacherRepository:Repository<Teacher>,ITeacherRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public TeacherRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 
        }
    }
}
