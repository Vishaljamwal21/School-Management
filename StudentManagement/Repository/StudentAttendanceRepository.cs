using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class StudentAttendanceRepository : IStudentAttendanceRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentAttendanceRepository(ApplicationDbContext context)
        {
            _context = context;       
        }
        public bool CreateStudentAttendance(StudentAttendance studentAttendance)
        {
            _context.StudentAttendances.Add(studentAttendance);
            return Save();
        }

        public bool DeleteStudentAttendance(StudentAttendance studentAttendance)
        {
            _context.StudentAttendances.Remove(studentAttendance);
            return Save();

        }

        public ICollection<StudentAttendance> GetAttendanceByClassID(int classId)
        {
            return _context.StudentAttendances
                          .Where(sa => sa.ClassId == classId)
                          .ToList();
        }

        public ICollection<StudentAttendance> GetAttendanceBysubjectId(int subjectId)
        {
            return _context.StudentAttendances.Where(sa => sa.SubjectId == subjectId).ToList();
        }

        public StudentAttendance GetStudentAttendance(int Id)
        {
            return _context.StudentAttendances.Find(Id);
        }

        public ICollection<StudentAttendance> GetStudentAttendances()
        {
            return _context.StudentAttendances.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool StudentAttendanceExists(int Id)
        {
            return _context.StudentAttendances.Any(sa => sa.Id == Id);
        }

        public bool UpdateStudentAttendance(StudentAttendance studentAttendance)
        {
            _context.StudentAttendances.Update(studentAttendance);
            return Save();
        }
    }
}
