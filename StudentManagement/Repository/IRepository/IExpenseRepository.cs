using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface IExpenseRepository
    {
        ICollection<Expense> GetExpenses();
        Expense GetExpense(int ExpenseId);
        bool ExpenseExists(int ExpenseId);
        bool CreateExpense(Expense expense);
        bool UpdateExpense(Expense expense);
        bool DeleteExpense(Expense expense);
        ICollection<Expense> GetExpenseByClassID(int classId);
        ICollection<Expense> GetExpenseBysubjectId(int subjectId);
        bool Save();
    }
}
