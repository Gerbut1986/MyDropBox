using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfClassesLib
{
    public class FileSystemElement
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public int ElementType { get; set; } // 0 - Folder, 1 - File
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
