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
        static object locker = new object();
        static ReadInfo readInfo = new ReadInfo();
        static WriteInfo writeInfo = new WriteInfo();

        [STAThread]
        static void Main(string[] args)
        {
            var currThread = Thread.CurrentThread;
            currThread.Name = "Main";

            Console.OutputEncoding = Encoding.UTF8;
            System.Windows.Forms.MessageBox.Show("Укажите путь к папке!");
            
            // Путь к папке
            curentFolderPath = readInfo.GetCurentFolder();
            if (!CheckPath(curentFolderPath))
                return;

            pathes.Enqueue(curentFolderPath);

            while (!infoFiles.Any())
            {
                Thread myThread1 = new Thread(GetPathes);
                myThread1.Start();

                while (pathes.Any())
                {
                    GetFileInfo();
                }

                lock (locker)
                {
                    if (infoFiles.Any())
                    {
                        Thread myThread3 = new Thread(WriteFileInfo);
                        myThread3.Start();
                    }
                }
            }
            Console.ReadKey();
        }



        public static void GetPathes()
        {
            lock (locker)
            {
                readInfo.GetPathes(curentFolderPath, ref pathes);
            }
        }

        public static void GetFileInfo()
        {
            lock (locker)
            {
                readInfo.GetFileInfo(ref pathes, ref infoFiles);
            }
        }

        private static void WriteFileInfo()
        {
            lock (locker)
            {
                if (infoFiles.Any())
                    writeInfo.WriteFilesInfo(curentFolderPath, ref infoFiles);
            }
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
