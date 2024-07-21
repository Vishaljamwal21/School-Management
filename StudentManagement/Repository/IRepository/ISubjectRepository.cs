using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface ISubjectRepository
    {
        ICollection<Subject> GetSubjects();
        Subject GetSubject(int subjectId);
        bool SubjectExists(int subjectId);
        bool SubjectExists(string subjectName);
        bool CreateSubject(Subject subject);
        bool UpdateSubject(Subject subject);
        bool DeleteSubject(Subject subject);
        bool Save();
    }
}
