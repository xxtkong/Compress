using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Compress
{
    public class CompressThread
    {
       
        /// <summary>
        /// 启动压缩线程
        /// </summary>
        public void ThreadRun()
        {
            Thread td = new Thread(new ThreadStart(() =>
            {
               
            }));
            td.IsBackground = true;
            td.Start();
        }
        private bool finish = false;
        /// <summary>
        /// 压缩是否完成
        /// </summary>
        /// <returns></returns>
        public bool isFinish()
        {
            return finish;
        }
    }
}
