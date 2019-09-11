using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxWCF.Classes
{
    public class FileSystemElement
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
