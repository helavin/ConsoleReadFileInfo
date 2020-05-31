using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;

namespace ConsoleReadFileInfo.Controllers
{
    public interface IWriteInfo
    {
        void WriteFilesInfo(string path, Queue<InfoFile> infoFiles);
        void WriteFileInfo(string path, InfoFile infoFile);
    }
}
