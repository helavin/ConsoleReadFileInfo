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
        private static Queue<string> pathes = new Queue<string>();
        private static Queue<InfoFile> infoFiles = new Queue<InfoFile>();

        [STAThread]
        static void Main(string[] args)
        {
            var currThread = Thread.CurrentThread;
            currThread.Name = "Main";

            Console.OutputEncoding = Encoding.UTF8;
            System.Windows.Forms.MessageBox.Show("Укажите путь к папке!");
            ReadInfo readInfo = new ReadInfo();

            

            // Путь к папке
            string curentFolderPath = readInfo.GetCurentFolder();
            if (!CheckPath(curentFolderPath))
                return;

            pathes.Enqueue(curentFolderPath);

            readInfo.GetPathes(curentFolderPath, ref pathes);


            while (pathes.Any())
            {
                //Trace.WriteLine($"\t{currThread.ManagedThreadId}: {currThread.Name}");
                readInfo.GetFileInfo(ref pathes, ref infoFiles);
            }

            while (infoFiles.Any())
            {
                WriteInfo writeInfo = new WriteInfo();
                writeInfo.WriteFileInfo(ref infoFiles);
            }


            //Console.ReadKey();
            //if (infoFiles.Count != 0)
            //    foreach (var info in infoFiles)
            //        Console.WriteLine(info.Name);
            Console.ReadKey();
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
