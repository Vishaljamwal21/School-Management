using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class TeacherAttendanceRepository:Repository<TeacherAttendance>,ITeacherAttendanceRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public TeacherAttendanceRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
