using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Repository
{
	public class ExpenseRepository:Repository<Expense>,IExpenseRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public ExpenseRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
