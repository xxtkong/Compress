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
    public partial class LanZhouSetting : Form
    {
        public LanZhouSetting()
        {
            InitializeComponent();
        }
        IniFileHelper iniFileHelper = new IniFileHelper();
        private void button1_Click(object sender, EventArgs e)
        {
            var username = this.txtUserName.Text;
            var pwd = this.txtPwd.Text;
            iniFileHelper.WriteIniString("LanZhou", "UserName", username);
            iniFileHelper.WriteIniString("LanZhou", "Pwd", pwd);
            MessageBox.Show("保存成功");
            this.Close();
        }

        private void LanZhouSetting_Load(object sender, EventArgs e)
        {
            StringBuilder sbPid = new StringBuilder(60);
            iniFileHelper.GetIniString("LanZhou", "UserName", "", sbPid, sbPid.Capacity);
            this.txtUserName.Text = sbPid.ToString();

            StringBuilder sbKey = new StringBuilder(60);
            iniFileHelper.GetIniString("LanZhou", "Pwd", "", sbKey, sbKey.Capacity);
            this.txtPwd.Text = sbKey.ToString();
        }
    }
}
