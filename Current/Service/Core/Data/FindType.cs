using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Data
{
    internal class FindType
    {
        private static string appRoot = "ConsoleService";



        internal List<string> GetTypeAndFolders(string arg)
        {
            var list = ListFoldersInNamespace(arg);
            list.AddRange(ListClassesInNamespace(arg));


            list.RemoveAll(item => item.StartsWith("ConsoleService..vs"));
            list.RemoveAll(item => item.StartsWith("ConsoleService.bin"));
            list.RemoveAll(item => item.StartsWith("ConsoleService.obj"));

            list.Sort();

            return list;
        }

        //internal List<string> GetTypes(string arg)
        //{
        //   return ListFoldersInNamespace(arg);
        //}

        //internal string GetType(string arg)
        //{

        //}

        static List<string> ListFoldersInNamespace(string targetNamespace)
        {
            string projectRootDirectory = FindProjectRootDirectory();

            if (projectRootDirectory == null)
            {
                return null;
            }

            List<string> foldersList = new List<string>();

            // Proje klasörünü tarayarak klasörleri bul
            var allFolders = Directory.GetDirectories(projectRootDirectory, "*", SearchOption.AllDirectories);

            foreach (var folderPath in allFolders)
            {
                // Klasörü işle
                string relativePath = Path.GetRelativePath(projectRootDirectory, folderPath);
                string folderNamespace = Path.Combine(targetNamespace, relativePath).Replace(Path.DirectorySeparatorChar, '.');
                foldersList.Add(folderNamespace);
            }

            return foldersList;
        }


        static List<string> ListClassesInNamespace(string targetNamespace)
        {
            string projectRootDirectory = FindProjectRootDirectory();

            if (projectRootDirectory == null)
            {
                return null;
            }

            List<string> classList = new List<string>();

            // Proje klasörünü tarayarak C# dosyalarını bul
            var csharpFiles = Directory.GetFiles(projectRootDirectory, "*.cs", SearchOption.AllDirectories);

            foreach (var filePath in csharpFiles)
            {
                // Dosyayı oku
                string fileContent = File.ReadAllText(filePath);

                // Dosyanın içeriğini analiz et ve sınıf adlarını tespit et
                var classesInFile = GetClassesInFile(fileContent, targetNamespace, projectRootDirectory, filePath);

                // Liste içerisine sınıf adlarını ekle
                classList.AddRange(classesInFile);
            }

            return classList;
        }

        static string FindProjectRootDirectory()
        {
            // Çalışma dizinini bul
            string currentDirectory = Environment.CurrentDirectory;

            // Proje adını içeren klasörünü bul
            string projectDirectory = currentDirectory;

            while (!File.Exists(Path.Combine(projectDirectory, $"{appRoot}.csproj")))
            {
                projectDirectory = Directory.GetParent(projectDirectory)?.FullName;

                if (projectDirectory == null)
                {
                    return null;
                }
            }

            return projectDirectory;
        }

        static IEnumerable<string> GetClassesInFile(string fileContent, string targetNamespace, string projectRootDirectory, string filePath)
        {
            List<string> classNames = new List<string>();

            // Dosyanın içeriğini satırlara böler
            string[] lines = fileContent.Split('\n');

            bool isInTargetNamespace = false;
            foreach (var line in lines)
            {
                if (line.Contains("namespace " + targetNamespace))
                {
                    isInTargetNamespace = true;
                }

                if (isInTargetNamespace)
                {
                    Match match = Regex.Match(line, @"\bclass\s+(\w+)\s*");

                    if (match.Success)
                    {
                        string className = match.Groups[1].Value;
                        string relativePath = Path.GetRelativePath(projectRootDirectory, filePath.Replace(".cs", null));
                        string classWithPath = Path.Combine(targetNamespace, relativePath).Replace(Path.DirectorySeparatorChar, '.');
                        classNames.Add(classWithPath);
                    }
                }
            }

            return classNames;
        }
    }
}
