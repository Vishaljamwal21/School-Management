using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface IStudentRepository
    {
        ICollection<Student> GetStudents();
        Student GetStudent(int studentId);
        bool StudentExists(int studentId);
        bool StudentExists(string name);
        bool Createstudent(Student student);
        bool Updatestudent(Student student);
        bool Deletestudent(Student student);
        ICollection<Student> GetStudentByClassId(int classId);
        bool Save();
    }
}
