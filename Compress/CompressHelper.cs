using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Threading.ThreadState;

namespace Compress
{
    public class CompressHelper
    {
        public int ThreadNum = 3;
        List<Thread> list = new List<Thread>();

        public CompressHelper()
        {
            doSendMsg += CompressHelper_doSendMsg;
        }

        private void CompressHelper_doSendMsg(CompressMsg msg)
        {
            if (msg.Tag == CompressStatus.Error || msg.Tag == CompressStatus.End)
            {
                StartCompress(1);
            }
        }

        public void AddCompress(string saveAddress,string item,string elseAddress, string password = "", int id = 0, int bPrice = 0, int ePrice = 0)
        {
            Thread tsk = new Thread(() =>
            {
                Operation(saveAddress, item, elseAddress, password,id, bPrice, ePrice);
            });
            list.Add(tsk);
        }

        public void Operation(string saveAddress, string item, string elseAddress,string password ="",int id = 0, int bPrice = 0, int ePrice = 0)
        {
            try
            {
                CompressMsg compress = new CompressMsg();
                compress.Tag = CompressStatus.Start;
                compress.Id = id;
                doSendMsg(compress);
                //创建保存文件夹
                var saveDirectory = saveAddress + "\\" + Path.GetFileNameWithoutExtension(item);
                Directory.CreateDirectory(saveDirectory);
                //复制当前文件到文件夹
                FileHelper.CopyDirectory(item, saveDirectory, true);
                //获取另外文件路径
                if (!string.IsNullOrEmpty(elseAddress))
                    FileHelper.CopyDirectory(elseAddress, saveDirectory, true);
                if (password == "0") //生成随机6位数压缩密码
                    password = FileHelper.GetRnd(6, true, true, true, true, "");
                //压缩文件夹
                FileHelper.CondenseRarOrZip(saveDirectory, saveAddress + "\\" + Path.GetFileNameWithoutExtension(item), true, password);
               // FileHelper.ZipDirectory(saveDirectory, saveDirectory + ".rar", password);
               // FileHelper.ZipFile()
                compress.Tag = CompressStatus.Compress;
                compress.Pwd = password;
                compress.Address = saveAddress + "\\" + Path.GetFileNameWithoutExtension(item)+".rar";
                doSendMsg(compress);

                ///创建密码图片
                FileHelper.CreateImgages("解压密码：" + password, saveAddress + "\\" + Path.GetFileNameWithoutExtension(item) + ".jpg");
               
                ///上传密码图片
                FileStream fs = File.OpenRead(saveAddress + "\\" + Path.GetFileNameWithoutExtension(item) + ".jpg");
                if (ePrice == 0)
                    compress.Price = bPrice;
                else
                    compress.Price = new Random().Next(bPrice, ePrice);
                compress.File = saveAddress + "\\" + Path.GetFileNameWithoutExtension(item);


                QiniuHelper qiniuHelper = new QiniuHelper(compress);
                qiniuHelper.doSendMsg += QiniuHelper_doSendMsg;
                qiniuHelper.Upload(fs, Path.GetFileNameWithoutExtension(item));



                //try
                //{
                //    byte[] buffur = new byte[fs.Length];
                //    fs.Read(buffur, 0, (int)fs.Length);
                //}
                //catch (Exception)
                //{
                //    throw;
                //}
                //finally
                //{
                //    if (fs != null)
                //        fs.Close();
                //}
            }
            catch (Exception)
            {
                CompressMsg compress = new CompressMsg();
                compress.Tag = CompressStatus.Error;
                doSendMsg(compress);
            }
        }
        IniFileHelper iniFileHelper = new IniFileHelper();
        private void QiniuHelper_doSendMsg(CompressMsg msg)
        {
            msg.Tag = CompressStatus.End;
            //上传至8图片
            StringBuilder sbPid = new StringBuilder(60);
            iniFileHelper.GetIniString("pic", "Pid", "", sbPid, sbPid.Capacity);
            StringBuilder sbKey = new StringBuilder(60);
            iniFileHelper.GetIniString("pic", "Key", "", sbKey, sbKey.Capacity);
            var result = HttpHelper.Get<PicResult>("http://web.8tupian.com/api/b.php?act=up1&pic=" + msg.PwdFileUrl + "&price=" + msg.Price + "&pid=" + sbPid + "&key=" + sbKey + "");
            if (result.code == 0)
                msg.PwdFileUrl = result.picurl;
            else
                msg.PwdFileUrl = "上传失败";
            doSendMsg(msg);
            //删除密码图片及文件夹
            Task.Run(() => {
                if (!System.IO.File.Exists(msg.File + ".txt"))
                {
                    byte[] bytes = null;
                    bytes = Encoding.UTF8.GetBytes("在浏览器输入地址查看解压密码。" + msg.PwdFileUrl);
                    FileStream fs = new FileStream(msg.File + ".txt", FileMode.Create, FileAccess.Write);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
                else
                {
                    byte[] bytes = null;
                    bytes = Encoding.UTF8.GetBytes("在浏览器输入地址查看解压密码。" + msg.PwdFileUrl);
                    FileStream fs = new FileStream(msg.File + ".txt", FileMode.Open, FileAccess.Write);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
                

                var img = msg.File + ".jpg";
                File.Delete(img);
                Directory.Delete(msg.File);
            });
        }

        public void StartCompress(int StartNum = 3)
        {
            for (int i2 = 0; i2 < StartNum; i2++)
            {
                lock (list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ThreadState == ThreadState.Unstarted || list[i].ThreadState == ThreadState.Suspended)
                        {
                            list[i].Start();
                            break;
                        }
                    }
                }
            }
        }
        public delegate void dlgSendMsg(CompressMsg msg);
        public event dlgSendMsg doSendMsg;
    }
}
