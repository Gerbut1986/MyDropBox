using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfClassesLib
{
    public class File : FileSystemElement
    {
        public string ParentHash { get; set; }
        public byte[] Content { get; set; }
    }
}
