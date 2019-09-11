namespace WcfClassesLib
{
    using System;
    using System.Collections.Generic;

    public class Folder : FileSystemElement
    {
        public string ParentHash { get; set; }
        public Tuple<List<Folder>, List<File>> Content { get; set; }
    }
}
