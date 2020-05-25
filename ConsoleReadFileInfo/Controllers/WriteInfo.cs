using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleReadFileInfo.Controllers
{
    class WriteInfo : IWriteInfo
    {
        readonly static object syncLock = new object();
        private Thread t3;

        public void WriteFilesInfo(string path, ref Queue<InfoFile> infoFiles)
        {
            string fullpath = $"{path}\\infoFiles.xml";

            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(List<InfoFile>));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(fullpath, FileMode.OpenOrCreate))
            {
                int count = 0;
                lock (syncLock)
                {
                    List<InfoFile> list = infoFiles.ToList();
                    formatter.Serialize(fs, list);
                    list.ForEach(x => x.GetInfoAboutFile());
                    infoFiles.Clear();
                    count = list.Count();
                }

                Console.WriteLine($"Сериализовано {count} объектов");
            }
        }

        public void WriteFileInfo(string path, InfoFile infoFile)
        {
            InfoFile file = null;
            lock (syncLock)
            {
                // объект для сериализации
                file = infoFile;
                string fullpath = Path.Combine(path, "infoFiles.xml");//$"{path}\\infoFiles.xml";

                t3 = new Thread(() =>
                {
                    var currThread = Thread.CurrentThread;
                    currThread.Name = "WriteFileInfo";

                    // передаем в конструктор тип класса
                    XmlSerializer formatter = new XmlSerializer(typeof(InfoFile));

                    // получаем поток, куда будем записывать сериализованный объект
                    using (FileStream fs = new FileStream(fullpath, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, file);

                        Console.WriteLine("Объект сериализован");
                    }

                    Console.WriteLine($"{currThread.ManagedThreadId} {currThread.Name}:");
                    file.GetInfoAboutFile();
                });
                t3.Start();

            }
        }
    }
}
