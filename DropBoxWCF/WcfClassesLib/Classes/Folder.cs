using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfClassesLib
{
    public class Folder : FileSystemElement
    {
        public string ParentHash { get; set; }
        public Tuple<List<Folder>, List<File>> Content { get; set; }
    }
}
