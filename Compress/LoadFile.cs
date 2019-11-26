using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress
{
    public class LoadFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Size { get; set; }
        public string SizeInfo { get; set; }
        public string Progress { get; set; }
        public string Status { get; set; }
        public string Pwd { get; set; }
        public string PwdFileUrl { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
    }

    public class UpLoadFile
    {
        public string FileName { get; set; }
        public string Pwd { get; set; }
        public string FileUrl { get; set; }
        public string Status { get; set; }
    }
}
