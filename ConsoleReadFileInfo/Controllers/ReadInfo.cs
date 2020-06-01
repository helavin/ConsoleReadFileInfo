using System;
using System.Collections.Generic;
using System.IO;
using ConsoleReadFileInfo.Model;
using System.Threading;
using System.Diagnostics;

namespace ConsoleReadFileInfo.Controllers
{
    public class ReadInfo : IReadInfo
    {
        readonly static object syncLock = new object();
        private Thread t1;
        internal Thread t2;

        /// <summary>
        /// Извлекает папки из очереди, получает коллекцию файлов и создает объекты типа InfoFile
        /// </summary>
        /// <param name="pathes">Очередь папок, из которой извлекаются подпапки</param>
        /// <param name="infoFiles">Очередь в которую добавляются объекты типа InfoFile</param>
        public void GetFileInfo(Queue<string> pathes, Queue<InfoFile> infoFiles)
        {
            string path = string.Empty;
            //var infoFiles_ = infoFiles;
            
            lock (syncLock)
            {
                if (t1 != null && !t1.IsAlive)
                    path = pathes.Dequeue();
                t2 = new Thread(() =>
                {
                    if (path != string.Empty)
                    {
                        try
                        {
                            var files = Directory.GetFiles(path);

                            foreach (var file in files)
                            {
                                var info = CreateInfoFile(file);
                                if (info == null)
                                    continue;

                                infoFiles.Enqueue(info);
                            }
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine($"\t* * * * * Нет доступа к файлу  по пути {path} * * * * *");
                        }
                    }
                }, 0);
                t2.Start();
            }
        }

        /// <summary>
        /// Создает объект типа InfoFile
        /// </summary>
        /// <param name="file">Путь к файлу</param>
        /// <returns>Возвращает объект типа InfoFile</returns>
        public InfoFile CreateInfoFile(string file)
        {
            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
                return null;

            InfoFile info = new InfoFile(fi.Directory.FullName, fi.Name, fi.Length);
            return info;
        }

        /// <summary>
        /// Извлекает все подпапки переданной папки
        /// </summary>
        /// <param name="path">Папка в которой нужно найти подпапки</param>
        /// <param name="pathes">Очередь папок, в которую добавляются подпапки</param>
        public void GetPathes(string path, Queue<string> pathes)
        {
            //var pathes_ = pathes;
            lock (syncLock)
            {
                t1 = new Thread(() =>
                {
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            var folders = Directory.GetDirectories(path);
                            foreach (var f in folders)
                            {
                                pathes.Enqueue(f);
                                GetPathes(f, pathes);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine($"\t* * * * * Нет доступа к папке по пути {path} * * * * *");
                    }
                }, 0);
                t1.Start();
            }
        }

        

    }
}
