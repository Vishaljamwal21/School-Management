using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class FeeRepository:Repository<Fee>,IFeeRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public FeeRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
