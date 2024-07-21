using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface IClassRepository
    {
        ICollection<Class> GetClasses();
        Class GetClass(int classId);
        bool ClassExists(int classId);
        bool ClassExists(string className);
        Class GetClassByName(string className);
        bool CreateClass(Class Class);
        bool UpdateClass(Class Class);              
        bool DeleteClass(Class Class);
        bool Save();
    }
}
