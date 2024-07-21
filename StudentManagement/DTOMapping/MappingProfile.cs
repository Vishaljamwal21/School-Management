using AutoMapper;
using StudentManagement.Models.DTO;
using StudentManagement.Models;

namespace StudentManagement.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Class, ClassDTO>().ReverseMap();
            CreateMap<Exam, ExamDTO>().ReverseMap();
            CreateMap<Expense, ExpenseDTO>().ReverseMap();
            CreateMap<Fee, FeeDTO>().ReverseMap();
            CreateMap<StudentAttendance, StudentAttendanceDTO>().ReverseMap();
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Subject,SubjectDTO>().ReverseMap();
            CreateMap<Teacher,TeacherDTO>().ReverseMap();
            CreateMap<TeacherAttendance,TeacherAttendanceDTO>().ReverseMap();
            CreateMap<TeacherSubject,TeacherSubjectDTO>().ReverseMap();
        }
    }
}
