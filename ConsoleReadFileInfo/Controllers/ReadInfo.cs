using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ConsoleReadFileInfo.Model;
using System.Threading;
using System.Diagnostics;

namespace ConsoleReadFileInfo.Controllers
{
    class ReadInfo : IReadInfo
    {
        readonly static object syncLock = new object();
        private Thread t1;
        internal Thread t2;

        /// <summary>
        /// Извлекает папки из очереди, получает коллекцию файлов и создает объекты типа InfoFile
        /// </summary>
        /// <param name="pathes">Очередь папок, из которой извлекаются подпапки</param>
        public void GetFileInfo(ref Queue<string> pathes, ref Queue<InfoFile> infoFiles)
        {
            string path = string.Empty;
            var infoFiles_ = infoFiles;

            lock (syncLock)
            {
                if (t1 != null && !t1.IsAlive)
                    path = pathes.Dequeue();
                t2 = new Thread(() =>
                {
                    var currThread = Thread.CurrentThread;
                    currThread.Name = "CreateFileInfo";

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

                                //Console.WriteLine($"\t{currThread.ManagedThreadId} {currThread.Name}: {info.Dir} {info.Name}");
                                

                                infoFiles_.Enqueue(info);
                            }
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine($"\t*****Нет доступа к файлу  по пути {path}*****");
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
        private InfoFile CreateInfoFile(string file)
        {
            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
                return null;

            InfoFile info = new InfoFile(fi.Directory.FullName, fi.Name, fi.Length);
            //info.GetInfoAboutFile();
            return info;
        }

        /// <summary>
        /// Извлекает все подпапки переданной папки
        /// </summary>
        /// <param name="path">Папка в которой нужно найти подпапки</param>
        /// <param name="pathes">Очередь папок, в которую добавляются подпапки</param>
        public void GetPathes(string path, ref Queue<string> pathes)
        {
            var pathes_ = pathes;
            lock (syncLock)
            {
                t1 = new Thread(() =>
                {
                    var currThread = Thread.CurrentThread;
                    currThread.Name = "GetPathes";
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            var folders = Directory.GetDirectories(path);
                            foreach (var f in folders)
                            {
                                //Console.WriteLine($"{currThread.ManagedThreadId} {currThread.Name}: {f}");
                                pathes_.Enqueue(f);
                                GetPathes(f, ref pathes_);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine($"\t* * * * * Нет доступа к папке по пути {path} * * * * *");
                    }
                }, 0);
                t1.Start();

                // потестить
                //if (t2 != null && t2.IsAlive)
                //    t2.Join();
            }
        }

        /// <summary>
        /// Открывает окно выбора папки
        /// </summary>
        /// <returns>Возвращает путь к выбранной папке</returns>
        internal string GetCurentFolder()
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return dialog.SelectedPath;
                else return string.Empty;
            }
        }

    }
}
