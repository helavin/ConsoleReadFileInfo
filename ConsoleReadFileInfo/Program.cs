using ConsoleReadFileInfo.Controllers;
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

        [STAThread]
        static void Main(string[] args)
        {
            var currThread = Thread.CurrentThread;
            currThread.Name = "Main";

            Console.OutputEncoding = Encoding.UTF8;
            System.Windows.Forms.MessageBox.Show("Укажите путь к папке!");
            ReadInfo readInfo = new ReadInfo();

            // Путь к папке
            string firstFolderPath = readInfo.GetCurentFolder();
            if (!CheckPath(firstFolderPath))
                return;

            pathes.Enqueue(firstFolderPath);

            readInfo.GetPathes(firstFolderPath, ref pathes);

            while (pathes.Any())
            {
                Trace.WriteLine($"\t{currThread.ManagedThreadId}: {currThread.Name}");
                readInfo.CreateFileInfo(ref pathes);
            }

            Console.ReadKey();
        }

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
