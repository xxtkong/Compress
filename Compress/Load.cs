using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compress
{
    public partial class Load : Form
    {
        private ListView listView;
        public Load(ListView listView)
        {
            this.listView = listView;
            InitializeComponent();
        }
        public delegate void LoadDelegate(LoadFile msg);
        public event LoadDelegate loaddetegate;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog path = new OpenFileDialog();
            path.Multiselect = true;
            path.Title = "请选择文件";
            path.Filter = "所有文件(*xls*)|*.xls*";
            if (path.ShowDialog() == DialogResult.OK)
            {
                this.txtfile.Text = path.FileName.Trim();

                DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(path.FileName.Trim(), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (ExcelPackage pck = new ExcelPackage(fs))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets["Sheet1"];
                        int minRowNum = ws.Dimension.Start.Row; 
                        int minColNum = ws.Dimension.Start.Column;
                        int maxRowNum = ws.Dimension.End.Row; 
                        int maxColNum = ws.Dimension.End.Column;
                        for (int i = minColNum; i <= maxColNum; i++)
                        {
                            object obj = ws.Cells[minRowNum, i].Value;
                            string colName = obj.ToString();
                            DataColumn datacolum = new DataColumn(colName);
                            dt.Columns.Add(datacolum);
                        }
                        for (int i = minRowNum + 1; i <= maxRowNum; i++)
                        {
                            DataRow dr = dt.NewRow();
                            for (int j = minColNum; j <= maxColNum; j++)
                            {
                                object obj = ws.Cells[i, j].Value;
                                dr[j - 1] = obj;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }

                foreach (DataRow item in dt.Rows)
                {
                    ListViewItem viewitem = new ListViewItem(new string[] { item[0].ToString(), item[1]?.ToString(), item[2]?.ToString(), item[3]?.ToString(), item[4]?.ToString(), item[5]?.ToString(), item[6]?.ToString(), item[7]?.ToString(), item[8]?.ToString() });
                    listView.Items.Add(viewitem);
                }
                this.Close();
            }
        }
    }
}
