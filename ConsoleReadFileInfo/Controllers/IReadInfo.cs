using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;

namespace ConsoleReadFileInfo.Controllers
{
    public interface IReadInfo
    {
        void GetPathes(string path, Queue<string> pathes);
        void GetFileInfo(Queue<string> pathes, Queue<InfoFile> infoFiles);
    }
}
