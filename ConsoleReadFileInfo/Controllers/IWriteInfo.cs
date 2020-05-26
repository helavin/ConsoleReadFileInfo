using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;

namespace ConsoleReadFileInfo.Controllers
{
    interface IWriteInfo
    {
        void WriteFilesInfo(string path, ref Queue<InfoFile> infoFiles);
        void WriteFileInfo(string path, InfoFile infoFile);
    }
}
