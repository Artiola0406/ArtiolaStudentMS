using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentMS
{
    public class Course
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public int Capacity { get; set; }
        public string ProfessorID { get; set; } // ID e profesorit që jep këtë kurs
        public List<Enrollment> Enrollments { get; set; }

        public Course(string code, string name, int credits, int capacity, string professorId = "")
        {
            CourseCode = code;
            CourseName = name;
            Credits = credits;
            Capacity = capacity;
            ProfessorID = professorId;
            Enrollments = new List<Enrollment>();
        }

        public bool HasAvailableSeats()
        {
            return Enrollments.Count < Capacity;
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            if (!HasAvailableSeats())
                throw new InvalidOperationException("Course is full");

            Enrollments.Add(enrollment);
        }

        public bool RemoveEnrollment(string studentId)
        {
            var enrollment = Enrollments.FirstOrDefault(e => e.StudentID == studentId);
            if (enrollment != null)
            {
                Enrollments.Remove(enrollment);
                return true;
            }
            return false;
        }

        public int AvailableSeats()
        {
            return Capacity - Enrollments.Count;
        }

        public override string ToString()
        {
            return $"{CourseCode}: {CourseName} ({Credits} credits) - Prof: {ProfessorID} - Seats: {AvailableSeats()}/{Capacity}";
        }
    }
}