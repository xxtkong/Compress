using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.UploadExt
{
    public class UpLoadMsg
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ShareAddress { get; set; }
        public string Pwd { get; set; }
        public string Status { get; set; }
    }
}
