using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Controllers
{
    interface IReadInfo
    {
        string Path { get; set; }
        void ReadInfoAboutFiles();
    }
}
