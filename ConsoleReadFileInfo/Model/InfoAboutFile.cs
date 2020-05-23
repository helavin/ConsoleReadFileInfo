using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Model
{
    class InfoAboutFile
    {
        internal string FileDirectory;
        internal string FileName;
        internal long FileLength;

        public InfoAboutFile(string directory, string name, long length)
        {
            FileDirectory = directory;
            FileName = name;
            FileLength = length;
        }
    }
}
