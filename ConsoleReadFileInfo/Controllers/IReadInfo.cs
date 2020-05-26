using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;

namespace ConsoleReadFileInfo.Controllers
{
    interface IReadInfo
    {
        void GetPathes(string path, ref Queue<string> pathes);
        void GetFileInfo(ref Queue<string> pathes, ref Queue<InfoFile> infoFiles);
        
    }
}
