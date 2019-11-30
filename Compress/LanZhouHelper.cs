using Compress.UploadExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Compress
{
    public class LanZhouHelper
    {
        public static CookieContainer cookieContainer = null;
        IniFileHelper iniFileHelper = new IniFileHelper();
        private string userName = "", pwd = "";
        List<Task> tasks = new List<Task>();
        public LanZhouHelper()
        {
            if (cookieContainer == null)
            {
                StringBuilder sbUserName = new StringBuilder(60);
                iniFileHelper.GetIniString("LanZhou", "UserName", "", sbUserName, sbUserName.Capacity);
                userName = sbUserName.ToString();
                StringBuilder sbPwd = new StringBuilder(60);
                iniFileHelper.GetIniString("LanZhou", "Pwd", "", sbPwd, sbPwd.Capacity);
                pwd = sbPwd.ToString();
                CookieContainer cc = new CookieContainer();
                string postData = string.Format("action=login&task=login&ref=&formhash=002b2898&username={0}&password={1}", userName, pwd);
                string responseFromServer = HttpHelper.Post("https://up.woozooo.com/account.php", postData, cc);
                cookieContainer = cc;
            }
           
        }
        public string FileUpload(Stream stream, string fileName, string extension,int i)
        {
            if (cookieContainer == null)
                cookieContainer = GetCookie("https://up.woozooo.com/account.php", userName, pwd);
            IDictionary<string, string> postParameter = new Dictionary<string, string>();
            postParameter.Add("task", "1");
            postParameter.Add("folder_id", "-1");
            postParameter.Add("id", "WU_FILE_0");
            postParameter.Add("name", fileName);
            postParameter.Add("type", GetType(extension));
            postParameter.Add("lastModifiedDate", ToGMTFormat(DateTime.Now));
            postParameter.Add("size", stream.Length.ToString());
            string re = HttpHelper.Execute(new UploadParameterType
            {
                Url = "https://up.woozooo.com/fileup.php",
                UploadStream = stream,
                FileNameValue = fileName,
                PostParameters = postParameter
            }, cookieContainer);
            var lanZouFileResult = Newtonsoft.Json.JsonConvert.DeserializeObject<LanZouFileResult>(re);
            var file_id = lanZouFileResult.text[0].id;
            var result = HttpHelper.HttpPost<LanZouResult>("https://up.woozooo.com/doupload.php", "task=22&file_id=" + file_id, cookieContainer);
            string url = result.info.is_newd + "/" + result.info.f_id;
            string sharepwd = result.info.pwd;
            //设置密码
            HttpHelper.HttpPost<LanZouPwd>("https://up.woozooo.com/doupload.php", "task=23&file_id=" + file_id + "&shows=1&shownames=" + sharepwd, cookieContainer);
            return (url + "&" + sharepwd + "");
        }

        private  CookieContainer GetCookie(string url, string userName, string pwd)
        {
            CookieContainer cc = new CookieContainer();
            string postData = string.Format("action=login&task=login&formhash=55174221&username={0}&password={1}", userName, pwd);
            string responseFromServer = HttpHelper.Post(url, postData, cc);
            return cc;
        }

        private  string GetType(string extension)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() {
                { ".zip","application/x-zip-compressed"},
                { ".rar","application/octet-stream"},
                { ".txt","text/plain"},
                { ".7z","application/octet-stream"},
                { ".doc","application/msword"},
                { ".docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                { ".xls","application/vnd.ms-excel"},
                { ".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                { ".ppt","application/vnd.ms-powerpoint"},
                { ".pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                { ".exe","application/x-msdownload"},
                { ".pdf","application/pdf"}
            };
            return dic[extension];
        }
        /// <summary>  
        /// 本地时间转成GMT格式的时间  
        /// </summary>  
        private  string ToGMTFormat(DateTime dt)
        {
            return dt.ToString("r") + dt.ToString("zzz").Replace(":", "");
        }

        public void AddUpLoad(Stream stream, string fileName, string extension, int i, System.Windows.Forms.ListView listView)
        {
            string result = FileUpload(stream, fileName, extension, i);
            if (!string.IsNullOrEmpty(result))
            {
                listView.Items[i].SubItems[1].Text = result.Split('&')[0];
                listView.Items[i].SubItems[2].Text = result.Split('&')[1];
                listView.Items[i].SubItems[3].Text = "上传成功";
            }
        }

        
    }
}
