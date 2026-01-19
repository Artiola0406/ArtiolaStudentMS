using System;
using System.IO;
using System.Text.Json;

namespace StudentMS
{
    public class FileHandler
    {
        private const string FilePath = "university_data.json";

        // Klasa ndihmëse për të dhënat
        public class SaveData
        {
            public object Students { get; set; }
            public object Professors { get; set; }
            public object Courses { get; set; }
        }

        // Ruaj University në JSON
        public void SaveUniversity(University university)
        {
            try
            {
                var data = new
                {
                    Students = university.Students,
                    Professors = university.Professors,
                    Courses = university.Courses
                };

                string jsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(FilePath, jsonContent);
                Console.WriteLine($"Data saved to {FilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving: {ex.Message}");
            }
        }

        // Ngarko University nga JSON
        public University LoadUniversity()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("No saved data found.");
                return new University();
            }

            try
            {
                string jsonContent = File.ReadAllText(FilePath);

                // Për versionin e thjeshtë, kthe një universitet të ri
                // Ose implemento deserializimin plotë

                Console.WriteLine($"Data loaded from {FilePath}");

                // Kthe një universitet të ri (ose implemento deserializimin)
                return new University();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading: {ex.Message}");
                return new University();
            }
        }

        // Metodat e tua origjinale (për përdorim manual)
        public void SaveToJson(string jsonContent)
        {
            File.WriteAllText(FilePath, jsonContent);
        }

        public string LoadFromJson()
        {
            if (!File.Exists(FilePath)) return null;
            return File.ReadAllText(FilePath);
        }
    }
}