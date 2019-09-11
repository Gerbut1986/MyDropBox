using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxWCF.Classes
{
    public class File : FileSystemElement
    {
        public byte[] Content { get; set; }
    }
}
