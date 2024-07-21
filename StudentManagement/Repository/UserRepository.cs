using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;      
        }
        public Admin Register(Admin admin)
        {

            _context.Admins.Add(admin);
            _context.SaveChanges();

            return admin;
        }

        public Admin Authenticate(string email, string password)
        {
            var userindb = _context.Admins.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (userindb == null)
                return null;
            // JWT will be added here

            userindb.Password = ""; 
            return userindb;
        }

        public Teacher AuthenticateTeacher(string email, string password)
        {
            var teacherindb = _context.Teachers.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (teacherindb == null)
                return null;
            // JWT will be added here

            teacherindb.Password = "";
            return teacherindb;
        }

        public bool IsUniqueUser(string email)
        {
            return !_context.Admins.Any(x => x.Email == email);
        }

        public bool IsUniqueTeacher(string email)
        {
            return !_context.Teachers.Any(x => x.Email == email);
        }
    }
}
