using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            return Save();
        }

        public bool DeleteExpense(Expense expense)
        {
            _context.Expenses.Remove(expense);
            return Save();
        }

        public bool ExpenseExists(int ExpenseId)
        {
            return _context.Expenses.Any(ex=>ex.ExpenseId==ExpenseId);
        }

        public Expense GetExpense(int ExpenseId)
        {
            return _context.Expenses.Find(ExpenseId);
        }

        public ICollection<Expense> GetExpenseByClassID(int classId)
        {
            return _context.Expenses.Where(ex => ex.ClassId == classId).ToList();
        }

        public ICollection<Expense> GetExpenseBysubjectId(int subjectId)
        {
            return _context.Expenses.Where(ex => ex.SubjectId == subjectId).ToList();
        }

        public ICollection<Expense> GetExpenses()
        {
            return _context.Expenses.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool UpdateExpense(Expense expense)
        {
            _context.Expenses.Update(expense);
            return Save();
        }
    }
}
