using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Model
{
    class InfoFile
    {
        internal string FileName;
        internal int FileLength;

        public InfoFile(string name, int length)
        {
            FileName = name;
            FileLength = length;
        }
    }
}
