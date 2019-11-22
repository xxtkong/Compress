using Compress.BaiduExt;
using Gecko;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Compress
{
    
    public partial class BaiduUpLoad : Form
    {
        public BaiduUpLoad()
        {
            InitializeComponent();
        }
        IniFileHelper iniFileHelper = new IniFileHelper();
        //string panUrl = "https://pan.baidu.com/api/rapidupload?rtype=1&channel=chunlei&web=1&app_id=250528&bdstoken=85fbb50fc8eb5164f3dea9d1247b6b2e&logid=MTU3NDE1MzU1NzQ0MjAuMjYxNzg3NTIyODQ5MDc3OA==&clienttype=0"
        string BAIDU_PCS_REST = "https://pan.baidu.com/api/rapidupload";
        const long RAPIDUPLOAD_THRESHOLD = 256 * 1024;
        const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.154 Safari/537.36 LBBROWSER";
        const int LOCAL_READ_BUF_SIZE = 1024 * 1024;
        const int NET_READ_BUF_SIZE = 1024;
        private BaiduProgressInfo m_pi = null;
        private HttpStatusCode m_last_http_code = HttpStatusCode.OK;
        private int m_status = 0;
        public int Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        private void BaiduUpLoad_Load(object sender, EventArgs e)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("rtype", "1");
            nvc.Add("channel", "chunlei");
            nvc.Add("web", "1");
            nvc.Add("app_id", "250528");
            StringBuilder bdstoken = new StringBuilder(5000);
            iniFileHelper.GetIniString("Baidu", "Bdstoken", "", bdstoken, bdstoken.Capacity);
            nvc.Add("bdstoken", bdstoken.ToString());
            nvc.Add("logid", GetUSTimeStamp());
            nvc.Add("clienttype", "0");
            string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));
            string url = BAIDU_PCS_REST + "?" + str_params;

            StringBuilder cookie = new StringBuilder(5000);
            iniFileHelper.GetIniString("Baidu", "UserName", "", cookie, cookie.Capacity);

            var fs = File.OpenRead("D:\\source1\\netty-4-user-guidetemp.zip");
            IDictionary<string, string> postParameter = new Dictionary<string, string>();
            postParameter.Add("path", "/netty-4-user-guidetemp.zip");
            postParameter.Add("content-length", fs.Length.ToString());
            postParameter.Add("content-md5", MD5File(fs).ToLower());
            postParameter.Add("slice-md5", MD5File(fs, 0, RAPIDUPLOAD_THRESHOLD).ToLower());
            postParameter.Add("target_path","/");
            postParameter.Add("local_mtime", GetTimeStamp());

            string re = HttpHelper.BaiduExecute(new UploadParameterType
            {
                Url = url,
                UploadStream = fs,
                FileNameValue = "netty-4-user-guidetemp.zip",
                PostParameters = postParameter
            }, GetCookieFromString(cookie.ToString()));

           










            //string str_boundary = string.Format("----------{0}", DateTime.Now.Ticks.ToString("x"));
            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            //req.Proxy = null;
            //req.Timeout = -1;
            //req.UserAgent = USER_AGENT;
            //req.Method = WebRequestMethods.Http.Post;
            //req.CookieContainer = GetCookieFromString(cookie.ToString());
            //req.KeepAlive = false;
            //req.ContentType = "multipart/form-data; boundary=" + str_boundary;
            //req.ServicePoint.Expect100Continue = false;
            ////req.ServicePoint.UseNagleAlgorithm = false;
            ////req.AllowWriteStreamBuffering = false;
            ////req.SendChunked = true;

            //byte[] bytes_disposition =
            //    Encoding.UTF8.GetBytes(string.Format(
            //        "--{0}\r\n" +
            //        "Content-Disposition: form-data; name=\"file\"; " +
            //        "filename=\"{1}\"\r\n" +
            //        "Content-Type: application/octet-stream\r\n\r\n",
            //        str_boundary,
            //        HttpUtility.UrlEncode(GetDirFileName("D:\\source1\\重庆周工作计划temp.zip"))));

            //byte[] bytes_footer = Encoding.UTF8.GetBytes("\r\n--" + str_boundary + "--\r\n");
            ////req.ContentLength = bytes_disposition.Length + fs.Length + bytes_footer.Length;
            //Stream s = req.GetRequestStream();
            //s.Write(bytes_disposition, 0, bytes_disposition.Length);
            //m_pi.current_bytes = 0;
            //m_pi.total_bytes = fs.Length;
            //while (m_pi.current_bytes < m_pi.total_bytes)
            //{
            //    if (1 == m_status)
            //    {
            //        while (1 == m_status)
            //        {
            //            Application.DoEvents();
            //            Thread.Sleep(50);
            //        }
            //    }
            //    else if (2 == m_status)
            //    {
            //        return ;
            //    }

            //    long left_bytes = m_pi.total_bytes - m_pi.current_bytes;
            //    long buf_size = (left_bytes > LOCAL_READ_BUF_SIZE ? LOCAL_READ_BUF_SIZE : left_bytes);
            //    byte[] bytes_buf = new byte[buf_size];
            //    int cur_read_len = fs.Read(bytes_buf, 0, bytes_buf.Length);
            //    if (0 == cur_read_len) break;
            //    s.Write(bytes_buf, 0, cur_read_len);
            //    m_pi.current_bytes += cur_read_len;
            //    m_pi.current_size += cur_read_len;
            //}
            //s.Write(bytes_footer, 0, bytes_footer.Length);
            //s.Close();
            //req.BeginGetResponse((IAsyncResult ar) =>
            //{
            //    HttpWebRequest wq = ar.AsyncState as HttpWebRequest;
            //    if (null == wq) return;
            //    HttpWebResponse res = wq.EndGetResponse(ar) as HttpWebResponse;
            //    if (null == res) return;

            //    m_last_http_code = res.StatusCode;
            //    if (HttpStatusCode.OK != m_last_http_code)
            //    {
            //        return;
            //    }

            //    BinaryReader br = new BinaryReader(res.GetResponseStream());
            //    byte[] html = new byte[NET_READ_BUF_SIZE * 2];
            //    int offset = 0, read_len = 0;
            //    do
            //    {
            //        offset += read_len;
            //        if (offset + NET_READ_BUF_SIZE > html.Length)
            //        {
            //            Array.Resize(ref html, html.Length * 2);
            //        }

            //        read_len = br.Read(html, offset, NET_READ_BUF_SIZE);
            //    } while (0 != read_len);

            //    offset += read_len;
            //    Array.Resize(ref html, offset);



            //    br.Close(); res.Close();

            //    string str_html = Encoding.UTF8.GetString(html);
            //    object error_code = GetJsonValue(str_html, "error_code");
            //    if (null != error_code)
            //    {
            //        object error_msg = GetJsonValue(str_html, "error_msg");

            //    }
            //}, req);
        }

        public object GetJsonValue(string json, string path)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> d = jss.DeserializeObject(json) as Dictionary<string, object>;
            if (null == d) return null;

            string[] arr_path = path.Split(".".ToCharArray());
            for (int i = 0; i < arr_path.Length; i++)
            {
                if (d.ContainsKey(arr_path[i]))
                {
                    if (i == arr_path.Length - 1)
                    {
                        return d[arr_path[i]];
                    }
                    else if (d[arr_path[i]] is Dictionary<string, object>)
                    {
                        d = d[arr_path[i]] as Dictionary<string, object>;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public string GetDirFileName(string dir_path)
        {
            if ("/" == dir_path) return "/";

            string[] arr_path = dir_path.Split("/".ToCharArray());
            return arr_path[arr_path.Length - 1];
        }
        public CookieContainer GetCookieFromString(string cookieStr)
        {
            //string pattern = "((?<name>.+?)=(?<value>.+?)[:,])+?Domain=(?<domain>.+?),Path=(?<path>.+?);?";
            //var cookies = new CookieCollection();
            //Regex.Matches(str, pattern).Cast<Match>().SelectMany(m =>
            //    Enumerable.Range(0, m.Groups[1].Captures.Count).Select(i =>
            //        new System.Net.Cookie(m.Groups["name"].Captures[i].Value,
            //                m.Groups["value"].Captures[i].Value,
            //                m.Groups["path"].Value,
            //                m.Groups["domain"].Value)))
            //    .ToList()
            //    .ForEach(c => cookies.Add(c));
            //return cookies;




            CookieContainer myCookieContainer = new CookieContainer();
            string[] cookstr = cookieStr.Split(';');
            foreach (string str in cookstr)
            {
                //string[] cookieNameValue = str.Split('=');
                //System.Net.Cookie ck = new System.Net.Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                //ck.Domain = "baidu.com";//必须写对
                //myCookieContainer.Add(ck);

                string[] cookieNameValue = str.Split('=');
                System.Net.Cookie ck = new System.Net.Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString().Split(',')[0]);
                ck.Domain = "pan.baidu.com";
                myCookieContainer.Add(ck);
            }
            return myCookieContainer;








            //var uri = new Uri("https://pan.baidu.com/");
            //System.Net.CookieContainer cc = new System.Net.CookieContainer();
            //foreach (var str in cookieStr.Split(';'))
            //    cc.SetCookies(uri, str);
            //var cookies = cc.GetCookies(uri);
            //cc.Add(new System.Net.Cookie("a", "1") { Domain = "baidu.com" });
            //return cc;
        }

        /// <summary>
        /// 获取当前 UNIX_TIMESTAMP * 1000
        /// </summary>
        /// <returns></returns>
        static public string GetTimeStamp()
        {
            return Math.Floor((DateTime.Now - BASE_TIME).TotalSeconds).ToString();
            //return ((long)(DateTime.Now - BASE_TIME).TotalSeconds * 1000 & 0x7FFFFFFF).ToString();
        }
        private byte[] BuildKeyValueParams(NameValueCollection nvc, bool need_url_encode = true)
        {
            string str_ret = "";
            foreach (string key in nvc.AllKeys)
            {
                str_ret += key + "=" + (need_url_encode ? HttpUtility.UrlEncode(nvc[key]) : nvc[key]);
                str_ret += "&";
            }

            str_ret = str_ret.TrimEnd("&".ToCharArray());
            return Encoding.UTF8.GetBytes(str_ret);
        }
        private static DateTime BASE_TIME = new DateTime(1970, 1, 1);
        public string GetUSTimeStamp()
        {
            string str_time_stamp = ((DateTime.Now - BASE_TIME).Ticks / (double)TimeSpan.TicksPerMillisecond * 10).ToString("F1") +
                new Random().Next(99999999).ToString("D8") +
                new Random().Next(99999999).ToString("D8");
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str_time_stamp));
        }
        private string MD5File(FileStream fs, long offset = 0, long length = 0)
        {
            const int SLICE_SIZE = 16 * 1024;

            try
            {
                fs.Seek(offset, SeekOrigin.Begin);
                if (0 == length)
                {
                    length = fs.Length;
                }

                long read_size = 0;
                MD5 md5 = MD5CryptoServiceProvider.Create();
                byte[] bytes_slice = null, bytes_out = null;
                do
                {
                    int slice_size = (int)(length - read_size);
                    if (slice_size > SLICE_SIZE)
                    {
                        slice_size = SLICE_SIZE;
                    }

                    bytes_slice = new byte[slice_size];
                    read_size += fs.Read(bytes_slice, 0, slice_size);

                    if (read_size < length)
                    {
                        bytes_out = new byte[slice_size];
                        md5.TransformBlock(bytes_slice, 0, slice_size, bytes_out, 0);
                    }
                    else
                    {
                        bytes_out = md5.TransformFinalBlock(bytes_slice, 0, slice_size);
                    }
                } while (read_size < length);

                string str_ret = BitConverter.ToString(md5.Hash).Replace("-", "");
                return str_ret;
            }
            catch (Exception ex)
            {
                
                return "";
            }
        }
    }

}
