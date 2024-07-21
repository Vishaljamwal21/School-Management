using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface IExamRepository
    {
        ICollection<Exam> GetExams();
        Exam GetExam(int ExamId);
        bool ExamExists(int ExamId);
        bool CreateExam(Exam exam);
        bool UpdateExam(Exam exam);
        bool DeleteExam(Exam exam);
        ICollection<Exam> GetExamByClassID(int classId);
        ICollection<Exam> GetExamBysubjectId(int subjectId);
        bool Save();
    }
}
