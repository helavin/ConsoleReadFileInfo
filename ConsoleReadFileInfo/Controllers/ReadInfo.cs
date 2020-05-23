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
    internal class ReadInfo : IReadInfo
    {
        readonly static object syncLock = new object();
        internal Thread t1;
        internal Thread t2;

        /// <summary>
        /// Извлекает папки из очереди, получает коллекцию файлов и создает экземпляры класса InfoAboutFile
        /// </summary>
        /// <param name="pathes">Очередь папок, из которой извлекаются подпапки</param>
        public void CreateFileInfo(ref Queue<string> pathes)
        {
            string path_ = string.Empty;
            
            lock (syncLock)
            {
                if (t1 != null && !t1.IsAlive)
                    path_ = pathes.Dequeue();
                t2 = new Thread(() =>
                {
                    var currThread = Thread.CurrentThread;
                    currThread.Name = "CreateFileInfo";

                    if (path_ != string.Empty)
                    {
                        try
                        {
                            var files = Directory.GetFiles(path_);

                            // TODO: запилить в метод
                            foreach (var file in files)
                            {
                                FileInfo fi = new FileInfo(file);
                                if (fi.Exists)
                                {
                                    InfoAboutFile info = new InfoAboutFile(fi.Directory.FullName, fi.Name, fi.Length);

                                    Console.WriteLine($"\t{currThread.ManagedThreadId} {currThread.Name}: " +
                                        $"{info.FileDirectory} - Файл: {info.FileName}, {info.FileLength} байт.");

                                    //TODO: создание очереди из объектов класса InfoAboutFile
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"\t*****Нет доступа к файлу  по пути {path_}*****");
                        }
                    }
                }, 5);
                t2.Start();
            }
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
                                Console.WriteLine($"{currThread.ManagedThreadId} {currThread.Name}: {f}");

                                pathes_.Enqueue(f);
                                GetPathes(f, ref pathes_);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"\t* * * * * Нет доступа к папке по пути {path} * * * * *");
                    }
                }, 5);
                t1.Start();

                // потестить
                if (t2 != null && t2.IsAlive)
                    t2.Join();
            }
        }

        /// <summary>
        /// Открывает форму для выбора директории
        /// </summary>
        /// <returns></returns>
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
