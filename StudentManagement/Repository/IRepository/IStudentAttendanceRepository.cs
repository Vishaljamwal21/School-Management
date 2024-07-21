using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface IStudentAttendanceRepository
    {
        ICollection<StudentAttendance> GetStudentAttendances();
        StudentAttendance GetStudentAttendance(int Id);
        bool StudentAttendanceExists(int Id);
        bool CreateStudentAttendance(StudentAttendance studentAttendance);
        bool UpdateStudentAttendance(StudentAttendance studentAttendance);
        bool DeleteStudentAttendance(StudentAttendance studentAttendance);
        ICollection<StudentAttendance> GetAttendanceByClassID(int classId);
        ICollection<StudentAttendance> GetAttendanceBysubjectId(int subjectId);
        bool Save();
    }
}
