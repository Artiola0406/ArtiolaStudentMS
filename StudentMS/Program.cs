using System;

namespace StudentMS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Student Management System ===");

            var fileHandler = new FileHandler();
            var university = fileHandler.LoadUniversity();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Student Operations");
                Console.WriteLine("2. Professor Operations");
                Console.WriteLine("3. Course Operations");
                Console.WriteLine("4. Enrollment Operations");
                Console.WriteLine("5. Display Operations");
                Console.WriteLine("6. Save and Exit");
                Console.Write("Choose option: ");

                string mainChoice = Console.ReadLine() ?? "";

                try
                {
                    switch (mainChoice)
                    {
                        case "1": // STUDENT OPERATIONS
                            StudentOperations(university);
                            break;

                        case "2": // PROFESSOR OPERATIONS
                            ProfessorOperations(university);
                            break;

                        case "3": // COURSE OPERATIONS
                            CourseOperations(university);
                            break;

                        case "4": // ENROLLMENT OPERATIONS
                            EnrollmentOperations(university);
                            break;

                        case "5": // DISPLAY OPERATIONS
                            DisplayOperations(university);
                            break;

                        case "6": // SAVE AND EXIT
                            fileHandler.SaveUniversity(university);
                            exit = true;
                            Console.WriteLine("All data has been saved successfully. Goodbye!");
                            break;

                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void StudentOperations(University university)
        {
            Console.WriteLine("\n=== STUDENT OPERATIONS ===");
            Console.WriteLine("1. Register New Student");
            Console.WriteLine("2. Delete Student");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.Write("Enter student full name: ");
                    string name = Console.ReadLine() ?? "";
                    Console.Write("Enter student ID: ");
                    string id = Console.ReadLine() ?? "";
                    university.RegisterStudent(name, id);
                    break;

                case "2":
                    Console.Write("Enter student ID to delete: ");
                    string deleteId = Console.ReadLine() ?? "";
                    university.DeleteStudent(deleteId);
                    break;

                case "3":
                    return;
            }
        }

        static void ProfessorOperations(University university)
        {
            Console.WriteLine("\n=== PROFESSOR OPERATIONS ===");
            Console.WriteLine("1. Register New Professor");
            Console.WriteLine("2. Delete Professor");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.Write("Enter professor full name: ");
                    string name = Console.ReadLine() ?? "";
                    Console.Write("Enter department: ");
                    string department = Console.ReadLine() ?? "";
                    Console.Write("Enter office number: ");
                    string office = Console.ReadLine() ?? "";
                    Console.Write("Enter professor ID: ");
                    string id = Console.ReadLine() ?? "";
                    university.RegisterProfessor(name, department, office, id);
                    break;

                case "2":
                    Console.Write("Enter professor ID to delete: ");
                    string deleteId = Console.ReadLine() ?? "";
                    university.DeleteProfessor(deleteId);
                    break;

                case "3":
                    return;
            }
        }

        static void CourseOperations(University university)
        {
            Console.WriteLine("\n=== COURSE OPERATIONS ===");
            Console.WriteLine("1. Create New Course");
            Console.WriteLine("2. Assign Professor to Course");
            Console.WriteLine("3. Delete Course");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.Write("Enter course code: ");
                    string code = Console.ReadLine() ?? "";
                    Console.Write("Enter course name: ");
                    string name = Console.ReadLine() ?? "";
                    Console.Write("Enter credits: ");
                    int credits = int.Parse(Console.ReadLine() ?? "0");
                    Console.Write("Enter capacity: ");
                    int capacity = int.Parse(Console.ReadLine() ?? "0");
                    Console.Write("Enter professor ID (optional): ");
                    string profId = Console.ReadLine() ?? "";

                    university.CreateCourse(code, name, credits, capacity, profId);
                    break;

                case "2":
                    Console.Write("Enter course code: ");
                    string courseCode = Console.ReadLine() ?? "";
                    Console.Write("Enter professor ID: ");
                    string professorId = Console.ReadLine() ?? "";

                    var course = university.Courses.FirstOrDefault(c => c.CourseCode == courseCode);
                    var professor = university.Professors.FirstOrDefault(p => p.ID == professorId);

                    if (course != null && professor != null)
                    {
                        course.ProfessorID = professorId;
                        professor.AddCourse(courseCode);
                        Console.WriteLine($"Professor {professor.Name} assigned to course {courseCode}");
                    }
                    else
                    {
                        Console.WriteLine("Course or professor not found");
                    }
                    break;

                case "3":
                    Console.Write("Enter course code to delete: ");
                    string deleteCode = Console.ReadLine() ?? "";
                    university.DeleteCourse(deleteCode);
                    break;

                case "4":
                    return;
            }
        }

        static void EnrollmentOperations(University university)
        {
            Console.WriteLine("\n=== ENROLLMENT OPERATIONS ===");
            Console.WriteLine("1. Enroll Student in Course");
            Console.WriteLine("2. Unenroll Student from Course");
            Console.WriteLine("3. Set Grade for Student");
            Console.WriteLine("4. Calculate All GPAs");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.Write("Enter student ID: ");
                    string sId = Console.ReadLine() ?? "";
                    Console.Write("Enter course code: ");
                    string cCode = Console.ReadLine() ?? "";
                    university.EnrollStudentInCourse(sId, cCode);
                    break;

                case "2":
                    Console.Write("Enter student ID: ");
                    string unenrollSId = Console.ReadLine() ?? "";
                    Console.Write("Enter course code: ");
                    string unenrollCCode = Console.ReadLine() ?? "";
                    university.UnenrollStudentFromCourse(unenrollSId, unenrollCCode);
                    break;

                case "3":
                    Console.Write("Enter student ID: ");
                    string gradeSId = Console.ReadLine() ?? "";
                    Console.Write("Enter course code: ");
                    string gradeCCode = Console.ReadLine() ?? "";
                    Console.Write("Enter grade (0-100): ");
                    double grade = double.Parse(Console.ReadLine() ?? "0");
                    university.SetGrade(gradeSId, gradeCCode, grade);
                    break;

                case "4":
                    university.CalculateAllGPAs();
                    break;

                case "5":
                    return;
            }
        }

        static void DisplayOperations(University university)
        {
            Console.WriteLine("\n=== DISPLAY OPERATIONS ===");
            Console.WriteLine("1. Display All Students");
            Console.WriteLine("2. Display All Professors");
            Console.WriteLine("3. Display All Courses");
            Console.WriteLine("4. Search Students");
            Console.WriteLine("5. Generate Transcript");
            Console.WriteLine("6. Back to Main Menu");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    university.DisplayAllStudents();
                    break;

                case "2":
                    university.DisplayAllProfessors();
                    break;

                case "3":
                    university.DisplayAllCourses();
                    break;

                case "4":
                    Console.Write("Enter search term: ");
                    string term = Console.ReadLine() ?? "";
                    var results = university.SearchStudents(term);

                    Console.WriteLine($"Found {results.Count} student(s):");
                    foreach (var s in results)
                        Console.WriteLine($"  - {s.Name} ({s.ID})");
                    break;

                case "5":
                    Console.Write("Enter student ID: ");
                    string transId = Console.ReadLine() ?? "";
                    Console.WriteLine(university.GenerateTranscript(transId));
                    break;

                case "6":
                    return;
            }
        }
    }
}