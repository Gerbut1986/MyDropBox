using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxWCF.Classes
{
    public static class UIWorker
    {
        public static PackIcon GetPackIconByFileExtension(string fileName)
        {
            PackIcon pi = null;
            switch (System.IO.Path.GetExtension(fileName).ToLower())
            {
                case ".doc":
                case ".docx":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FileWord };
                        break;
                    }
                case ".xls":
                case ".xlsx":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FileExcel };
                        break;
                    }
                case ".xml":
                case ".xaml":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FileXml };
                        break;
                    }
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".ico":
                case ".bmp":
                case ".tiff":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FileImage };
                        break;
                    }
                case ".pdf":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FilePdf };
                        break;
                    }
                case ".ppt":
                case ".pptx":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FilePowerpoint };
                        break;
                    }
                case ".mp4":
                case ".wmv":
                case ".mpg":
                case ".avi":
                case ".mkv":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FileVideo };
                        break;
                    }
                case ".txt":
                case ".rtf":
                case ".config":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FileDocument };
                        break;
                    }
                case ".mp3":
                case ".wav":
                case ".m3u":
                case ".m4a":
                    {
                        pi = new PackIcon { Kind = PackIconKind.FileMusic };
                        break;
                    }
                case ".cs":
                case ".cpp":
                case ".js":
                case ".sql":
                case ".html":
                case ".dll":
                case ".php":
                case ".csproj":
                case ".sln":
                    {
                        pi = new PackIcon { Kind = PackIconKind.CodeBraces };
                        break;
                    }
                case ".db":
                case ".ldf":
                case ".mdf":
                    {
                        pi = new PackIcon { Kind = PackIconKind.Database };
                        break;
                    }
                case ".exe":
                    {
                        pi = new PackIcon { Kind = PackIconKind.Matrix };
                        break;
                    }
                default:
                    pi = new PackIcon { Kind = PackIconKind.File };
                    break;
            }

            return pi;
        }
    }
}
