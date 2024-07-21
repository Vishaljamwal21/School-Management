using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class TeacherAttendance
{
    public int Id { get; set; }

    public int? TeacherId { get; set; }

    public bool? Status { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Teacher Teacher { get; set; }
}
