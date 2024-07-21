using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class AdminRepository:Repository<Admin>,IAdminRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public AdminRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
