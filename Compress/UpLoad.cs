using Compress.UploadExt;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace Compress
{
    public partial class UpLoad : Form
    {
        private ListViewItemCollection listViewItemCollection;
        private string fileAddress = "";
        public UpLoad(ListViewItemCollection listViewItemCollection,string fileAddress)
        {
            this.listViewItemCollection = listViewItemCollection;
            this.fileAddress = fileAddress;
            InitializeComponent();
        }
        LanZhouHelper lanZhouHelper = new LanZhouHelper();
       
        private void UpLoad_Load(object sender, EventArgs e)
        {
            if (listViewItemCollection == null || listViewItemCollection.Count == 0)
            {
                MessageBox.Show("请先启动程序，或手动加载压缩文件");
                this.Close();
                return;
            }
            
            if (LanZhouHelper.cookieContainer.Count == 1)
            {
                MessageBox.Show("请先录入蓝奏云用户名或密码");
                this.Close();
                return;
            }

            ///创建临时文件并上传
            Temp temp = new Temp();
            temp.CompressMsg += Temp_CompressMsg;
            int i = 0;
            foreach (ListViewItem item in listViewItemCollection)
            {
                var address = item.SubItems[8].Text;
                listView1.Items.Add(new ListViewItem(new string[] { address, "", "", "" }));
                temp.AddTask(address, i);
                i++;
            }
            //启动线程
            temp.RunTask();
        }

        private void Temp_CompressMsg(UpLoadMsg msg)
        {
            switch (msg.Status)
            {
                case "压缩完成":
                    var fs = File.OpenRead(msg.FileName);
                    string ext = Path.GetExtension(msg.FileName);
                    string fileName = Path.GetFileName(msg.FileName);
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items[msg.Id].SubItems[3].Text = msg.Status;
                        Application.DoEvents();
                    });
                    //System.Net.ServicePointManager.DefaultConnectionLimit = 50;
                    System.GC.Collect();
                    lanZhouHelper.AddUpLoad(fs, fileName, ext, msg.Id, listView1);

                   
                    break;
                case "上传成功":
                    this.Invoke(new MethodInvoker(() =>
                    {
                        listView1.Items[msg.Id].SubItems[0].Text = msg.FileName;
                        listView1.Items[msg.Id].SubItems[1].Text = msg.ShareAddress;
                        listView1.Items[msg.Id].SubItems[2].Text = msg.Pwd;
                        listView1.Items[msg.Id].SubItems[3].Text = msg.Status;
                    }));
                    break;
                default:
                    break;
            }   
        }

        /// <summary>
        /// 下载listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            List<UpLoadFile> list = new List<UpLoadFile>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                list.Add(new UpLoadFile()
                {
                    FileName = listView1.Items[i].SubItems[0].Text,
                    FileUrl = listView1.Items[i].SubItems[1].Text,
                    Pwd = listView1.Items[i].SubItems[2].Text,
                    Status = listView1.Items[i].SubItems[3].Text,
                });
            }
            string fileaddress = "D:/上传文档.xlsx";
            Export(fileaddress, list);
            MessageBox.Show("下载地址:" + fileaddress, "下载成功");
        }

        public void Export(string fileAddress, IList<UpLoadFile> students)
        {
            using (FileStream fs = new FileStream(fileAddress, FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage(fs);
                package.Workbook.Worksheets.Add("Students");
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                #region write header
                sheet.Cells[1, 1].Value = "上传文件";
                sheet.Cells[1, 2].Value = "分享地址";
                sheet.Cells[1, 3].Value = "分享密码";
                sheet.Cells[1, 4].Value = "状态";
                using (ExcelRange range = sheet.Cells[1, 1, 1, 4])
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
                foreach (UpLoadFile s in students)
                {
                    sheet.Cells[pos, 1].Value = s.FileName;
                    sheet.Cells[pos, 2].Value = s.FileUrl;
                    sheet.Cells[pos, 3].Value = s.Pwd;
                    sheet.Cells[pos, 4].Value = s.Status;
                    pos++;
                }
                package.Save();
                #endregion
                fs.Close();
            }
        }

    }
}
