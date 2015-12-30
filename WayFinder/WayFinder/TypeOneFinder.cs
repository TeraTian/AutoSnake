using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayLib
{
    public class Type1Finder : WayFinder
    {
        public override void FindWay(ContainerDictionary dict, int x, int y)
        {
            var result = new List<IShowContainer>();
            //循环左、上、右、下
            for (int i = 0; i < 4; i++)
            {
                //如果已经出结果了就直接返回
                if (wayResult.Count > 0)
                    return;
                var curX = x + Direction.DirectionArray[i].X_;
                var curY = y + Direction.DirectionArray[i].Y_;
                //如果该坐标是终点
                if (curX == endX && curY == endY)
                {
                    wayResult = dict[x, y].Way;
                    return;
                }

                if (
                    curX < 0 || curX > dict.TotalX || curY < 0 || curY > dict.TotalY//坐标超出范围
                    || !dict[curX, curY].CanPass//该坐标无法通过
                    || dict[curX, curY].IsChecked//该坐标已经检查过了
                    )
                    continue;
                dict[curX, curY].IsChecked = true;//当前格子已经判断过了
                wayProcedure.Add(dict[curX, curY]);//添加入过程列表

                dict[curX, curY].Way = dict[x, y].Way;//设置当前格子路径，当前格子路径为其前一个格子的路径
                dict[curX, curY].Way.Add(new Coordinate { X_ = curX, Y_ = curY });//加上其自身
                //递归查找路径
                FindWay(dict, curX, curY);
            }
        }
    }
}
