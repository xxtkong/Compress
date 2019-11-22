using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Compress.BaiduExt
{
    public class Win32Mouse
    {
        //鼠标移动
        //dX和dy保留移动的信息。给出的信息是绝对或相对整数值。
        public static readonly int MOUSEEVENTF_MOVE = 0x0001;
        //绝对位置
        //则dX和dy含有标准化的绝对坐标，其值在0到65535之间。
        //事件程序将此坐标映射到显示表面。
        //坐标（0，0）映射到显示表面的左上角，（65535，65535）映射到右下角
        //如果没指定MOUSEEVENTF_ABSOLUTE，dX和dy表示相对于上次鼠标事件产生的位置（即上次报告的位置）的移动。
        //正值表示鼠标向右（或下）移动；负值表示鼠标向左（或上）移动。
        public static readonly int MOUSEEVENTF_ABSOLUTE = 0x8000;
        public static readonly int MOUSEEVENTF_LEFTDOWN = 0x0002;//左键按下
        public static readonly int MOUSEEVENTF_LEFTUP = 0x0004;//左键抬起
        public static readonly int MOUSEEVENTF_RIGHTDOWN = 0x0008; //右键按下 
        public static readonly int MOUSEEVENTF_RIGHTUP = 0x0010; //右键抬起 
        public static readonly int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //中键按下 
        public static readonly int MOUSEEVENTF_MIDDLEUP = 0x0040;// 中键抬起 

        /// <summary>
        /// 得到光标的位置
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pt);

        /// <summary>
        /// 移动光标的位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);

        /// <summary>
        /// 第2个参数应该是为，
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx">DX=需要移动的X*65536/取屏幕宽度 ()+1</param>
        /// <param name="dy">DY=需要移动的Y*65536/取屏幕高度 ()+1</param>
        /// <param name="cButtons"></param>
        /// <param name="dwExtraInfo"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    }
}
