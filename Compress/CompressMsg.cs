using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress
{
    public enum CompressStatus
    {
        Start,
        GetLength,
        Compress,
        End,
        Error
    }
    public class CompressMsg
    {
        private int _Id = 0;
        public int Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        private CompressStatus _Tag = 0;
        public CompressStatus Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                _Tag = value;
            }
        }

        private string _SizeInfo ="";
        public string SizeInfo
        {
            get
            {
                return _SizeInfo;
            }

            set
            {
                _SizeInfo = value;
            }
        }
        private double _Progress = 0;
        public double Progress
        {
            get
            {
                return _Progress;
            }

            set
            {
                _Progress = value;
            }
        }

        private string _Pwd = "";
        public string Pwd
        {
            get
            {
                return _Pwd;
            }

            set
            {
                _Pwd = value;
            }
        }

        private string _PwdFileUrl = "";
        public string PwdFileUrl
        {
            get
            {
                return _PwdFileUrl;
            }

            set
            {
                _PwdFileUrl = value;
            }
        }
        public int Price { get; set; }
        public string File { get; set; }

        public string Address { get; set; }
    }
}
