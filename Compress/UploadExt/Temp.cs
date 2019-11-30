using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Compress.UploadExt
{
    public class Temp
    {
        public Temp()
        {
            CompressMsg += Temp_CompressMsg;
        }

        private void Temp_CompressMsg(UpLoadMsg msg)
        {
            if (msg.Status == "压缩完成")
            {
                RunTask(1);
            }
        }
        private void CreateTemp(string address,int i)
        {
            var txtaddress = Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address) + ".txt";
            //创建临时文件夹，合并2个文件并压缩
            var temp = Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address) + "temp";
            if (!Directory.Exists(temp))
                Directory.CreateDirectory(temp);
            //删除文件夹内文件
            foreach (string d in Directory.GetFileSystemEntries(temp))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);
                }
            }
            //复制文件到文件夹
            if (File.Exists(address))
                FileHelper.CopyDirectory(address, temp, true);
            if (File.Exists(txtaddress))
                FileHelper.CopyDirectory(txtaddress, temp, true);
            bool b = FileHelper.ZipDirectory(temp, temp + ".zip");
            //删除文件夹及rar 压缩文件
            if (b)
            {
                CompressMsg(new UpLoadMsg() { Id =i, FileName = temp+".zip", Status = "压缩完成" });
                var old = Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address);
                if(Directory.Exists(old))
                    Directory.Delete(old, true);
                if (Directory.Exists(temp))
                    Directory.Delete(temp, true);
                var rar = Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address) + ".rar";
                if (File.Exists(rar))
                    File.Delete(rar);
                var txt = Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address) + ".txt";
                if (File.Exists(txt))
                    File.Delete(txt);
            }
        }

        public delegate void UpLoadSendMsg(UpLoadMsg msg);
        public event UpLoadSendMsg CompressMsg;

        List<Task> tasks = new List<Task>();
        public void AddTask(string address,int i)
        {
            tasks.Add(new Task(() => {
                CreateTemp(address,i);
            }));
        }

        public void RunTask(int StartNum =3)
        {
            for (int i = 0; i < StartNum; i++)
            {
                lock (tasks)
                {
                    for (int j = 0; j < tasks.Count; j++)
                    {
                        if (tasks[j].Status == TaskStatus.Created)
                        {
                            tasks[j].Start();
                            break;
                        }
                    }
                }
            }
        }
    }
}
