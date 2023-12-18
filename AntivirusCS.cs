using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CodeTranslationAssistant
{
    class Program
    {
        static void Main(string[] args)
        {
            Files f = new Files();
            // f.WriteMalwareData();
            f.SetMalwareDatas();
            f.RunScan();
        }
    }

    class FileFormat
    {
        public string FName { get; set; }
        public string MD5 { get; set; }

        public FileFormat(string fName, string md5)
        {
            FName = fName;
            MD5 = md5;
        }
    }

    class Files
    {
        private List<FileFormat> allSystemFiles;
        private List<string> MFinSystem;
        private List<FileFormat> MalwareFiles;

        public Files()
        {
            allSystemFiles = new List<FileFormat>();
            MFinSystem = new List<string>();
            MalwareFiles = new List<FileFormat>();
        }

        public void RunScan(string file = null)
        {
            SetMalwareDatas();
            string currentDir;
            if (file == null)
            {
                currentDir = Path.Combine("C:", "Users", "mgoek", "Desktop", "BIL412_Bitirme_Projesi", "BIL412_Bitirme_Projesi", "TESTET");
            }
            else
            {
                currentDir = file;
            }
            DisplayDirectoryContents(currentDir);

            CleanAll();
        }

        public void DisplayDirectoryContents(string dir)
        {
            try
            {
                string[] files = Directory.GetFiles(dir);
                string[] directories = Directory.GetDirectories(dir);

                foreach (string file in files)
                {
                    Console.Write(file);
                    AddFile(file);
                }

                foreach (string subdirectory in directories)
                {
                    Console.Write(subdirectory);
                    DisplayDirectoryContents(subdirectory);
                }
            }
            catch (Exception e)
            {
                // Handle exception
            }
        }

        public void WriteTxt(string content)
        {
            string fileName = "C:\\Users\\mgoek\\Desktop\\otu\\print.txt";
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine(content);
            }
        }

        public void CleanAll()
        {
            if (MalwareFiles.Count >= 1)
            {
                int count = MalwareFiles.Count;
                string dateFormat = "yyyy/MM/dd HH:mm:ss";
                DateTime date = DateTime.Now;
                string content = "Last Scan Date: " + date.ToString(dateFormat) + "\nFOUND MALWARE(s) COUNT:" + count.ToString();
                WriteTxt(content);
                foreach (FileFormat malwareFile in MalwareFiles)
                {
                    DeleteFile(malwareFile.FName);
                    content = " **REMOVED! " + " Path: " + malwareFile.FName;
                    WriteTxt(content);
                }
            }
            else
            {
                string dateFormat = "yyyy/MM/dd HH:mm:ss";
                DateTime date = DateTime.Now;
                WriteTxt(" SYSTEM SAFE! " + " Last Scan Date: " + date.ToString(dateFormat));
            }
        }

        public void AddFile(string fPat)
        {
            string md5 = GetMD5(fPat);
            bool MFcheck = IsMalwareFile(md5);
            if (MFcheck)
            {
                MalwareFiles.Add(new FileFormat(fPat, md5));
            }
            allSystemFiles.Add(new FileFormat(fPat, md5));
        }

        public bool IsMalwareFile(string curFile)
        {
            return IsElementOfMF(curFile);
        }

        public bool IsElementOfMF(string newFMd5)
        {
            foreach (string temp in MFinSystem)
            {
                if (temp == newFMd5)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetMD5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        public int GetNumberofAllFiles()
        {
            return allSystemFiles.Count;
        }

        public int GetNumberofMFFiles()
        {
            return MFinSystem.Count;
        }

        public void UpdateMF()
        {
            string directoryPath = "C:\\Users\\mgoek\\Desktop";
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        Console.WriteLine(file);
                    }
                }
                else
                {
                    Console.WriteLine("Dizin boş.");
                }
            }
            else
            {
                Console.WriteLine("Dizin bulunamadı veya bir dizin değil.");
            }
            try
            {
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        string md5 = GetMD5(file);
                        MFinSystem.Add(md5);
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exception
            }
        }

        public void WriteMalwareData()
        {
            string fileName = "C:\\Users\\mgoek\\Desktop\\otu\\MALWARES.txt";
            UpdateMF();
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                foreach (string md5 in MFinSystem)
                {
                    file.WriteLine(md5);
                }
            }
        }

        public void SetMalwareDatas()
        {
            string fileName = "C:\\Users\\mgoek\\Desktop\\otu\\MALWARES.txt";
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    MFinSystem.Add(line.Trim());
                }
            }
            catch (Exception e)
            {
                // Handle exception
            }
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}


