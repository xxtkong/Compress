using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Compress.BaiduExt
{
    public class BaiduProgressInfo
    {
        internal bool is_download = false;
        internal long current_size = 0;
        internal long total_size = 0;
        internal long current_bytes = 0;
        internal long total_bytes = 0;
        internal int current_files = 0;
        internal int total_files = 0;
        internal string local_file = "";
        internal string remote_file = "";
        internal WebClient m_web_client = null;
    }
}
