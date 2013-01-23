using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProjektSoftwareverteilung2013.Models;

namespace ProjektSoftwareverteilung2013.Controller
{
    class FileController
    {
        private string UserDocumentsPaht = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string ProgrammPath = null;

        public FileController()
        {
            ProgrammPath = UserDocumentsPaht + "\\Softwareverteilung";
        }

        public FileController(List<GroupInfoModel> groups)
        {
            ProgrammPath = UserDocumentsPaht + "\\Softwareverteilung";

            for (int i = 0; i < groups.Count; i++)
            {
                GroupInfoModel group = groups[i];
                string groupName = group.Name;
                string path = ProgrammPath + "\\" + groupName;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("Directory creat:" + groupName);
                }
            }
        }

        public string getPathFromGroup(string groupName)
        {
            string path = "";

            if (groupName != "")
            {
                path = ProgrammPath + "\\" + groupName;
            }

            return path;
        }

        public string getPathFromFile(string groupName, string fileName)
        {
            string path = "";

            if (groupName != "" && fileName != "")
            {
                path = ProgrammPath + "\\"+ groupName + "\\" + fileName;
            }
            return path;
        }

        public bool creatGroupOrder(string name)
        {
            string path = "";
            if (name != "")
            {
                path = ProgrammPath + "\\" + name;

                if (Directory.Exists(path))
                {
                    return true;
                }
                Directory.CreateDirectory(path);
                return true;
            }
            
            return false;
        }

        public bool delGroupOrder(string name)
        {
            string path = "";
            if (name !="")
            {
                path = ProgrammPath + "\\" + name;

                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
