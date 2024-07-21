using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string email);
        Admin Authenticate(string email, string password);
        Admin Register(Admin admin);
        Teacher AuthenticateTeacher(string email, string password);
        //Teacher RegisterTeacher(string email, string password);
    }
}
