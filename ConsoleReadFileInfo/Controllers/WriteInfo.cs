using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConsoleReadFileInfo.Controllers
{
    class WriteInfo : IWriteInfo
    {
        readonly static object syncLock = new object();
        static Mutex mutexWriteObj = new Mutex();

        public void WriteFilesInfo(string path, ref Queue<InfoFile> infoFiles)
        {
            var currThread = Thread.CurrentThread;
            currThread.Name = "WriteFileInfo";

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
                    //list.ForEach(x => Console.WriteLine($"{currThread.ManagedThreadId} {currThread.Name}: {x.Dir} {x.Name}"));
                    list.ForEach(x => x.GetInfoAboutFile());
                    infoFiles.Clear();
                    count = list.Count();
                }

                Console.WriteLine($"Сериализовано {count} объектов");
            }
        }

        public void WriteFileInfo(string path, InfoFile infoFile)
        {
            if (infoFile == null)
                return;

            var currThread = Thread.CurrentThread;
            currThread.Name = "WriteFileInfo";

            string fullpath = Path.Combine(path, "infoFiles.xml");
            mutexWriteObj.WaitOne();
            XDocument xdoc;
            XElement root = new XElement("InfoFiles");

            // создаем элемент
            XElement infoFileElem = new XElement("InfoFile");
            //XElement idElem = new XElement("id", infoFile.id);
            XElement nameElem = new XElement("Name", infoFile.Name);
            XElement lengthElem = new XElement("Length", infoFile.Length.ToString());

            //infoFileElem.Add(idElem);
            infoFileElem.Add(nameElem);
            infoFileElem.Add(lengthElem);

            FileInfo fileInf = new FileInfo(fullpath);
            if (!fileInf.Exists)
            {
                xdoc = new XDocument();
                root.Add(infoFileElem);
                xdoc.Add(root);
            }
            else
            {
                xdoc = XDocument.Load(fullpath);
                xdoc.Root.Add(infoFileElem);
            }
            //Console.WriteLine($"{currThread.ManagedThreadId} {currThread.Name}: {infoFile.Dir} {infoFile.Name}");
            infoFile.GetInfoAboutFile();
            xdoc.Save(fullpath);
            mutexWriteObj.ReleaseMutex();
        }
    }
}
