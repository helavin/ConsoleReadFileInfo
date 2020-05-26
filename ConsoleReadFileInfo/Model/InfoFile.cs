using System;

namespace ConsoleReadFileInfo.Model
{
    
    [Serializable]
    public class InfoFile
    {
        public static int i = 0;
        internal int id = 0;

        internal string Dir;
        public string Name;
        public long Length;

        public InfoFile()
        { }
        public InfoFile(string directory, string name, long length)
        {
            id = ++i;
            Dir = directory;
            Name = name;
            Length = length;
        }

        public void GetInfoAboutFile()
        {
            Console.WriteLine($" {Dir} \t Файл: {Name}, {Length} байт.");
        }
    }
}
