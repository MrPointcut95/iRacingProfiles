using System;
using System.IO;
using System.Xml.Serialization;
using iRacingProfiles.Exceptions;

namespace iRacingProfiles.Logic {
    class FileUtils {
        public static String GetFileContent(String location) {
            CheckFile(location);
            return File.ReadAllText(location);
        }

        private static void CheckFile(string route) {
            if(!File.Exists(route)) {
                throw new iRacingProfileException("exception_file_exists");
            }
        }

        public static void CheckDirectory(string path) {
            if (!Directory.Exists(path)) {
                throw new iRacingProfileException("exception_folder_exists");
            }
        }

        public static void CreateDirectory(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        public static void DeleteDirectory(string path) {
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
            }
        }
        
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new() {
            TextWriter writer = null;
            try {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            } finally {
                if (writer != null)
                    writer.Close();
            }
        }

        public static void CopyFile(string source, string dest) {
            CheckFile(source);
            File.Copy(source, dest, true);
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : class {
            TextReader reader = null;
            try {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            } catch (IOException e) {
                return null;
            } finally {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
