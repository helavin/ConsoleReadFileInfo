using ConsoleReadFileInfo.Controllers;
using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


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

        [STAThread]
        static void Main(string[] args)
        {
            object mutex = new object();
            var currThread = Thread.CurrentThread;
            currThread.Name = "Main";

            Console.OutputEncoding = Encoding.UTF8;
            System.Windows.Forms.MessageBox.Show("Укажите путь к папке!");

            // Путь к папке
            curentFolderPath = readInfo.GetCurentFolder();
            if (!CheckPath(curentFolderPath))
                return;

            pathes.Enqueue(curentFolderPath);

            new Thread(() => GetPathes(ref pathes/*, mutex*/)).Start();

            while (!infoFiles.Any())
            {
                new Thread(() => GetFileInfo(ref pathes/*, mutex*/)).Start();
                
                new Thread(() => WriteFilesInfo(ref infoFiles/*, mutex*/)).Start();
            }
            Console.ReadKey();
        }


        /// <summary>
        /// Потокозащищенный вызов readInfo.GetPathes
        /// </summary>
        /// <param name="pathes">Очередь папок, в которую добавляются подпапки</param>
        /// <param name="mutex"></param>
        public static void GetPathes(ref Queue<string> pathes/*, object mutex*/)
        {
            //lock (mutex)
            //{
            //    readInfo.GetPathes(curentFolderPath, ref pathes);
            //} // или
            mutexObj.WaitOne();
            readInfo.GetPathes(curentFolderPath, ref pathes);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Потокозащищенный вызов readInfo.GetFileInfo
        /// </summary>
        /// <param name="pathes">Очередь папок из которой извлекаются подпапки</param>
        public static void GetFileInfo(ref Queue<string> pathes/*, object mutex*/)
        {
            //lock (mutex)
            //{
            //    while (pathes.Any())
            //        readInfo.GetFileInfo(ref pathes, ref infoFiles);
            //} // или
            mutexObj.WaitOne();
            while (pathes.Any())
                readInfo.GetFileInfo(ref pathes, ref infoFiles);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Потокозащищенный вызов writeInfo.WriteFilesInfo
        /// </summary>
        /// <param name="infoFiles">Очередь из которой извлекаются объекты типа InfoFile</param>
        private static void WriteFilesInfo(ref Queue<InfoFile> infoFiles/*, object mutex*/)
        {
            //lock (mutex)
            //{
            //    if (infoFiles.Any())
            //        writeInfo.WriteFilesInfo(curentFolderPath, ref infoFiles);
            //} // или
            mutexObj.WaitOne();
            if (infoFiles.Any())
                writeInfo.WriteFilesInfo(curentFolderPath, ref infoFiles);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Проверяет содержит ли path путь к папке
        /// </summary>
        /// <param name="path">Путь который необходимо проверить</param>
        /// <returns></returns>
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
