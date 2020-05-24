using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Controllers
{
    interface IReadInfo
    {
        void GetPathes(string path, ref Queue<string> pathes);
        void GetFileInfo(ref Queue<string> pathes, ref Queue<InfoFile> infoFiles);
        
    }
}
