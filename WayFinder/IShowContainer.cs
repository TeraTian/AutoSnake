using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayLib
{
    public interface IShowContainer
    {
        int ID { get; set; }
        /// <summary>
        /// X坐标
        /// </summary>
        int X { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        int Y { get; set; }
        /// <summary>
        /// 该格子到起点的距离
        /// </summary>
        int StartDistance { get; set; }
        /// <summary>
        /// 该格子到终点的距离
        /// </summary>
        int EndDistance { get; set; }
        /// <summary>
        /// 通过该格子时的权重，用来表达通过的难易程度
        /// </summary>
        int Weight { get; set; }
        /// <summary>
        /// 是否允许通过
        /// </summary>
        bool CanPass { get; set; }
        /// <summary>
        /// 显示的颜色
        /// </summary>
        Color ContainerColor { get; set; }
        /// <summary>
        /// 格子的类型
        /// </summary>
        ContainerType Type { get; set; }
        /// <summary>
        /// 是否已经被检测过
        /// </summary>
        bool IsChecked { get; set; }
        /// <summary>
        /// 从起点到达该格子的路径
        /// </summary>
        List<Coordinate> Way { get; set; }
    }
}
