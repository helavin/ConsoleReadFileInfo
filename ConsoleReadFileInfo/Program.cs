using ConsoleReadFileInfo.Controllers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ConsoleReadFileInfo
{
    class Program
    {
        private static Queue<string> pathes = new Queue<string>();

        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            System.Windows.Forms.MessageBox.Show("Укажите путь к папке!");
            ReadInfo readInfo = new ReadInfo();
            //readInfo.ReadInfoAboutFiles();

            // Путь к папке
            string currentFolderPath = readInfo.GetCurentFolder();
            pathes.Enqueue(currentFolderPath);

            Task taskGetPathes = new Task(() => //{
            readInfo.GetPathes(currentFolderPath, ref pathes)
        //    }
        );

            // задача продолжения
            Task task2 = taskGetPathes.ContinueWith(task => readInfo.Display(ref pathes));

            taskGetPathes.Start();

            // ждем окончания второй задачи
            task2.Wait();

            Console.WriteLine("Завершение метода Main");

            Console.ReadKey();
        }


    }
}
