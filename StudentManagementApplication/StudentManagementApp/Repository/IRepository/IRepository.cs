
using StudentManagementApp.Models;

namespace StudentManagementApp.Repository.IRepository
{
	public interface IRepository<T>where T:class
	{
		Task<T> GetAsync(string url, int id);
		Task<IEnumerable<T>> GetAllAsync(string url);
		Task<bool> CreateAsync(string url, T ObjToCreate);
		Task<bool> UpdateAsync(string url, T ObjToUpdate);
		Task<bool> DeleteAsync(string url, int id);
		Task<bool> IsUniqueUser(string Email);
		Task<object> Authenticate(string Email, string Password);
		Task<Admin> Register(string url, Admin admin);
        Task<bool> ExistAsync(string url, T ObjTocheck);

    }
}