using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compress
{
    public partial class PicSeeting : Form
    {
        public PicSeeting()
        {
            InitializeComponent();
        }
        IniFileHelper iniFileHelper = new IniFileHelper();
        private void button1_Click(object sender, EventArgs e)
        {
            var pid = this.txtPid.Text;
            var key = this.txtKey.Text;
            iniFileHelper.WriteIniString("pic", "Pid", pid);
            iniFileHelper.WriteIniString("pic", "Key", key);
            MessageBox.Show("保存成功");
            this.Close();
        }

        private void PicSeeting_Load(object sender, EventArgs e)
        {
            StringBuilder sbPid = new StringBuilder(60);
            iniFileHelper.GetIniString("pic", "Pid", "", sbPid, sbPid.Capacity);
            this.txtPid.Text = sbPid.ToString();

            StringBuilder sbKey = new StringBuilder(60);
            iniFileHelper.GetIniString("pic", "Key", "", sbKey, sbKey.Capacity);
            this.txtKey.Text = sbKey.ToString();
        }
    }
}
