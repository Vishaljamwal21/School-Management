using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface ITeacherSubjectRepository
    {
        ICollection<TeacherSubject> GetTeacherSubjects();
        TeacherSubject GetTeacherSubject(int Id);
        bool TeacherSubjectExists(int Id);
        bool CreateTeacherSubject(TeacherSubject teacherSubject);
        bool UpdateTeacherSubject(TeacherSubject teacherSubject);
        bool DeleteTeacherSubject(TeacherSubject teacherSubject);
        bool Save();

    }
}

