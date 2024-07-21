using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string Name { get; set; }

    public DateOnly? Dob { get; set; }

    public string Gender { get; set; }

    public long? Mobile { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }

    public string Token { get; set; }

    public virtual ICollection<TeacherAttendance> TeacherAttendances { get; set; } = new List<TeacherAttendance>();

    public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}
