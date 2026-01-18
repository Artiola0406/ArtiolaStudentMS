using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentMS
{
    public class University
    {
        public List<Student> Students { get; set; }
        public List<Professor> Professors { get; set; }
        public List<Course> Courses { get; set; }

        public University()
        {
            Students = new List<Student>();
            Professors = new List<Professor>();
            Courses = new List<Course>();
        }

        // === STUDENT OPERATIONS ===

        public void RegisterStudent(string name, string id)
        {
            if (Students.Any(s => s.ID == id))
                throw new ArgumentException($"Student with ID {id} already exists");

            string email = Person.GenerateEmail(name);
            var student = new Student(name, email, id);
            Students.Add(student);

            Console.WriteLine($"Student registered: {student.Name} ({student.ID})");
            Console.WriteLine($"Email generated: {student.Email}");
        }

        public bool DeleteStudent(string studentId)
        {
            var student = Students.FirstOrDefault(s => s.ID == studentId);
            if (student != null)
            {
                // Fshi enrollment-et e studentit nga të gjitha kurset
                foreach (var course in Courses)
                {
                    course.RemoveEnrollment(studentId);
                }

                Students.Remove(student);
                Console.WriteLine($"✅ Student {student.Name} was successfully deleted from the system.");
                return true;
            }
            else 
            {
                Console.WriteLine($"❌ Student with ID {studentId} not found.");
                return false;
            }
            return false;
        }

        // === PROFESSOR OPERATIONS ===

        public void RegisterProfessor(string name, string department, string officeNumber, string id)
        {
            if (Professors.Any(p => p.ID == id))
                throw new ArgumentException($"Professor with ID {id} already exists");

            var professor = new Professor(name, department, officeNumber, id);
            Professors.Add(professor);

            Console.WriteLine($"Professor registered: Prof. {professor.Name} ({professor.ID})");
            Console.WriteLine($"Email generated: {professor.Email}");
        }

        public bool DeleteProfessor(string professorId)
        {
            var professor = Professors.FirstOrDefault(p => p.ID == professorId);
            if (professor != null)
            {
                // Hiq profesorin nga kurset ku jep
                foreach (var course in Courses.Where(c => c.ProfessorID == professorId))
                {
                    course.ProfessorID = "";
                }

                Professors.Remove(professor);
                Console.WriteLine($"Professor {professor.Name} deleted successfully.");
                return true;
            }
            return false;
        }

        // === COURSE OPERATIONS ===

        public void CreateCourse(string code, string name, int credits, int capacity, string professorId = "")
        {
            if (Courses.Any(c => c.CourseCode == code))
                throw new ArgumentException($"Course with code {code} already exists");

            var course = new Course(code, name, credits, capacity, professorId);
            Courses.Add(course);

            // Nëse ka profesor, shto kursin te lista e tij
            if (!string.IsNullOrEmpty(professorId))
            {
                var professor = Professors.FirstOrDefault(p => p.ID == professorId);
                professor?.AddCourse(code);
            }

            Console.WriteLine($"Course created: {course.CourseCode} - {course.CourseName}");
        }

        public bool DeleteCourse(string courseCode)
        {
            var course = Courses.FirstOrDefault(c => c.CourseCode == courseCode);
            if (course != null)
            {
                // Hiq enrollment-et e të gjithë studentëve nga ky kurs
                foreach (var student in Students)
                {
                    student.RemoveEnrollment(courseCode);
                }

                // Hiq kursin nga lista e profesorit
                if (!string.IsNullOrEmpty(course.ProfessorID))
                {
                    var professor = Professors.FirstOrDefault(p => p.ID == course.ProfessorID);
                    professor?.RemoveCourse(courseCode);
                }

                Courses.Remove(course);
                Console.WriteLine($"Course {courseCode} deleted successfully.");
                return true;
            }
            return false;
        }

        // === ENROLLMENT OPERATIONS ===

        public void EnrollStudentInCourse(string studentId, string courseCode)
        {
            var student = Students.FirstOrDefault(s => s.ID == studentId);
            var course = Courses.FirstOrDefault(c => c.CourseCode == courseCode);

            if (student == null)
                throw new ArgumentException("Student not found");
            if (course == null)
                throw new ArgumentException("Course not found");
            if (!course.HasAvailableSeats())
                throw new InvalidOperationException("Course is full");

            var enrollment = new Enrollment(studentId, courseCode);
            student.AddEnrollment(enrollment);
            course.AddEnrollment(enrollment);

            Console.WriteLine($"Student {student.Name} enrolled in {course.CourseName}");
        }

        public bool UnenrollStudentFromCourse(string studentId, string courseCode)
        {
            var student = Students.FirstOrDefault(s => s.ID == studentId);
            var course = Courses.FirstOrDefault(c => c.CourseCode == courseCode);

            if (student == null || course == null)
                return false;

            bool removedFromStudent = student.RemoveEnrollment(courseCode);
            bool removedFromCourse = course.RemoveEnrollment(studentId);

            if (removedFromStudent && removedFromCourse)
            {
                Console.WriteLine($"Student {student.Name} unenrolled from {course.CourseName}");
                return true;
            }

            return false;
        }

        // === GRADE OPERATIONS ===

        public void SetGrade(string studentId, string courseCode, double grade)
        {
            var student = Students.FirstOrDefault(s => s.ID == studentId);
            if (student == null)
                throw new ArgumentException("Student not found");

            var enrollment = student.Enrollments.FirstOrDefault(e =>
                e.CourseCode == courseCode && e.StudentID == studentId);

            if (enrollment == null)
                throw new ArgumentException("Enrollment not found");

            enrollment.SetGrade(grade);
            Console.WriteLine($"Grade {grade} set for {student.Name} in {courseCode}");
        }

        // === DISPLAY OPERATIONS ===

        public void CalculateAllGPAs()
        {
            Console.WriteLine("\n=== GPA Calculation ===");
            foreach (var student in Students)
            {
                var gpa = student.CalculateGPA();
                Console.WriteLine($"{student.Name}: {gpa?.ToString("F2") ?? "No grades yet"}");
            }
        }

        public List<Student> SearchStudents(string searchTerm)
        {
            return Students.Where(s =>
                s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                s.ID.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public string GenerateTranscript(string studentId)
        {
            var student = Students.FirstOrDefault(s => s.ID == studentId);
            if (student == null)
                return "Student not found";

            var transcript = $"Transcript for {student.Name} (ID: {student.ID})\n";
            transcript += "=================================\n";

            foreach (var enrollment in student.Enrollments)
            {
                var course = Courses.FirstOrDefault(c => c.CourseCode == enrollment.CourseCode);
                transcript += $"{enrollment.CourseCode}: {course?.CourseName ?? "Unknown"} - Grade: {enrollment.Grade?.ToString() ?? "N/A"}\n";
            }

            transcript += $"Overall GPA: {student.CalculateGPA()?.ToString("F2") ?? "N/A"}";
            return transcript;
        }

        public void DisplayAllStudents()
        {
            Console.WriteLine("\n=== All Students ===");
            foreach (var student in Students)
            {
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Email: {student.Email}, Courses: {student.Enrollments.Count}");
            }
        }

        public void DisplayAllProfessors()
        {
            Console.WriteLine("\n=== All Professors ===");
            foreach (var prof in Professors)
            {
                Console.WriteLine($"ID: {prof.ID}, Name: Prof. {prof.Name}, Department: {prof.Department}, Courses: {string.Join(", ", prof.CoursesTeaching)}");
            }
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("\n=== All Courses ===");
            foreach (var course in Courses)
            {
                var professor = Professors.FirstOrDefault(p => p.ID == course.ProfessorID);
                Console.WriteLine($"Code: {course.CourseCode}, Name: {course.CourseName}, Prof: {professor?.Name ?? "None"}, Seats: {course.AvailableSeats()}/{course.Capacity}");
            }
        }
    }
}
