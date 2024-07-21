using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class StudentAttendanceRepository:Repository<StudentAttendance>,IStudentAttendanceRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public StudentAttendanceRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 
        }
    }
}
