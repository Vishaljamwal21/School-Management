using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class TeacherSubjectRepository : ITeacherSubjectRepository
    {
        private readonly ApplicationDbContext _context;
        public TeacherSubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public ICollection<TeacherSubject> GetTeacherSubjects()
        {
            return _context.TeacherSubjects.ToList();
        }

        public TeacherSubject GetTeacherSubject(int Id)
        {
            return _context.TeacherSubjects.Find(Id);
        }

        public bool TeacherSubjectExists(int Id)
        {
            return _context.TeacherSubjects.Any(ts=>ts.Id==Id);
        }

        public bool CreateTeacherSubject(TeacherSubject teacherSubject)
        {
            _context.TeacherSubjects.Add(teacherSubject);
            return Save();
        }

        public bool UpdateTeacherSubject(TeacherSubject teacherSubject)
        {
            _context.TeacherSubjects.Update(teacherSubject);
            return Save();
        }

        public bool DeleteTeacherSubject(TeacherSubject teacherSubject)
        {
            _context.TeacherSubjects.Remove(teacherSubject);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }
    }
}
