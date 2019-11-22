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
            temp.StartTask(3);
            

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
            DoExport(this.listView1, "上传");
        }


        private void DoExport(ListView listView, string strFileName)
        {
            int rowNum = listView.Items.Count;
            int columnNum = listView.Items[0].SubItems.Count;
            int rowIndex = 1;//行号
            int columnIndex = 0;//列号
            if (rowNum == 0 || string.IsNullOrEmpty(strFileName))//列表为空或导出的文件名为空
            {
                return;
            }
            if (rowNum > 0)
            {
                //加载Excel
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)//判断是否装了Excel
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel");
                    return;
                }
                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = true;//是否需要显示提示
                xlApp.SheetsInNewWorkbook = 1;//返回或设置Microsoft Excel自动插入到新工作簿中的工作表数。
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);//创建工作铺
                //将ListView的列名导入Excel表第一行
                foreach (ColumnHeader dc in listView.Columns)
                {
                    columnIndex++;//行号自增
                    xlApp.Cells[rowIndex, columnIndex] = dc.Text;
                }
                //将ListView中的数据导入Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;//列号自增
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        //注意这个在导出的时候加了“\t” 的目的就是避免导出的数据显示为科学计数法。可以放在每行的首尾。
                        xlApp.Cells[rowIndex, columnIndex] = Convert.ToString(listView.Items[i].SubItems[j].Text) + "\t";
                    }
                }
                //例外需要说明的是用strFileName,Excel.XlFileFormat.xlExcel9795保存方式时 当你的Excel版本不是95、97 而是2003、2007 时导出的时候会报一个错误：异常来自 HRESULT:0x800A03EC。 解决办法就是换成strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal。
                //xlBook.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, false, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                xlBook.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //xlApp = null;
                //xlBook = null;
                xlBook.Close(Type.Missing, Type.Missing, Type.Missing);
                xlApp.Quit();
                MessageBox.Show("导出文件成功！");
                GC.Collect();
            }
        }

    }
}
