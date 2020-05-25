using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Model
{
    [Serializable]
    public class InfoFile
    {
        internal string Dir;
        public string Name;
        public long Length;

        public InfoFile()
        { }
        public InfoFile(string directory, string name, long length)
        {
            Dir = directory;
            Name = name;
            Length = length;
        }

        public void GetInfoAboutFile()
        {
            Console.WriteLine($"{Dir} \t Файл: {Name}, {Length} байт.");
        }
    }
}
