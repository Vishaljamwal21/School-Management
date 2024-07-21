using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;
        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;     
        }
        public bool Createteacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            return Save();
        }

        public bool Deleteteacher(Teacher teacher)
        {
            // Delete related records in TeacherAttendance
            var relatedAttendances = _context.TeacherAttendances.Where(a => a.TeacherId == teacher.TeacherId);
            _context.TeacherAttendances.RemoveRange(relatedAttendances);

            // Delete related records in TeacherSubject
            var relatedSubjects = _context.TeacherSubjects.Where(s => s.TeacherId == teacher.TeacherId);
            _context.TeacherSubjects.RemoveRange(relatedSubjects);

            // Now delete the teacher
            _context.Teachers.Remove(teacher);

            return Save(); // Assuming this method commits the changes to the database
        }


        public Teacher GetTeacher(int teacherId)
        {
            return _context.Teachers.Find(teacherId);
        }

        public ICollection<Teacher> GetTeachers()
        {
            return _context.Teachers.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool TeacherExists(int teacherId)
        {
            return _context.Teachers.Any(t=>t.TeacherId==teacherId);
        }

        public bool TeacherExists(string name)
        {
            return _context.Teachers.Any(t => t.Name == name);
        }

        public bool Updateteacher(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            return Save();
        }
    }
}
