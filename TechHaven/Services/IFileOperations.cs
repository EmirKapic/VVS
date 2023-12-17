namespace TechHaven.Services
{
    public interface IFileOperations
    {
        string[] ReadAllLines(string path);
        void WriteAllLines(string path, string[] contents);
    }
}
