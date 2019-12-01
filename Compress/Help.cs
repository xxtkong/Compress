using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compress
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }
        
        private void Help_Load(object sender, EventArgs e)
        {
            labelVersion.Text = string.Format("当前版本：{0}", Assembly.GetEntryAssembly().GetName().Version);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.bangbangwo.net/Compress.pdf");

        }
    }
}
