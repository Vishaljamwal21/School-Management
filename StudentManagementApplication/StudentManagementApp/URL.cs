namespace StudentManagementApp
{
	public static class URL
	{
		public static string APIBaseURl = "https://localhost:7028/";
		public static string StudentAPIPath = APIBaseURl + "api/Student";
		public static string ClassAPIPath = APIBaseURl + "api/Class";
		public static string ExamAPIPath = APIBaseURl + "api/Exam";
		public static string ExpenseAPIPath = APIBaseURl + "api/Expense";
		public static string FeeAPIPath = APIBaseURl + "api/Fee";
		public static string StudentAttendanceAPIPath = APIBaseURl + "api/StudentAttendance";
		public static string SubjectAPIPath = APIBaseURl + "api/Subject";
		public static string TeacherAPIPath = APIBaseURl + "api/Teacher";
		public static string TeacherAttendanceAPIPath = APIBaseURl + "api/TeacherAttendance";
		public static string TeacherSubjectAPIPath = APIBaseURl + "api/TeacherSubject";
		// UserController URLs
		public static string AuthenticateAPIPath = APIBaseURl + "api/User/Authenticate";
		public static string RegisterAPIPath = APIBaseURl + "api/User/Register";
	}
}
