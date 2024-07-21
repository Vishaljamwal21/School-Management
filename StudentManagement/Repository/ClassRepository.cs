using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;
        public ClassRepository(ApplicationDbContext context)
        {
            _context = context;      
        }
        public bool ClassExists(int classId)
        {
            return _context.Classes.Any(cl => cl.ClassId == classId);
        }

        public bool ClassExists(string className)
        {
            return _context.Classes.Any(cl => cl.ClassName == className);
        }

        public bool CreateClass(Class Class)
        {
            _context.Classes.Add(Class);
            return Save();
        }

        public bool DeleteClass(Class Class)
        {
            _context.Classes.Remove(Class);
            return Save();
        }

        public Class GetClass(int classId)
        {
            return _context.Classes.Find(classId);
        }

        public ICollection<Class> GetClasses()
        {
            return _context.Classes.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool UpdateClass(Class Class)
        {
            _context.Classes.Update(Class);
            return Save();
        }
        public Class GetClassByName(string className)
        {
            return _context.Classes.FirstOrDefault(c => c.ClassName == className);
        }
    }
}
