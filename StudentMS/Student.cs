using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentMS
{
    public class Student : Person
    {
        public List<Enrollment> Enrollments { get; set; }

        /// <summary>
        /// KLASA STUDENT - trashëgon nga Person
        /// Paraqet një student në universitet.
        /// Studenti është një lloj i veçantë i Person.
        /// </summary>

        public Student(string name, string email, string id) : base(name, email, id)
        {
            Enrollments = new List<Enrollment>();
        }

        public double? CalculateGPA()
        {
            if (Enrollments.Count == 0) return null;

            var validGrades = Enrollments
                .Where(e => e.Grade.HasValue)
                .Select(e => e.Grade.Value)
                .ToList();

            if (validGrades.Count == 0) return null;

            return validGrades.Average();
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            Enrollments.Add(enrollment);
        }

        public bool RemoveEnrollment(string courseCode)
        {
            var enrollment = Enrollments.FirstOrDefault(e => e.CourseCode == courseCode);
            if (enrollment != null)
            {
                Enrollments.Remove(enrollment);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Student: {Name} ({ID}) - Email: {Email} - Courses: {Enrollments.Count}";
        }
    }
}