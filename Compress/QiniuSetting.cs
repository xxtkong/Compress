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
    public partial class QiniuSetting : Form
    {
        public QiniuSetting()
        {
            InitializeComponent();
        }

        private void QiniuSetting_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        IniFileHelper iniFileHelper = new IniFileHelper();
        private void button1_Click(object sender, EventArgs e)
        {
            var ak = this.txtAK.Text;
            var sk = this.txtSK.Text;
            var bucket = this.cb1.Text;
            var Zone = this.txtZone.Text;
            var Domain = this.txtDomain.Text;
            iniFileHelper.WriteIniString("qiniu", "AK", ak);
            iniFileHelper.WriteIniString("qiniu", "SK", sk);
            iniFileHelper.WriteIniString("qiniu", "Bucket", bucket);
            iniFileHelper.WriteIniString("qiniu", "Zone", Zone);
            iniFileHelper.WriteIniString("qiniu", "Domain", Domain);
            MessageBox.Show("保存成功");
            this.Close();
        }

        private void QiniuSetting_Load(object sender, EventArgs e)
        {
            StringBuilder sbAK = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "AK", "", sbAK, sbAK.Capacity);
            this.txtAK.Text = sbAK.ToString();

            StringBuilder sbSK = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "SK", "", sbSK, sbSK.Capacity);
            this.txtSK.Text = sbSK.ToString();

            StringBuilder sbBucket = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "Bucket", "", sbBucket, sbBucket.Capacity);
            this.cb1.Text = sbBucket.ToString();

            StringBuilder sbZone = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "Zone", "", sbZone, sbZone.Capacity);
            this.txtZone.Text = sbZone.ToString();

            StringBuilder sbDomain = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "Domain", "", sbDomain, sbDomain.Capacity);
            this.txtDomain.Text = sbDomain.ToString();
        }
    }
}
