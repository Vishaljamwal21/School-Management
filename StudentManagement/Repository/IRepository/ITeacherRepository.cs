using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface ITeacherRepository
    {
        ICollection<Teacher> GetTeachers();
        Teacher GetTeacher(int teacherId);
        bool TeacherExists(int teacherId);
        bool TeacherExists(string name);
        bool Createteacher(Teacher teacher);
        bool Updateteacher(Teacher teacher);
        bool Deleteteacher(Teacher teacher);
        bool Save();
    }
}
