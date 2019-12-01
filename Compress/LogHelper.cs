using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress
{
    public class LogHelper
    {
        public static string path = "c:\\logs";
        /// <summary>
        /// 直接写文件，文件地址D:\\logs
        /// </summary>
        /// <param name="error"></param>
        public static void WriteLogFile(string error)
        {
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名
            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename);
            //向日志文件写入内容
            string write_content = time + ":" + error;
            mySw.WriteLine(write_content);
            //关闭日志文件
            mySw.Close();
        }
    }
}
