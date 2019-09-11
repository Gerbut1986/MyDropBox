using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxWCF.Classes
{
    public class Folder : FileSystemElement
    {
        public string ParentHash { get; set; }
        public List<FileSystemElement> Content { get; set; } = new List<FileSystemElement>();
    }
}
