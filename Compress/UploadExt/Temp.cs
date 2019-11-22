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
        List<Task> list = new List<Task>();
        public void CreateTemp(string address)
        {
            var task = new Task(() => {
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
                    Directory.Delete(Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address), true);
                    Directory.Delete(Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address) + "temp", true);
                    File.Delete(Path.GetDirectoryName(address) + "\\" + Path.GetFileNameWithoutExtension(address) + ".rar");
                }
            });
            list.Add(task);
        }
        public void StartTask(int StartNum = 3)
        {
            for (int i = 0; i < StartNum; i++)
            {
                lock (list)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j].Status == TaskStatus.Created)
                        {
                            list[j].Start();
                            break;
                        }
                    }
                }
            }
        }
    }
}
