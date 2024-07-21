using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;      
        }
        public bool Createstudent(Student student)
        {
            _context.Students.Add(student);
                return Save();
        }

        public bool Deletestudent(Student student)
        {
            _context.Students.Remove(student);
            return Save();
        }

        public Student GetStudent(int studentId)
        {
            return _context.Students.Find(studentId);
        }

        public ICollection<Student> GetStudentByClassId(int classId)
        {
            return _context.Students.Where(s => s.ClassId == classId).ToList();
        }

        public ICollection<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool StudentExists(int studentId)
        {
            return _context.Students.Any(st=>st.StudentId==studentId);
        }

        public bool StudentExists(string name)
        {
            return _context.Students.Any(st=>st.Name==name);
        }

        public bool Updatestudent(Student student)
        {
            _context.Students.Update(student);
            return Save();
        }
    }
}
