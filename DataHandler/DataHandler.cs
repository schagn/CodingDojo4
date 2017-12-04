using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class DataHandler
    {

        private const string extension = ".txt";
        private const string target = "C:/Benutzer";
        

        public String[] Load (string name)
        {
            String[] messages = File.ReadAllLines(target + name);
            return messages;
        }

        public void Save(List<string> data)
        {
            File.WriteAllLines(target + DateTime.Now.ToShortDateString() + " *-* " + DateTime.Now.ToFileTimeUtc() + extension, data.ToArray());
        }

        public void Delete(string name)
        {
            if (File.Exists(target + name))
            {

                File.Delete(target + name);
            } else
            {
                Console.WriteLine(" No file to delete!");
            }
        }

        public bool FileExisting(string name)
        {
            return File.Exists(target + name + extension);
        }

        public String[] QueryFilesFromFolder()
        {
            if(!Directory.Exists(target)) { Directory.CreateDirectory(target); }
            DirectoryInfo info = new DirectoryInfo(target);
            var result = info.GetFiles("*" + extension);
            string[] tmp = new string[result.Length];
            int i = 0;
            foreach (var item in result)
            {
                tmp[i++] = item.Name;
            }
            return tmp;
        }
    }
}
