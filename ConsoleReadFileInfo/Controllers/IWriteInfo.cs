using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Controllers
{
    interface IWriteInfo
    {
        void WriteFilesInfo(string path, ref Queue<InfoFile> infoFiles);
    }
}
