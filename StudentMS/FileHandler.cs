using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace StudentMS
{
    /// <summary>
    /// KLASA FILEHANDLER - menaxhon ruajtjen dhe ngarkimin e të dhënave
    /// Kjo klasë është përgjegjëse për PERSISTENCE (mbajtjen e të dhënave)
    /// Përdor JSON për formatimin e të dhënave.
    /// Ky është shembull i DESIGN PATTERN "REPOSITORY".
    /// </summary>
    public class FileHandler
    {
        private const string FilePath = "university_data.json";

        // Ruaj University në JSON
        public void SaveUniversity(University university)
        {
            try
            {
                // Krijo një strukturë të thjeshtë për ruajtje
                var saveData = new
                {
                    Students = university.Students.Select(s => new
                    {
                        s.Name,
                        s.Email,
                        s.ID,
                        Enrollments = s.Enrollments.Select(e => new
                        {
                            e.StudentID,
                            e.CourseCode,
                            e.Grade
                        }).ToList()
                    }).ToList(),

                    Professors = university.Professors.Select(p => new
                    {
                        p.Name,
                        p.Email,
                        p.ID,
                        p.Department,
                        p.OfficeNumber,
                        p.CoursesTeaching
                    }).ToList(),

                    Courses = university.Courses.Select(c => new
                    {
                        c.CourseCode,
                        c.CourseName,
                        c.Credits,
                        c.Capacity,
                        c.ProfessorID,
                        Enrollments = c.Enrollments.Select(e => new
                        {
                            e.StudentID,
                            e.CourseCode,
                            e.Grade
                        }).ToList()
                    }).ToList()
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                string jsonContent = JsonSerializer.Serialize(saveData, options);
                File.WriteAllText(FilePath, jsonContent);

                Console.WriteLine($"✅ Data saved successfully to {FilePath}");
                Console.WriteLine($"   Students: {university.Students.Count}");
                Console.WriteLine($"   Professors: {university.Professors.Count}");
                Console.WriteLine($"   Courses: {university.Courses.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving data: {ex.Message}");
            }
        }

        // Ngarko University nga JSON
        public University LoadUniversity()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("📁 No saved data found. Starting with empty system.");
                return new University();
            }

            try
            {
                string jsonContent = File.ReadAllText(FilePath);

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Console.WriteLine("📁 File is empty. Starting fresh.");
                    return new University();
                }

                // Deserialize the JSON
                using JsonDocument document = JsonDocument.Parse(jsonContent);
                var root = document.RootElement;

                var university = new University();

                // Load Students
                if (root.TryGetProperty("Students", out var studentsElement))
                {
                    foreach (var studentElement in studentsElement.EnumerateArray())
                    {
                        if (studentElement.TryGetProperty("Name", out var nameProp) &&
                            studentElement.TryGetProperty("Email", out var emailProp) &&
                            studentElement.TryGetProperty("ID", out var idProp))
                        {
                            string name = nameProp.GetString() ?? "";
                            string email = emailProp.GetString() ?? "";
                            string id = idProp.GetString() ?? "";

                            var student = new Student(name, email, id);
                            university.Students.Add(student);

                            // Load student enrollments
                            if (studentElement.TryGetProperty("Enrollments", out var enrollmentsProp))
                            {
                                foreach (var enrollElement in enrollmentsProp.EnumerateArray())
                                {
                                    if (enrollElement.TryGetProperty("StudentID", out var sIdProp) &&
                                        enrollElement.TryGetProperty("CourseCode", out var cCodeProp))
                                    {
                                        string studentId = sIdProp.GetString() ?? "";
                                        string courseCode = cCodeProp.GetString() ?? "";

                                        var enrollment = new Enrollment(studentId, courseCode);

                                        if (enrollElement.TryGetProperty("Grade", out var gradeProp) &&
                                            gradeProp.ValueKind != JsonValueKind.Null)
                                        {
                                            if (gradeProp.TryGetDouble(out double grade))
                                            {
                                                enrollment.Grade = grade;
                                            }
                                        }

                                        student.Enrollments.Add(enrollment);
                                    }
                                }
                            }
                        }
                    }
                }

                // Load Professors
                if (root.TryGetProperty("Professors", out var professorsElement))
                {
                    foreach (var profElement in professorsElement.EnumerateArray())
                    {
                        if (profElement.TryGetProperty("Name", out var nameProp) &&
                            profElement.TryGetProperty("Department", out var deptProp) &&
                            profElement.TryGetProperty("OfficeNumber", out var officeProp) &&
                            profElement.TryGetProperty("ID", out var idProp))
                        {
                            string name = nameProp.GetString() ?? "";
                            string department = deptProp.GetString() ?? "";
                            string office = officeProp.GetString() ?? "";
                            string id = idProp.GetString() ?? "";

                            var professor = new Professor(name, department, office, id);

                            // Load professor's email if exists
                            if (profElement.TryGetProperty("Email", out var emailProp))
                            {
                                professor.Email = emailProp.GetString() ?? professor.Email;
                            }

                            // Load courses teaching
                            if (profElement.TryGetProperty("CoursesTeaching", out var coursesProp))
                            {
                                foreach (var courseElement in coursesProp.EnumerateArray())
                                {
                                    professor.CoursesTeaching.Add(courseElement.GetString() ?? "");
                                }
                            }

                            university.Professors.Add(professor);
                        }
                    }
                }

                // Load Courses
                if (root.TryGetProperty("Courses", out var coursesElement))
                {
                    foreach (var courseElement in coursesElement.EnumerateArray())
                    {
                        if (courseElement.TryGetProperty("CourseCode", out var codeProp) &&
                            courseElement.TryGetProperty("CourseName", out var nameProp) &&
                            courseElement.TryGetProperty("Credits", out var creditsProp) &&
                            courseElement.TryGetProperty("Capacity", out var capacityProp))
                        {
                            string code = codeProp.GetString() ?? "";
                            string name = nameProp.GetString() ?? "";
                            int credits = creditsProp.GetInt32();
                            int capacity = capacityProp.GetInt32();

                            string professorId = "";
                            if (courseElement.TryGetProperty("ProfessorID", out var profIdProp))
                            {
                                professorId = profIdProp.GetString() ?? "";
                            }

                            var course = new Course(code, name, credits, capacity, professorId);

                            // Load course enrollments
                            if (courseElement.TryGetProperty("Enrollments", out var enrollmentsProp))
                            {
                                foreach (var enrollElement in enrollmentsProp.EnumerateArray())
                                {
                                    if (enrollElement.TryGetProperty("StudentID", out var sIdProp) &&
                                        enrollElement.TryGetProperty("CourseCode", out var cCodeProp))
                                    {
                                        string studentId = sIdProp.GetString() ?? "";
                                        string courseCode = cCodeProp.GetString() ?? "";

                                        var enrollment = new Enrollment(studentId, courseCode);

                                        if (enrollElement.TryGetProperty("Grade", out var gradeProp) &&
                                            gradeProp.ValueKind != JsonValueKind.Null)
                                        {
                                            if (gradeProp.TryGetDouble(out double grade))
                                            {
                                                enrollment.Grade = grade;
                                            }
                                        }

                                        // Only add if not already added by student loading
                                        if (!course.Enrollments.Any(e => e.StudentID == studentId && e.CourseCode == courseCode))
                                        {
                                            course.Enrollments.Add(enrollment);
                                        }

                                        // Also ensure student has this enrollment
                                        var student = university.Students.FirstOrDefault(s => s.ID == studentId);
                                        if (student != null && !student.Enrollments.Any(e => e.CourseCode == courseCode))
                                        {
                                            student.Enrollments.Add(enrollment);
                                        }
                                    }
                                }
                            }

                            university.Courses.Add(course);
                        }
                    }
                }

                Console.WriteLine($"✅ Data loaded successfully from {FilePath}");
                Console.WriteLine($"   Students: {university.Students.Count}");
                Console.WriteLine($"   Professors: {university.Professors.Count}");
                Console.WriteLine($"   Courses: {university.Courses.Count}");

                return university;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading data: {ex.Message}");
                Console.WriteLine("Starting with empty system.");
                return new University();
            }
        }

        // Metodë për të fshirë të gjitha të dhënat (opsionale)
        public void ClearAllData()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                Console.WriteLine("✅ All data has been cleared.");
            }
            else
            {
                Console.WriteLine("❌ No data file found to clear.");
            }
        }

        // Metodë për të kontrolluar nëse ekzistojnë të dhëna
        public bool HasSavedData()
        {
            return File.Exists(FilePath) && new FileInfo(FilePath).Length > 0;
        }
    }
}