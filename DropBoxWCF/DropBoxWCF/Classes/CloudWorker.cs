namespace DropBoxWCF.Classes
{
    using DropBoxWCF.ServiceReference1;
    using DropBoxWCF.Windows;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using WcfClassesLib;

    public static class CloudWorker
    {
        public static Certificate Certificate { get; set; }

        public static ICloud CloudClientChannel { get; set; } = null;
        public static IAccount AccountClientChannel { get; set; } = null;

        public static void CreateCloudClientChannel()
        {
            try
            {
                var myBinding = new NetTcpBinding { CloseTimeout = TimeSpan.MaxValue, OpenTimeout = TimeSpan.MaxValue, ReceiveTimeout = TimeSpan.MaxValue, SendTimeout
                = TimeSpan.MaxValue,
                };
                var myEndpoint = new EndpointAddress("net.tcp://localhost/DropBox/Cloud");
                var myChannelFactory = new ChannelFactory<ICloud>(myBinding, myEndpoint);

                CloudClientChannel = myChannelFactory.CreateChannel();
            }
            catch { }
        }

        public static string Auth(string login, string password) // Null if login or pass was incorrect
        {
            string result = string.Empty;
            try
            {
                Certificate = new Certificate();

                if (AccountClientChannel == null)
                {
                    try
                    {
                        var myBinding = new NetTcpBinding{
                            CloseTimeout = TimeSpan.MaxValue,
                            OpenTimeout = TimeSpan.MaxValue,
                            ReceiveTimeout = TimeSpan.MaxValue,
                            SendTimeout
                = TimeSpan.MaxValue,
                        };
                        var myEndpoint = new EndpointAddress("net.tcp://localhost/DropBox/Account");
                        var myChannelFactory = new ChannelFactory<IAccount>(myBinding, myEndpoint);

                        AccountClientChannel = myChannelFactory.CreateChannel();
                    }
                    catch { }
                }

                result = AccountClientChannel.Auth(login, password);
            }
            catch { }

            return result;
        }
        public static bool Register(User user)
        {
            bool result = false;

            try
            {
                if (AccountClientChannel == null)
                {
                    var myBinding = new NetTcpBinding {
                        CloseTimeout = TimeSpan.MaxValue,
                        OpenTimeout = TimeSpan.MaxValue,
                        ReceiveTimeout = TimeSpan.MaxValue,
                        SendTimeout
                = TimeSpan.MaxValue,
                    };
                    var myEndpoint = new EndpointAddress("net.tcp://localhost/DropBox/Account");
                    var myChannelFactory = new ChannelFactory<IAccount>(myBinding, myEndpoint);

                    AccountClientChannel = myChannelFactory.CreateChannel();
                }

                result = AccountClientChannel.Register(new WcfClassesLib.User { Login = user.Login, Email = user.Email, Name = user.Name, Password = user.Password });
            }
            catch { }
            return result;
        }

        public static WcfClassesLib.Folder getRootFolder(string userLogin)
        {
            CLog.Log("getRootFolder()");

            if (CloudClientChannel == null)
                CreateCloudClientChannel();

            return CloudClientChannel.GetFolder("root",userLogin);
        }
        public static List<Folder> getFoldersByName(string folderName, string userLogin)
        {
            CLog.Log("getFoldersByName()");


            if (CloudClientChannel == null)
                CreateCloudClientChannel();

            return CloudClientChannel.GetFolderHashesByName(folderName, userLogin).ToList();
        }
        public static Folder getFolderByHash(string hash,string userLogin)
        {
            var c= CloudClientChannel.GetFolder(hash, userLogin);

            foreach (var el in fList)
                c.Content.Item2.Add(el);
            //c.Content.Item2.Add(new File { Name="1.mp4" });
            return c;
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
        public static async void DeleteElement(string hash, Lobby lobby)
        {
            CLog.Log("DeleteElement()");
            if (CloudClientChannel == null)
                CreateCloudClientChannel();
            CloudClientChannel.DeleteElement(hash);


            lobby._currentFolder = await CloudClientChannel.GetFolderAsync(lobby._currentFolder.Hash,lobby.user.Login);
        }
        public static void MoveElement(string hashSource, string hashDestinationFolder)
        {
            CLog.Log("MoveElement()");
            if (CloudClientChannel == null)
                CreateCloudClientChannel();
            CloudClientChannel.MoveElement(hashSource, hashDestinationFolder);
        }
        private static List<File> fList = new List<File>();
        public static async void CreateFolder(string name, string hash, string userLogin, Lobby lobby)
        {
            await CloudClientChannel.CreateFolderAsync(name, hash, userLogin);
            lobby._currentFolder = await CloudClientChannel.GetFolderAsync(lobby._currentFolder.Hash,lobby.user.Login);
        }

        public static async void UploadFile(string userLogin, string fileName, string folderHash, byte[] file, Lobby lobby)
        {
            try
            {
                if (CloudClientChannel == null)
                    CreateCloudClientChannel();

                fList.Add(new File { Name = fileName, ParentHash = folderHash, Hash = folderHash, ElementType = 0 });

                CloudClientChannel.UploadFile(userLogin, fileName, folderHash, file);
                lobby._currentFolder = await CloudClientChannel.GetFolderAsync(lobby._currentFolder.Hash, lobby.user.Login);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static File DownloadFile(string fileHash)
        {
            if (CloudClientChannel == null)
                CreateCloudClientChannel();

            var file = CloudClientChannel.GetFile(fileHash);

            return file;
        }
    }
}
