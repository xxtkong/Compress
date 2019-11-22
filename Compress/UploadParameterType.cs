using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress
{
    public class UploadParameterType
    {
        public UploadParameterType()
        {
            FileNameKey = "fileName";
            Encoding = Encoding.UTF8;
            PostParameters = new Dictionary<string, string>();
        }
        /// <summary>
        /// 上传地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 文件名称key
        /// </summary>
        public string FileNameKey { get; set; }
        /// <summary>
        /// 文件名称value
        /// </summary>
        public string FileNameValue { get; set; }
        /// <summary>
        /// 编码格式
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 上传文件的流
        /// </summary>
        public Stream UploadStream { get; set; }
        /// <summary>
        /// 上传文件 携带的参数集合
        /// </summary>
        public IDictionary<string, string> PostParameters { get; set; }
    }
}
