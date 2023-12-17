using System.IO;
using TechHaven.Services;

public class FileOperations : IFileOperations
{
    public string[] ReadAllLines(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Fajl nije pronađen: {path}");
        }

        return File.ReadAllLines(path);
    }

    public void WriteAllLines(string path, string[] contents)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllLines(path, contents);
    }
}
