using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ConsoleReadFileInfo.Controllers
{
    class ReadInfo : IReadInfo
    {
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (value != string.Empty)
                path = value;
            }
        }


        public void ReadInfoAboutFiles()
        {
            Path = GetCurentFolder();
            if (Path == null)
            {
                Console.WriteLine("Папка не выбрана!");
                return;
            }
                
            Console.WriteLine(Path);

            Queue<string> fileNames = new Queue<string>();
            Queue<string> directoriesNames = new Queue<string>();

            foreach (var f in Directory.GetFiles(Path))
            {
                fileNames.Enqueue(f);
            }

            foreach (var f in Directory.GetDirectories(Path))
            {
                directoriesNames.Enqueue(f);
            }

            Console.WriteLine("Directories:");
            foreach (var fn in directoriesNames)
                Console.WriteLine(fn);

            Console.WriteLine("\nFiles:");
            foreach (var fn in fileNames)
                Console.WriteLine(fn);

        }

        internal void Display(ref Queue<string> pathes)
        {
            Parallel.ForEach(pathes, (p) =>
            {
                Console.WriteLine(p);
            });
        }

        internal void GetPathes(string path, ref Queue<string> pathes)
        {
            try
            {
                var folders = Directory.GetDirectories(path);
                foreach (var f in folders)
                {
                    //Console.WriteLine($"{f} - {Directory.GetAccessControl(f).AreAccessRulesCanonical}");
                    pathes.Enqueue(f);
                    GetPathes(f, ref pathes);
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Нет доступа к папке по пути {path}");                
            }            
        }

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
