using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuitarManagement.CommonDefine
{
    public class CommonFunction
    {
        public static List<string> getProductCategory()
        {
            List<string> result = new List<string>()
            {
                "Classical Guitar",
                "Acoustic Guitar",
                "Electric Guitar",
                "Ukulele",
                "Capo",
                "Dây đàn"
            };
            return result;
        }

        public static string getProductImagePath()
        {
            string rs = "";
            string currentPath = Directory.GetCurrentDirectory();
            string[] pathComponent = currentPath.Split('\\');
            int i = 0;
            foreach (string str in pathComponent)
            {
                i++;
                if (str.Equals("GuitarManagement"))
                {
                    for (int j = 0; j < i; j++)
                    {
                        rs = rs + pathComponent[j] + @"\";
                    }
                    break;
                }
            }
            rs = rs + @"ProductImages\";
            return rs;
        }
        public static string getFileExtention(OpenFileDialog fileDialog)
        {
            return Path.GetExtension(fileDialog.FileName);
        }
        public static void createTempFolder(string path)
        {
            Directory.CreateDirectory(path + "temp");
        }
        public static void createTempImage(string path, string imageName, string extention)
        {
            File.Copy(path + imageName + extention, path + @"temp\" + imageName + "Temp" + extention, true);
        }
        public static void deleteFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
