namespace WcfService
{    
    public partial class FileSystemElements
    {
        public int Id { get; set; }
        public int Id_Owner { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Parent { get; set; }
        public int ElementType { get; set; }
        public byte[] Content { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
