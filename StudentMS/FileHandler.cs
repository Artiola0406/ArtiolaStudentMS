using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StudentMS
{
    public class FileHandler
    {
        private const string FilePath = "university_data.txt";

        public void SaveUniversity(University university)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    // Shkruaj studentët
                    writer.WriteLine("=== STUDENTS ===");
                    foreach (var student in university.Students)
                    {
                        writer.WriteLine($"STUDENT|{student.Name}|{student.Email}|{student.ID}");

                        foreach (var enrollment in student.Enrollments)
                        {
                            writer.WriteLine($"ENROLLMENT|{enrollment.StudentID}|{enrollment.CourseCode}|{enrollment.Grade}");
                        }
                    }

                    // Shkruaj profesorët
                    writer.WriteLine("\n=== PROFESSORS ===");
                    foreach (var professor in university.Professors)
                    {
                        writer.WriteLine($"PROFESSOR|{professor.Name}|{professor.Department}|{professor.OfficeNumber}|{professor.ID}");

                        foreach (var course in professor.CoursesTeaching)
                        {
                            writer.WriteLine($"TEACHES|{professor.ID}|{course}");
                        }
                    }

                    // Shkruaj kurset
                    writer.WriteLine("\n=== COURSES ===");
                    foreach (var course in university.Courses)
                    {
                        writer.WriteLine($"COURSE|{course.CourseCode}|{course.CourseName}|{course.Credits}|{course.Capacity}|{course.ProfessorID}");
                    }
                }

                Console.WriteLine($"Data saved to {FilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save error: {ex.Message}");
            }
        }

        public University LoadUniversity()
        {
            if (!File.Exists(FilePath))
                return new University();

            try
            {
                var university = new University();

                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("STUDENT|"))
                        {
                            var parts = line.Split('|');
                            if (parts.Length >= 4)
                            {
                                var student = new Student(parts[1], parts[2], parts[3]);
                                university.Students.Add(student);
                            }
                        }
                        else if (line.StartsWith("PROFESSOR|"))
                        {
                            var parts = line.Split('|');
                            if (parts.Length >= 5)
                            {
                                var professor = new Professor(parts[1], parts[2], parts[3], parts[4]);
                                university.Professors.Add(professor);
                            }
                        }
                        else if (line.StartsWith("COURSE|"))
                        {
                            var parts = line.Split('|');
                            if (parts.Length >= 6)
                            {
                                int credits = int.Parse(parts[3]);
                                int capacity = int.Parse(parts[4]);
                                var course = new Course(parts[1], parts[2], credits, capacity, parts[5]);
                                university.Courses.Add(course);
                            }
                        }
                        else if (line.StartsWith("ENROLLMENT|"))
                        {
                            var parts = line.Split('|');
                            if (parts.Length >= 4)
                            {
                                var student = university.Students.FirstOrDefault(s => s.ID == parts[1]);
                                var course = university.Courses.FirstOrDefault(c => c.CourseCode == parts[2]);

                                if (student != null && course != null)
                                {
                                    var enrollment = new Enrollment(parts[1], parts[2]);
                                    if (double.TryParse(parts[3], out double grade))
                                        enrollment.Grade = grade;

                                    student.Enrollments.Add(enrollment);
                                    course.Enrollments.Add(enrollment);
                                }
                            }
                        }
                        else if (line.StartsWith("TEACHES|"))
                        {
                            var parts = line.Split('|');
                            if (parts.Length >= 3)
                            {
                                var professor = university.Professors.FirstOrDefault(p => p.ID == parts[1]);
                                if (professor != null)
                                {
                                    professor.CoursesTeaching.Add(parts[2]);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"Data loaded from {FilePath}");
                return university;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load error: {ex.Message}");
                return new University();
            }
        }
    }
}