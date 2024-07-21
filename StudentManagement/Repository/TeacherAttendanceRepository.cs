using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class TeacherAttendanceRepository : ITeacherAttendanceRepository
    {
        private readonly ApplicationDbContext _context;
        public TeacherAttendanceRepository(ApplicationDbContext context)
        {
            _context = context;      
        }
        public bool CreateTeacherAttendance(TeacherAttendance teacherAttendance)
        {
            _context.TeacherAttendances.Add(teacherAttendance);
            return Save();
        }

        public bool DeleteTeacherAttendance(TeacherAttendance teacherAttendance)
        {
            _context.TeacherAttendances.Remove(teacherAttendance);
            return Save();
        }

        public ICollection<TeacherAttendance> GetAttendanceByTeacherId(int teacherid)
        {
            return _context.TeacherAttendances
                           .Where(ta => ta.TeacherId == teacherid)
                           .ToList();
        }

        public TeacherAttendance GetTeacherAttendance(int Id)
        {
            return _context.TeacherAttendances.Find(Id);
        }

        public ICollection<TeacherAttendance> GetTeacherAttendances()
        {
            return _context.TeacherAttendances.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool TeacherAttendanceExists(int Id)
        {
            return _context.TeacherAttendances.Any(ta=>ta.Id==Id);
        }

        public bool UpdateTeacherAttendance(TeacherAttendance teacherAttendance)
        {
            _context.TeacherAttendances.Update(teacherAttendance);
            return Save();
        }
    }
}
