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

        private void UpLoad_Load(object sender, EventArgs e)
        {
            if (listViewItemCollection == null || listViewItemCollection.Count == 0)
            {
                MessageBox.Show("请先启动程序，或手动加载压缩文件");
                this.Close();
            }
            Temp temp = new Temp();
            foreach (ListViewItem item in listViewItemCollection)
            {
                var address = item.SubItems[8].Text;
                temp.CreateTemp(address);
            }
            temp.StartTask(listViewItemCollection.Count);
            

            LanZhouHelper lanZhouHelper = new LanZhouHelper();
            if (LanZhouHelper.cookieContainer.Count == 1)
            {
                MessageBox.Show("请先录入蓝奏云用户名或密码");
                this.Close();
                return;
            }
            lanZhouHelper.loadSendMsg += LanZhouHelper_loadSendMsg;
            int i = 0;
            foreach (var item in Directory.GetFileSystemEntries(fileAddress))
            {
                if (Path.GetExtension(item).Equals(".zip"))
                {
                    var fs = File.OpenRead(item);
                    string ext = Path.GetExtension(item);
                    string fileNmae = Path.GetFileName(item);
                    listView1.Items.Add(new ListViewItem(new string[] { "", "", "", "" }));
                    lanZhouHelper.FileUpload(fs, fileNmae, ext, i);
                    i++;
                }
            }
        }

        private void LanZhouHelper_loadSendMsg(UpLoadMsg msg)
        {
           
            this.Invoke(new MethodInvoker(() =>
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    listView1.Items[msg.Id].SubItems[0].Text = msg.FileName;
                    listView1.Items[msg.Id].SubItems[1].Text = msg.ShareAddress;
                    listView1.Items[msg.Id].SubItems[2].Text = msg.Pwd;
                    listView1.Items[msg.Id].SubItems[3].Text = msg.Status;
                    Application.DoEvents();
                });
            }));
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
