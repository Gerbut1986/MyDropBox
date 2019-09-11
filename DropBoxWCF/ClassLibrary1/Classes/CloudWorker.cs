using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxWCF.Classes
{
    public static class CloudWorker
    {
        public static Certificate Certificate { get; set; }

        public static User Auth(string login, string password) // Null if login or pass was incorrect
        {
            Certificate = new Certificate();


            return new User();
        }
        public static bool Register(User user)
        {
            return true;
        }

        public static Folder getRootFolder()
        {
            CLog.Log("getRootFolder()");
            return new Folder() { Content = new List<FileSystemElement>() { new Folder() { Name="folder" }, new File { Name= "file.mp4" }, new File { Name = "file_1.mp3" }, } };
        }
        public static List<Folder> getFoldersByName (string folderName)
        {

            CLog.Log("getFoldersByName()");
            return new List<Folder>();
        }
        public static Folder getFolderByHash(string hash)
        {
            return new Folder();
        }
        public static byte[] getFileContent(string hash)
        {
            CLog.Log("getFileContent()");
            return null;
        }
        public static List<FileSystemElement> getFolderContent(string hash)
        {
            CLog.Log("getFolderContent()");
            return null;
        }
        public static void DeleteElement(string hash)
        {
            CLog.Log("DeleteElement()");

        }
        public static void MoveElement(string hashSource, string hashDestinationFolder)
        {
            CLog.Log("MoveElement()");
        }
        public static void CreateFolder(string name)
        {

        }

        public static void UploadFile(string folderHash, byte[] file)
        {

        }
        public static File DownloadFile(string fileHash)
        {
            return new File();
        }
    }
}
