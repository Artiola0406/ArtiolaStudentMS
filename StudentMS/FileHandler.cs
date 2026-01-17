using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace StudentMS
{
    public class FileHandler
    {
        private const string FilePath = "university_data.json";

        private class UniversityData
        {
            public List<Student> Students { get; set; } = new List<Student>();
            public List<Professor> Professors { get; set; } = new List<Professor>();
            public List<Course> Courses { get; set; } = new List<Course>();
            public List<Enrollment> AllEnrollments { get; set; } = new List<Enrollment>();
        }

        public void SaveUniversity(University university)
        {
            var allEnrollments = new List<Enrollment>();
            foreach (var student in university.Students)
            {
                allEnrollments.AddRange(student.Enrollments);
            }

            var data = new UniversityData
            {
                Students = university.Students,
                Professors = university.Professors,
                Courses = university.Courses,
                AllEnrollments = allEnrollments
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonContent = JsonSerializer.Serialize(data, options);
            File.WriteAllText(FilePath, jsonContent);
            Console.WriteLine($"Data saved to {FilePath}");
        }

        public University LoadUniversity()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("No saved data found. Starting fresh.");
                return new University();
            }

            string jsonContent = File.ReadAllText(FilePath);
            var data = JsonSerializer.Deserialize<UniversityData>(jsonContent);

            if (data == null)
                return new University();

            var university = new University
            {
                Students = data.Students ?? new List<Student>(),
                Professors = data.Professors ?? new List<Professor>(),
                Courses = data.Courses ?? new List<Course>()
            };

            // Restauro enrollment-et
            if (data.AllEnrollments != null)
            {
                foreach (var enrollment in data.AllEnrollments)
                {
                    var student = university.Students.FirstOrDefault(s => s.ID == enrollment.StudentID);
                    var course = university.Courses.FirstOrDefault(c => c.CourseCode == enrollment.CourseCode);

                    if (student != null && !student.Enrollments.Any(e =>
                        e.StudentID == enrollment.StudentID && e.CourseCode == enrollment.CourseCode))
                    {
                        student.Enrollments.Add(enrollment);
                    }

                    if (course != null && !course.Enrollments.Any(e =>
                        e.StudentID == enrollment.StudentID && e.CourseCode == enrollment.CourseCode))
                    {
                        course.Enrollments.Add(enrollment);
                    }
                }
            }

            Console.WriteLine($"Data loaded from {FilePath}");
            return university;
        }
    }
}