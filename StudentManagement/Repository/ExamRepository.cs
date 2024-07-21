using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;
        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;  
        }
        public bool CreateExam(Exam exam)
        {
            _context.Exams.Add(exam);
            return Save();
        }

        public bool DeleteExam(Exam exam)
        {
            _context.Exams.Remove(exam);
            return Save();
        }

        public bool ExamExists(int ExamId)
        {
            return _context.Exams.Any(ex=>ex.ExamId==ExamId);
        }

        public Exam GetExam(int ExamId)
        {
            return _context.Exams.Find(ExamId);
        }

        public ICollection<Exam> GetExamByClassID(int classId)
        {
            return _context.Exams.Where(ex => ex.ClassId == classId).ToList();
        }

        public ICollection<Exam> GetExamBysubjectId(int subjectId)
        {
            return _context.Exams.Where(ex => ex.SubjectId == subjectId).ToList();
        }

        public ICollection<Exam> GetExams()
        {
            return _context.Exams.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool UpdateExam(Exam exam)
        {
            _context.Exams.Update(exam);
            return Save();
        }
    }
}
