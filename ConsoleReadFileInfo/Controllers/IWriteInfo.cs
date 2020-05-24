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
        void WriteFileInfo(ref Queue<InfoFile> infoFiles);
    }
}
