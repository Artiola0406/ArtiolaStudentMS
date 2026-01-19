using System;
using System.Collections.Generic;

namespace StudentMS
{
    /// <summary>
    /// KLASA PROFESSOR - trashëgon nga Person
    /// Paraqet një profesor në universitet.
    /// Profesori është një lloj i veçantë i Person.
    /// </summary>
    public class Professor : Person
    {
        public string Department { get; set; }
        public string OfficeNumber { get; set; }
        public List<string> CoursesTeaching { get; set; } // Lëndët që jep

        public Professor(string name, string department, string officeNumber, string id)
            : base(name, Person.GenerateEmail(name), id)
        {
            Department = department;
            OfficeNumber = officeNumber;
            CoursesTeaching = new List<string>();
        }

        public void AddCourse(string courseCode)
        {
            if (!CoursesTeaching.Contains(courseCode))
            {
                CoursesTeaching.Add(courseCode);
            }
        }

        public bool RemoveCourse(string courseCode)
        {
            return CoursesTeaching.Remove(courseCode);
        }

        public override string ToString()
        {
            return $"Prof. {Name} - {Department} (Office: {OfficeNumber}) - Courses: {string.Join(", ", CoursesTeaching)}";
        }
    }
}