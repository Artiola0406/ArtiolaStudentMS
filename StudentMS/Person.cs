using System;

namespace StudentMS
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ID { get; set; }

        public Person(string name, string email, string id)
        {
            Name = name;
            Email = email;
            ID = id;
        }

        public static string GenerateEmail(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "unknown@umib.net";

            string[] nameParts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length >= 2)
            {
                string firstName = nameParts[0].ToLower();
                string lastName = nameParts[1].ToLower();
                return $"{firstName}.{lastName}@umib.net";
            }
            else if (nameParts.Length == 1)
            {
                return $"{nameParts[0].ToLower()}@umib.net";
            }

            return "unknown@umib.net";
        }
    }
}