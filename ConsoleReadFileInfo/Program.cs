using ConsoleReadFileInfo.Controllers;
using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleReadFileInfo
{
    class Program
    {
        private static string curentFolderPath;
        private static Queue<string> pathes = new Queue<string>();
        private static Queue<InfoFile> infoFiles = new Queue<InfoFile>();

        static ReadInfo readInfo = new ReadInfo();
        static WriteInfo writeInfo = new WriteInfo();

        static Mutex mutexObj = new Mutex();

        public static InfoFile CurrentInfoFile
        {
            get
            {
                if (infoFiles.Count >= 1)
                {
                    return infoFiles.Peek();
                }
                else
                {
                    return null;
                }
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            object mutex = new object();

            Console.OutputEncoding = Encoding.UTF8;
            System.Windows.Forms.MessageBox.Show("Укажите путь к папке!");

            // Путь к папке
            curentFolderPath = readInfo.GetCurentFolder();
            if (!CheckPath(curentFolderPath))
                return;

            pathes.Enqueue(curentFolderPath);

            new Thread(() => GetPathes(ref pathes)).Start();
            Console.WriteLine("Идёт обработка данных... шас всё будет...");

            while (!infoFiles.Any() && pathes.Any())
            {
                new Thread(() => GetFileInfo(ref pathes)).Start();

                while (infoFiles.Any())
                {
                    if (CurrentInfoFile != null)
                        WriteFileInfo(infoFiles.Dequeue());
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Потокозащищенный вызов readInfo.GetPathes
        /// </summary>
        /// <param name="pathes">Очередь папок, в которую добавляются подпапки</param>
        /// <param name="mutex"></param>
        public static void GetPathes(ref Queue<string> pathes)
        {
            mutexObj.WaitOne();
            readInfo.GetPathes(curentFolderPath, ref pathes);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Потокозащищенный вызов readInfo.GetFileInfo
        /// </summary>
        /// <param name="pathes">Очередь папок из которой извлекаются подпапки</param>
        public static void GetFileInfo(ref Queue<string> pathes)
        {
            mutexObj.WaitOne();
            while (pathes.Any())
                readInfo.GetFileInfo(ref pathes, ref infoFiles);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Потокозащищенная запись объектов в xml
        /// </summary>
        /// <param name="infoFile">Объект для записи</param>
        private static void WriteFileInfo(InfoFile infoFile)
        {
            mutexObj.WaitOne();
            new Thread(() => writeInfo.WriteFileInfo(curentFolderPath, infoFile)).Start();
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Потокозащищенная запись объектов в xml
        /// </summary>
        /// <param name="infoFiles">Очередь из которой извлекаются объекты типа InfoFile</param>
        private static void WriteFilesInfo(ref Queue<InfoFile> infoFiles)
        {
            mutexObj.WaitOne();
            writeInfo.WriteFilesInfo(curentFolderPath, ref infoFiles);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Проверяет содержит ли path путь к папке
        /// </summary>
        /// <param name="path">Путь который необходимо проверить</param>
        /// <returns>bool</returns>
        private static bool CheckPath(string path)
        {
            if (path == string.Empty)
            {
                Console.WriteLine("Папка не выбрана. Нажмите любую клавишу для закрытия.");
                Console.ReadKey();
                return false;
            }
            else return true;
        }
    }
}
