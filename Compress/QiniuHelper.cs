using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress
{
    public class QiniuHelper
    {
        CompressMsg compressMsg = null;
        public QiniuHelper(CompressMsg compressMsg)
        {
            this.compressMsg = compressMsg;
        }

        public delegate void dlgSendMsg(CompressMsg msg);
        public event dlgSendMsg doSendMsg;

        private static string imgUrRL = "http://q0yez9uk6.bkt.clouddn.com/";
        IniFileHelper iniFileHelper = new IniFileHelper();
        public string Upload(Stream stream,string fileName)
        {
            StringBuilder sbAK = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "AK", "", sbAK, sbAK.Capacity);
            StringBuilder sbSK = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "SK", "", sbSK, sbSK.Capacity);

            Mac mac = new Mac(sbAK.ToString(), sbSK.ToString());
            Auth auth = new Auth(mac);

            StringBuilder sbZone = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "Zone", "", sbZone, sbZone.Capacity);
            // 目标空间名
            string bucket = sbZone.ToString();
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = bucket;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 文件上传完毕后，1天后自动删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证
            string uploadToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            StringBuilder sbBucket = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "Bucket", "", sbBucket, sbBucket.Capacity);
            Zone zone = Zone.ZONE_CN_East;
            switch (sbBucket.ToString())
            {
                case "ZONE_CN_East":
                    zone = Zone.ZONE_CN_East;
                    break;
                case "ZONE_CN_North":
                    zone = Zone.ZONE_CN_North;
                    break;
                case "ZONE_CN_South":
                    zone = Zone.ZONE_CN_South;
                    break;
                case "ZONE_US_North":
                    zone = Zone.ZONE_US_North;
                    break;
                default:
                    zone = Zone.ZONE_AS_Singapore;
                    break;
            }
            Config config = new Config()
            {
                //// 空间对应的机房
                Zone = zone,
                // 是否使用https域名
                UseHttps = true,
                // 上传是否使用cdn加速
                UseCdnDomains = true
            };
            FormUploader upload = new FormUploader(config);
            HttpResult result = upload.UploadStream(stream, fileName+".jpg", uploadToken, null);
            StringBuilder sbDomain = new StringBuilder(60);
            iniFileHelper.GetIniString("qiniu", "Domain", "", sbDomain, sbDomain.Capacity);
            imgUrRL = sbDomain.Length > 0 ? sbDomain.ToString() : imgUrRL;
            compressMsg.PwdFileUrl = imgUrRL + fileName + ".jpg";
            doSendMsg(compressMsg);
            if (result.Code == 200)
            {
                return "Ok";
            }
            return "fall";
        }
    }
}
