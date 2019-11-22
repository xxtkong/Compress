using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Compress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.txtFileAddress.Text = path.SelectedPath.Trim();
        }
        CompressHelper compressHelper = new CompressHelper();
        private void button2_Click(object sender, EventArgs e)
        {
            if (isConnected())
            {
                //button2.Enabled = false;
                //设置最大活动线程数以及可等待线程数
                ThreadPool.SetMaxThreads(10, 1);
                var fileAddress = this.txtFileAddress.Text;
                if (string.IsNullOrEmpty(fileAddress))
                {
                    MessageBox.Show("请选择需要压缩文件路径");
                    return;
                }
                //获取保存文件夹路径
                var saveAddress = this.txtFileAddress2.Text;
                if (string.IsNullOrEmpty(saveAddress))
                {
                    MessageBox.Show("请选择压缩后生成文件路径");
                    return;
                }
                string[] filenames = Directory.GetFileSystemEntries(fileAddress);
                var elseAddress = this.txtFileAddress3.Text;
                int i = 0;string password = "";int bPrice = 0, ePrice = 0;
                if (rb1.Checked)
                    password = "0";
                else if (rb2.Checked)
                    password = this.txtpass.Text;
                if (rb4.Checked)
                {
                    bPrice = int.Parse(this.txtPrice.Text);
                }
                else
                {
                    bPrice = int.Parse(this.txtBPrice.Text);
                    ePrice = int.Parse(this.txtEPrice.Text);
                }
                foreach (var item in filenames)
                {
                    var size = new FileInfo(item).Length / (1024);
                    ListViewItem listView = new ListViewItem(new string[] { Path.GetFileNameWithoutExtension(item), size+"KB", "0", "0", "未开始","","","","" });
                    listView1.Items.Add(listView);   
                    compressHelper.AddCompress(saveAddress, item, elseAddress, password, i,bPrice,ePrice);
                    i++;
                }
                compressHelper.StartCompress();
            }
            else
            {
                MessageBox.Show("网络异常!");
            }
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.txtFileAddress2.Text = path.SelectedPath.Trim();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.txtFileAddress3.Text = path.SelectedPath.Trim();
        }

       

        #region 检查网络
        //检测网络状态
        [DllImport("wininet.dll")]
        extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        /// <summary>
        /// 检测网络状态
        /// </summary>
        bool isConnected()
        {
            int I = 0;
            bool state = InternetGetConnectedState(out I, 0);
            return state;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            compressHelper.ThreadNum = 3;//线程数，不设置默认为3
            compressHelper.doSendMsg += SendMsgHander;//压缩过程处理事件
        }

        private void SendMsgHander(CompressMsg msg)
        {
            switch (msg.Tag)
            {
                case CompressStatus.Start:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items[msg.Id].SubItems[4].Text = "开始压缩";
                    });
                    break;
                case CompressStatus.GetLength:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items[msg.Id].SubItems[4].Text = "链接成功";
                    });
                    break;
                case CompressStatus.Compress:
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            listView1.Items[msg.Id].SubItems[2].Text = msg.SizeInfo;
                            listView1.Items[msg.Id].SubItems[3].Text = msg.Progress.ToString() + "%";
                            listView1.Items[msg.Id].SubItems[4].Text = "压缩中";
                            listView1.Items[msg.Id].SubItems[5].Text = msg.Pwd;
                            listView1.Items[msg.Id].SubItems[8].Text = msg.Address;
                            Application.DoEvents();
                        });
                    }));
                    break;
                case CompressStatus.End:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items[msg.Id].SubItems[2].Text = "已完成";
                        listView1.Items[msg.Id].SubItems[3].Text =  "100%";
                        listView1.Items[msg.Id].SubItems[4].Text = "压缩完成";
                        listView1.Items[msg.Id].SubItems[5].Text = msg.Pwd;
                        listView1.Items[msg.Id].SubItems[6].Text = msg.PwdFileUrl;
                        listView1.Items[msg.Id].SubItems[7].Text = msg.Price.ToString();
                    });
                    break;
                case CompressStatus.Error:
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items[msg.Id].SubItems[4].Text = "失败";
                    });
                    break;
                default:
                    break;
            }
        }

        private void qiniuTSM_Click(object sender, EventArgs e)
        {
            QiniuSetting qiniu = new QiniuSetting();
            qiniu.StartPosition = FormStartPosition.CenterScreen;
            qiniu.Show();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listview = (ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            System.Windows.Forms.ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            try
            {
                Clipboard.SetDataObject(strText);
                NotifyIcon notifyIcon = new NotifyIcon();
                notifyIcon.Visible = true;
                string info = string.Format("内容【{0}】已经复制到剪贴板", strText);
                notifyIcon.ShowBalloonTip(1500, "提示", info, ToolTipIcon.Info);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picSeeting_Click(object sender, EventArgs e)
        {
            PicSeeting pic = new PicSeeting();
            pic.StartPosition = FormStartPosition.CenterScreen;
            pic.Show();
        }

        /// <summary>
        /// 上传至蓝奏云
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpLoadLanZhou_Click(object sender, EventArgs e)
        {
            UpLoad load = new UpLoad(this.listView1.Items,this.txtFileAddress2.Text);
            load.StartPosition = FormStartPosition.CenterScreen;
            load.Show();
        }

        private void lanzhouSM_Click(object sender, EventArgs e)
        {
            LanZhouSetting lanZhou = new LanZhouSetting();
            lanZhou.StartPosition = FormStartPosition.CenterScreen;
            lanZhou.Show();
        }

        private void BaiduSM_Click(object sender, EventArgs e)
        {
            BaiduSetting baidu = new BaiduSetting();
            baidu.StartPosition = FormStartPosition.CenterScreen;
            baidu.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            BaiduUpLoad baiduUpLoad = new BaiduUpLoad();
            baiduUpLoad.StartPosition = FormStartPosition.CenterScreen;
            baiduUpLoad.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<LoadFile> list = new List<LoadFile>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                list.Add(new LoadFile()
                {
                    FileName = listView1.Items[i].SubItems[0].Text,
                    Size = listView1.Items[i].SubItems[1].Text,
                    SizeInfo = listView1.Items[i].SubItems[2].Text,
                    Progress = listView1.Items[i].SubItems[3].Text,
                    Status = listView1.Items[i].SubItems[4].Text,
                    Pwd = listView1.Items[i].SubItems[5].Text,
                    PwdFileUrl = listView1.Items[i].SubItems[6].Text,
                    Price = listView1.Items[i].SubItems[7].Text,
                    Address = listView1.Items[i].SubItems[8].Text
                });
            }
            string fileaddress = "D:/压缩文档.xlsx";
            Export(fileaddress, list);
            MessageBox.Show("下载地址:" + fileaddress, "下载成功");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Load load = new Load(this.listView1);
            load.loaddetegate += Load_loaddetegate;
            load.StartPosition = FormStartPosition.CenterScreen;
            load.Show();
        }

        private void Load_loaddetegate(LoadFile msg)
        {
            throw new NotImplementedException();
        }

        public  void Export(string fileAddress,IList<LoadFile> students)
        {
            using (FileStream fs = new FileStream(fileAddress, FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage(fs);
                package.Workbook.Worksheets.Add("Students");
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                #region write header
                sheet.Cells[1, 1].Value = "文档名";
                sheet.Cells[1, 2].Value = "总大小";
                sheet.Cells[1, 3].Value = "总完成";
                sheet.Cells[1, 4].Value = "进度";
                sheet.Cells[1, 5].Value = "状态";
                sheet.Cells[1, 6].Value = "压缩密码";
                sheet.Cells[1, 7].Value = "密码地址";
                sheet.Cells[1, 8].Value = "价格(分)";
                sheet.Cells[1, 9].Value = "物理路径";
                using (ExcelRange range = sheet.Cells[1, 1, 1, 9])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                    range.AutoFitColumns(4);
                }
                #endregion

                #region write content
                int pos = 2;
                foreach (LoadFile s in students)
                {
                    sheet.Cells[pos, 1].Value = s.FileName;
                    sheet.Cells[pos, 2].Value = s.Size;
                    sheet.Cells[pos, 3].Value = s.SizeInfo;
                    sheet.Cells[pos, 4].Value = s.Progress;
                    sheet.Cells[pos, 5].Value = s.Status;
                    sheet.Cells[pos, 6].Value = s.Pwd;
                    sheet.Cells[pos, 7].Value = s.PwdFileUrl;
                    sheet.Cells[pos, 8].Value = s.Price;
                    sheet.Cells[pos, 9].Value = s.Address;
                    pos++;
                }
                package.Save();
                #endregion
                fs.Close();
            }
        }
    }
}
