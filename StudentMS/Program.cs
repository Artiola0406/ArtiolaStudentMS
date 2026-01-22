using System;
using System.Linq;

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
                Console.Clear();
                Console.WriteLine("\n==== MAIN MENU ====");
                Console.WriteLine("1. Student Operations");
                Console.WriteLine("2. Professor Operations");
                Console.WriteLine("3. Course Operations");
                Console.WriteLine("4. Enrollment Operations");
                Console.WriteLine("5. Display Operations");
                Console.WriteLine("6. Save and Exit");
                Console.Write("Choose option (1-6): ");

                string choice = Console.ReadLine()?.Trim() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            StudentOperations(university);
                            break;

                        case "2":
                            ProfessorOperations(university);
                            break;

                        case "3":
                            CourseOperations(university);
                            break;

                        case "4":
                            EnrollmentOperations(university);
                            break;

                        case "5":
                            DisplayOperations(university);
                            break;

                        case "6":
                            fileHandler.SaveUniversity(university);
                            exit = true;
                            Console.WriteLine("All data has been saved successfully. Goodbye!");
                            break;

                        default:
                            Console.WriteLine("⚠️ Invalid option. Please choose 1-6.");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void StudentOperations(University university)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("\n=== STUDENT OPERATIONS ===");
                Console.WriteLine("1. Register New Student");
                Console.WriteLine("2. Delete Student");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("=== REGISTER NEW STUDENT ===\n");
                        Console.Write("Enter student full name (e.g., Artiola Qollaku): ");
                        string name = Console.ReadLine() ?? "";
                        Console.Write("Enter student ID: ");
                        string id = Console.ReadLine() ?? "";

                        university.RegisterStudent(name, id);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=== DELETE STUDENT ===\n");
                        Console.Write("Enter student ID to delete: ");
                        string deleteId = Console.ReadLine() ?? "";
                        university.DeleteStudent(deleteId);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "3":
                        backToMain = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ProfessorOperations(University university)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("\n=== PROFESSOR OPERATIONS ===");
                Console.WriteLine("1. Register New Professor");
                Console.WriteLine("2. Delete Professor");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("=== REGISTER NEW PROFESSOR ===\n");
                        Console.Write("Enter professor full name: ");
                        string name = Console.ReadLine() ?? "";
                        Console.Write("Enter department: ");
                        string department = Console.ReadLine() ?? "";
                        Console.Write("Enter office number: ");
                        string office = Console.ReadLine() ?? "";
                        Console.Write("Enter professor ID: ");
                        string id = Console.ReadLine() ?? "";

                        university.RegisterProfessor(name, department, office, id);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=== DELETE PROFESSOR ===\n");
                        Console.Write("Enter professor ID to delete: ");
                        string deleteId = Console.ReadLine() ?? "";
                        university.DeleteProfessor(deleteId);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "3":
                        backToMain = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void CourseOperations(University university)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
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
                        Console.Clear();
                        Console.WriteLine("=== CREATE NEW COURSE ===\n");
                        Console.Write("Enter course code: ");
                        string code = Console.ReadLine() ?? "";
                        Console.Write("Enter course name: ");
                        string name = Console.ReadLine() ?? "";
                        Console.Write("Enter credits: ");
                        int credits = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter capacity: ");
                        int capacity = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter professor ID (optional, press Enter to skip): ");
                        string profId = Console.ReadLine() ?? "";

                        university.CreateCourse(code, name, credits, capacity, profId);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=== ASSIGN PROFESSOR TO COURSE ===\n");
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

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("=== DELETE COURSE ===\n");
                        Console.Write("Enter course code to delete: ");
                        string deleteCode = Console.ReadLine() ?? "";
                        university.DeleteCourse(deleteCode);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "4":
                        backToMain = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void EnrollmentOperations(University university)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
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
                        Console.Clear();
                        Console.WriteLine("=== ENROLL STUDENT IN COURSE ===\n");
                        Console.Write("Enter student ID: ");
                        string sId = Console.ReadLine() ?? "";
                        Console.Write("Enter course code: ");
                        string cCode = Console.ReadLine() ?? "";

                        university.EnrollStudentInCourse(sId, cCode);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=== UNENROLL STUDENT FROM COURSE ===\n");
                        Console.Write("Enter student ID: ");
                        string unenrollSId = Console.ReadLine() ?? "";
                        Console.Write("Enter course code: ");
                        string unenrollCCode = Console.ReadLine() ?? "";

                        university.UnenrollStudentFromCourse(unenrollSId, unenrollCCode);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("=== SET GRADE FOR STUDENT ===\n");
                        Console.Write("Enter student ID: ");
                        string gradeSId = Console.ReadLine() ?? "";
                        Console.Write("Enter course code: ");
                        string gradeCCode = Console.ReadLine() ?? "";
                        Console.Write("Enter grade (6-10): ");
                        double grade = double.Parse(Console.ReadLine() ?? "0");

                        university.SetGrade(gradeSId, gradeCCode, grade);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("=== CALCULATE ALL GPAs ===\n");
                        university.CalculateAllGPAs();

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "5":
                        backToMain = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayOperations(University university)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
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
                        Console.Clear();
                        university.DisplayAllStudents();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        university.DisplayAllProfessors();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        university.DisplayAllCourses();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("=== SEARCH STUDENTS ===\n");
                        Console.Write("Enter search term: ");
                        string term = Console.ReadLine() ?? "";
                        var results = university.SearchStudents(term);
                                                Console.WriteLine($"\nFound {results.Count} student(s):");
                        foreach (var s in results)
                            Console.WriteLine($"- {s.Name} ({s.ID})");

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("=== GENERATE TRANSCRIPT ===\n");
                        Console.Write("Enter student ID: ");
                        string transId = Console.ReadLine() ?? "";
                        Console.WriteLine("\n" + university.GenerateTranscript(transId));
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "6":
                        backToMain = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}