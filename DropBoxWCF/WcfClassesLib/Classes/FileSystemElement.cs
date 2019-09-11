namespace WcfClassesLib
{
    using System;

    public class FileSystemElement
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public int ElementType { get; set; } // 0 - Folder, 1 - File
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
