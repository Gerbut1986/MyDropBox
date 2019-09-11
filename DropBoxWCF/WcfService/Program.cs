namespace WcfService
{
    using WcfClassesLib;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.Data.Entity.Validation;

    public interface IConnection
    {
        [OperationContract]
        void Quit(string login);
    }
    [ServiceContract]
    public interface IAccount
    {
        [OperationContract]
        string Auth(string login, string password);
        [OperationContract]
        bool Register(User user);
    }
    [ServiceContract]
    public interface ICloud
    {
        // Get Data
        [OperationContract]
        Folder GetFolder(string folderHash,string userLogin);
        [OperationContract]
        File GetFile(string fileHash);
        [OperationContract]
        List<Folder> GetFolderHashesByName(string folderName, string login);

        // FileSystemOperations
        [OperationContract]
        void UploadFile(string userLogin,string folderHash, string fileName, byte[] file);
        [OperationContract]
        void CreateFolder(string name, string parentFolderHash, string userLogin);
        [OperationContract]
        void DeleteElement(string hash);
        [OperationContract]
        void MoveElement(string hashSource, string hashDestinationFolder);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CloudService : ICloud, IAccount, IConnection
    {
        //public List<string> currentUsersLogins = new List<string>();

        public string Auth(string login, string password)
        {
            using (DropBoxCloudEntities db = new DropBoxCloudEntities())
            {
                if (db.Users.Where(t => t.Login.Equals(login)).Count() > 0)
                {
                    if (db.Users.Where(t => t.Login.Equals(login)).FirstOrDefault().Password.Equals(password))
                    {
                        if (/*currentUsersLogins.Contains(login).Equals(false)*/true)
                        {
                            //currentUsersLogins.Add(login);
                            db.SaveChanges();
                            return db.Users.Where(t=>t.Login.Equals(login)).FirstOrDefault().Name;
                        }
                    }
                }
            }
            return string.Empty;
        }
        public bool Register(User user)
        {
            using (DropBoxCloudEntities db = new DropBoxCloudEntities())
            {
                if (db.Users.Where(t => t.Login.Equals(user.Login)).Count() == 0)
                {
                    db.Users.Add(new Users { Name = user.Name, Login = user.Login, Password = user.Password, Email = user.Email }); // adding user
                    db.SaveChanges();
                    db.FileSystemElements.Add(new FileSystemElements
                    { Id_Owner = db.Users.Where(t=>t.Login.Equals(user.Login)).FirstOrDefault().Id, Hash = "root", Name = "root", Parent = "null", ElementType = 0 }); // creating root folder
                    db.SaveChanges();
                }
                else
                    return false;
            }
            return true;
        }
        public void CreateFolder(string name, string parentFolderHash, string userLogin)
        {
            try
            {
                using (DropBoxCloudEntities db = new DropBoxCloudEntities())
                {
                    db.FileSystemElements.Add(new FileSystemElements
                    {
                        Id_Owner = db.Users.Where(t => t.Login.Equals(userLogin)).Select(t => t.Id).FirstOrDefault(),
                        ElementType = 0,
                        Name = name,
                        Parent = parentFolderHash,
                        Hash = HashGenerator.GenerateHash()
                    });
                    db.FileSystemElements.Attach(new FileSystemElements
                    {
                        Id_Owner = db.Users.Where(t => t.Login.Equals(userLogin)).Select(t => t.Id).FirstOrDefault(),
                        ElementType = 0,
                        Name = name,
                        Parent = parentFolderHash,
                        Hash = HashGenerator.GenerateHash()
                    });
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void DeleteElement(string hash)
        {
            try
            {
                using (DropBoxCloudEntities db = new DropBoxCloudEntities())
                {
                    db.FileSystemElements.Remove(db.FileSystemElements.Where(t => t.Hash.Equals(hash)).FirstOrDefault());
                    db.SaveChanges();
                }
            }
            catch { }
        }
        public File GetFile(string fileHash)
        {
            File returnedFile = new File();
            using (DropBoxCloudEntities db = new DropBoxCloudEntities())
            {
                var file = db.FileSystemElements.Where(t => t.Hash.Equals(fileHash)).FirstOrDefault();
                db.SaveChanges();

                returnedFile = new File { Name = file.Name, Content = file.Content, Hash = file.Hash, ParentHash = file.Parent };
            }

            return returnedFile;
        }
        public Folder GetFolder(string folderHash,string userLogin)
        {
            dynamic folder = null;

            List<Folder> folderContentFolders = new List<Folder>();
            List<File> folderContentFiles = new List<File>();

            using (DropBoxCloudEntities db = new DropBoxCloudEntities())
            {
                folder        = db.FileSystemElements.Where(t => t.Hash.Equals(folderHash)).Where(t=>t.Id_Owner.Equals( db.Users.Where(p=>p.Login.Equals(userLogin) ).FirstOrDefault().Id)).FirstOrDefault();

                var collection = db.FileSystemElements.Where(t => t.Parent.Equals(folderHash)).ToList();
                foreach(var el in collection)
                {
                    if (el.Content != null)
                        folderContentFiles.Add(new File { Name = el.Name, Hash = el.Hash, ElementType = 1, ParentHash = el.Parent });
                    else
                        folderContentFolders.Add(new Folder { Name= el.Name, Hash = el.Hash,
                            ElementType = 1, ParentHash = el.Parent });
                }
                db.SaveChanges();
            }

            return new Folder { Name = folder.Name, Content = new Tuple<List<Folder>, List<File>>(folderContentFolders, folderContentFiles), Hash = folder.Hash, ParentHash = folder.Parent };
        }
        public void MoveElement(string hashSource, string hashDestinationFolder)
        {
            using (DropBoxCloudEntities db = new DropBoxCloudEntities())
            {
                db.FileSystemElements.Where(t => t.Hash.Equals(hashSource)).FirstOrDefault().Parent = hashDestinationFolder;
                db.SaveChanges();
            }
        }
        public void UploadFile(string userLogin, string folderHash, string fileName, byte[] file)
        {
            try
            {
                using (DropBoxCloudEntities db = new DropBoxCloudEntities())
                {
                    //db.FileSystemElements.Add(new FileSystemElements
                    //{ Name = fileName, Parent = folderHash, ElementType = 1, Content = file, Id_Owner = db.Users.Where(t => t.Login.Equals(userLogin)).FirstOrDefault().Id, Hash = HashGenerator.GenerateHash() });
                    //db.SaveChanges();
                    //db.FileSystemElements.Attach(new FileSystemElements
                    //{ Name = fileName, Parent = folderHash, ElementType = 1, Content = file, Id_Owner = db.Users.Where(t => t.Login.Equals(userLogin)).FirstOrDefault().Id, Hash = HashGenerator.GenerateHash() });
                    //db.SaveChanges();
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); List<string> errorMessages = new List<string>();
                foreach (DbEntityValidationResult validationResult in ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        Console.WriteLine(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
            }
        }
        public void Quit(string login)
        {
            //currentUsersLogins.Remove(login);
        }

        public List<Folder> GetFolderHashesByName(string folderName, string login)
        {
            using (DropBoxCloudEntities db = new DropBoxCloudEntities())
            {
                var collection =
                    db.FileSystemElements.Where(t => t.Name.Equals(folderName))
                    .Where(t => t.Id_Owner.Equals(db.Users.Where(p => p.Login.Equals(login))
                    .FirstOrDefault().Id)).Select(t => new Folder { Hash = t.Hash, Name = t.Name, ElementType = t.ElementType, ParentHash = t.Parent }).ToList();
                return collection;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(CloudService),
         new Uri("net.tcp://localhost/DropBox")/* базовый адрес */))
            {
                // добавление оконечной точки для общения с клиентом (авторизация)
                host.AddServiceEndpoint(
                    typeof(IAccount), /* контракт */
                    new NetTcpBinding { SendTimeout = TimeSpan.MaxValue },  /* привязка */
                    "net.tcp://localhost/DropBox/Account"); /* адрес не указан, значит совпадает с базовым */

                // добавление оконечной точки для общения с клиентом (работа с облаком)
                host.AddServiceEndpoint(
                    typeof(ICloud), /* контракт */
                    new NetTcpBinding { SendTimeout = TimeSpan.MaxValue, MaxBufferSize = int.MaxValue, MaxReceivedMessageSize = int.MaxValue },  /* привязка */
                    "net.tcp://localhost/DropBox/Cloud"); /* адрес не указан, значит совпадает с базовым */

                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior(); // объект-поведение

                host.Description.Behaviors.Add(behavior); // добавили поведение

                // добавление оконечной точки для MEX
                host.AddServiceEndpoint(
                   typeof(IMetadataExchange), /* контракт mex */
                   MetadataExchangeBindings.CreateMexTcpBinding(),  /* привязка mex */
                   "mex"); /* оконечная точка mex */

                //((IContextChannel)host).OperationTimeout = TimeSpan.MaxValue;

                host.Open();
                Console.WriteLine("Service launched. Press any key to stop...");
                Console.ReadKey(true);
                Console.WriteLine("[SERVICE STOPPED]");
            }
        }
    }
}
