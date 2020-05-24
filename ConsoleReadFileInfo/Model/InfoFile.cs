using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Model
{
    class InfoFile
    {
        internal string Dir;
        internal string Name;
        internal long Length;

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
