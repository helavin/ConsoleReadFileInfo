using ConsoleReadFileInfo.Controllers;
using ConsoleReadFileInfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace ConsoleReadFileInfo
{
    public class Program
    {
        public static string CurentFolderPath { get; set; }

        public static Queue<string> Pathes { get; private set; } = new Queue<string>();
        public static Queue<InfoFile> InfoFiles { get; private set; } = new Queue<InfoFile>();

        static readonly IReadInfo readInfo = new ReadInfo();
        static readonly IWriteInfo writeInfo = new WriteInfo();

        static readonly Mutex mutexObj = new Mutex();


        [STAThread]
        static void Main(string[] args)
        {
            object mutex = new object();

            Console.OutputEncoding = Encoding.UTF8;
            System.Windows.Forms.MessageBox.Show("Укажите путь к папке!");

            // Путь к папке
            CurentFolderPath = GetCurentFolder();
            if (!CheckPath(CurentFolderPath))
                return;

            Console.WriteLine("Идёт обработка данных... шас всё будет...");
            Pathes.Enqueue(CurentFolderPath);

            new Thread(() => GetPathes(Pathes)).Start();

            while (!InfoFiles.Any() && Pathes.Any())
            {
                new Thread(() => GetFileInfo(Pathes)).Start();

                while (InfoFiles.Any())
                {
                    if (InfoFiles.Any())
                        WriteFileInfo(InfoFiles.Dequeue());
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Потокозащищенный вызов readInfo.GetPathes
        /// </summary>
        /// <param name="pathes">Очередь папок, в которую добавляются подпапки</param>
        /// <param name="mutex"></param>
        private static void GetPathes(Queue<string> pathes)
        {
            mutexObj.WaitOne();
            readInfo.GetPathes(CurentFolderPath, pathes);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Потокозащищенный вызов readInfo.GetFileInfo
        /// </summary>
        /// <param name="pathes">Очередь папок из которой извлекаются подпапки</param>
        private static void GetFileInfo(Queue<string> pathes)
        {
            mutexObj.WaitOne();
            while (pathes.Any())
                readInfo.GetFileInfo(pathes, InfoFiles);
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Потокозащищенная запись объектов в xml
        /// </summary>
        /// <param name="infoFile">Объект для записи</param>
        private static void WriteFileInfo(InfoFile infoFile)
        {
            mutexObj.WaitOne();
            new Thread(() => writeInfo.WriteFileInfo(CurentFolderPath, infoFile)).Start();
            mutexObj.ReleaseMutex();
        }

        /// <summary>
        /// Проверяет содержит ли path путь к папке
        /// </summary>
        /// <param name="path">Путь который необходимо проверить</param>
        /// <returns>bool</returns>
        public static bool CheckPath(string path)
        {
            if (path == string.Empty)
            {
                Console.WriteLine("Папка не выбрана. Нажмите любую клавишу для закрытия.");
                Console.ReadKey();
                return false;
            }
            else return true;
        }

        /// <summary>
        /// Открывает окно выбора папки
        /// </summary>
        /// <returns>Возвращает путь к выбранной папке</returns>
        private static string GetCurentFolder()
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
