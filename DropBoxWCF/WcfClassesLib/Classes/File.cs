namespace WcfClassesLib
{
    public class File : FileSystemElement
    {
        public string ParentHash { get; set; }
        public byte[] Content { get; set; }
    }
}
