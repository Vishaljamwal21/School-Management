using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;
        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            return Save();
        }

        public bool DeleteSubject(Subject subject)
        {
            _context.Subjects.Remove(subject);
            return Save();
        }

        public Subject GetSubject(int subjectId)
        {
            return _context.Subjects.Find(subjectId);
        }

        public ICollection<Subject> GetSubjects()
        {
            return _context.Subjects
                            .Include(s => s.Class) 
                            .ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool SubjectExists(int subjectId)
        {
            return _context.Subjects.Any(sb=>sb.SubjectId==subjectId);
        }

        public bool SubjectExists(string subjectName)
        {
            return _context.Subjects.Any(sb=>sb.SubjectName==subjectName);
        }

        public bool UpdateSubject(Subject subject)
        {
            _context.Subjects.Update(subject);
            return Save();
        }
    }
}
