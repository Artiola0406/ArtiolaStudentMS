namespace StudentMS
{
    public class Enrollment
    {
        public string StudentID { get; set; }
        public string CourseCode { get; set; }
        public double? Grade { get; set; }

        public Enrollment(string studentId, string courseCode)
        {
            StudentID = studentId;
            CourseCode = courseCode;
            Grade = null;
        }

        public void SetGrade(double grade)
        {
            if (grade < 0 || grade > 100)
                throw new ArgumentException("Grade must be between 0 and 100");
            Grade = grade;
        }

        public override string ToString()
        {
            return $"{StudentID} in {CourseCode}: {Grade?.ToString() ?? "No grade"}";
        }
    }
}