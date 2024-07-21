using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class TeacherSubjectRepository:Repository<TeacherSubject>,ITeacherSubjectRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public TeacherSubjectRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;  
        }
    }
}
