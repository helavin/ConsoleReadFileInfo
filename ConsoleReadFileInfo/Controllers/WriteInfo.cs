using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleReadFileInfo.Controllers
{
    class WriteInfo : IWriteInfo
    {
        readonly static object syncLock = new object();
        private Thread t3;

        public void WriteFileInfo(ref Queue<InfoFile> infoFiles)
        {
            InfoFile file = null;
            lock (syncLock)
            {

                file = infoFiles.Dequeue();

                t3 = new Thread(() =>
                {
                    var currThread = Thread.CurrentThread;
                    currThread.Name = "WriteFileInfo";


                    Console.WriteLine($"\t{currThread.ManagedThreadId} {currThread.Name}:");
                    file.GetInfoAboutFile();
                });
                t3.Start();

            }
        }
    }
}
