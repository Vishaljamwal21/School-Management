using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface ITeacherAttendanceRepository
    {
        ICollection<TeacherAttendance> GetTeacherAttendances();
        TeacherAttendance GetTeacherAttendance(int Id);
        bool TeacherAttendanceExists(int Id);
        bool CreateTeacherAttendance(TeacherAttendance teacherAttendance);
        bool UpdateTeacherAttendance(TeacherAttendance teacherAttendance);
        bool DeleteTeacherAttendance(TeacherAttendance teacherAttendance);
        ICollection<TeacherAttendance> GetAttendanceByTeacherId(int teacherid);
        bool Save();
    }
}
