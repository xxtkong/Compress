//using Gecko;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Compress
{
    public partial class BaiduSetting : Form
    {
        public BaiduSetting()
        {
            InitializeComponent();
        }
        IniFileHelper iniFileHelper = new IniFileHelper();
        private void button1_Click(object sender, EventArgs e)
        {
            var username = this.txtUserName.Text;
            iniFileHelper.WriteIniString("Baidu", "UserName", username);

            var bdstoken = this.txtbdstoken.Text;
            iniFileHelper.WriteIniString("Baidu", "Bdstoken", bdstoken);

            MessageBox.Show("保存成功");
            this.Close();
        }
        //public GeckoWebBrowser browser;
        public WebBrowser browser;
        //public WebKitBrowser browser;
        private string loadUrl = "https://pan.baidu.com/";
        private void BaiduSetting_Load(object sender, EventArgs e)
        {
            StringBuilder sbPid = new StringBuilder(5000);
            iniFileHelper.GetIniString("Baidu", "UserName", "", sbPid, sbPid.Capacity);
            this.txtUserName.Text = sbPid.ToString();

            StringBuilder bdstoken = new StringBuilder(5000);
            iniFileHelper.GetIniString("Baidu", "Bdstoken", "", bdstoken, bdstoken.Capacity);
            this.txtbdstoken.Text = bdstoken.ToString();
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                //var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
                //Xpcom.Initialize(Path.Combine(app_dir, "xulrunner"));
                //browser = new GeckoWebBrowser();
                //browser.Dock = DockStyle.Fill;
                //this.browser.Name = "browser";
                //this.panel1.Controls.Add(browser);
                //browser.Navigate(loadUrl);



                browser = new WebBrowser();
                browser.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(browser);
                browser.Navigate(loadUrl);


                //webkit加载不了
                //browser = new WebKitBrowser();
                //browser.Dock = DockStyle.Fill;
                //this.panel1.Controls.Add(browser);
                //browser.Navigate(loadUrl);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            browser.Document.Cookie.Remove(0);
            //CookieManager.RemoveAll();
            MessageBox.Show("退出成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //CookieContainer myCookieContainer = new CookieContainer();
            //if (browser.Document.Cookie != null)
            //{
            //    string cookieStr = browser.Document.Cookie;
            //    string[] cookstr = cookieStr.Split(';');
            //    foreach (string str in cookstr)
            //    {
            //        string[] cookieNameValue = str.Split('=');
            //        Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
            //        ck.Domain = "www.baidu.com";
            //        myCookieContainer.Add(ck);
            //    }
            //}

            if (browser.Document.Cookie != null)
            {
                var cookieStr = GetCookieString("https://pan.baidu.com/");
                //string cookieStr = browser.Document.Cookie;
                //获取bdstoken
                var dom = browser.Document;
                string html = (dom.Body).OuterHtml;
                string str_context = GetStringIn(html, "var\\s+context\\s*\\=\\s*", ";[\r]?\n");
                JavaScriptSerializer jss = new JavaScriptSerializer();
                dynamic ret_obj = jss.DeserializeObject(str_context);
                var m_bdstoken = ret_obj["bdstoken"];
                iniFileHelper.WriteIniString("Baidu", "UserName", cookieStr);
                iniFileHelper.WriteIniString("Baidu", "Bdstoken", m_bdstoken);
                MessageBox.Show("保存成功");
                this.Close();
            }
            else
            {
                MessageBox.Show("请先登录");
                return;
            }
            




            //var host = new Uri(loadUrl).Host;
            //var cookies = CookieManager.GetCookiesFromHost(host);
            //string cookiesText = "";
            //while (cookies.MoveNext())
            //{
            //    var c = cookies.Current;
            //    cookiesText += c.Name + "=" + c.Value + ";";
            //}
            ////获取bdstoken
            //var dom = browser.Document;

            //string html = (dom.Body).OuterHtml;
            //string str_context = GetStringIn(html, "var\\s+context\\s*\\=\\s*", ";[\r]?\n");
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //dynamic ret_obj = jss.DeserializeObject(str_context);
            //var m_bdstoken = ret_obj["bdstoken"];
            //if (string.IsNullOrEmpty(m_bdstoken))
            //{
            //    MessageBox.Show("保存失败,请先登录");
            //    return;
            //}
            //iniFileHelper.WriteIniString("Baidu", "UserName", cookiesText);
            //iniFileHelper.WriteIniString("Baidu", "Bdstoken", m_bdstoken);

        }
        private string GetStringIn(
            string str_source,
            string str_start,
            string str_end,
            string str_find = ".+",
            RegexOptions options = RegexOptions.None)
        {
            Regex reg = new Regex("(?<=" + str_start + ")" +
                str_find +
                "(?=" + str_end + ")",
                options);
            if (!reg.IsMatch(str_source))
            {
                return "";
            }

            return reg.Match(str_source).Value;
        }
        //private string GetCookies(string url)
        //{
        //    var uri = new Uri(url);
        //    string host = uri.Host.Replace("www", "");
        //    var cookies = CookieManager.GetCookiesFromHost(host);
        //    string cookiesText = "";
        //    while (cookies.MoveNext())
        //    {
        //        var c = cookies.Current;
        //        cookiesText += c.Name + "=" + c.Value + ";";
        //    }
        //    return cookiesText;
        //}

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        private static string GetCookieString(string url)
        {
            // Determine the size of the cookie     
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);

            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;

                // Allocate stringbuilder large enough to hold the cookie     
                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }
    }
}
