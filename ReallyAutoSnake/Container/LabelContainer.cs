using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WayLib;

namespace ReallyAutoSnake.Container
{
    public class LabelContainer : System.Windows.Forms.Label
    {
        public int X { get; set; }
        public int Y { get; set; }
        /// <summary>
        /// 格子若为蛇，代表蛇的类型，头1，身2，尾3，无0，苹果4
        /// </summary>
        public SnakeTypeEnum SnakeType
        {
            get;
            set;
        }
    }
    public enum SnakeTypeEnum
    {
        Head = 1, Body = 2, Tail = 3, None = 0, Apple = 4
    }
}
