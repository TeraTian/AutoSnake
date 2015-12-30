using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WayLib
{
    public class TypeFiveFinder : WayFinder
    {

        private List<IShowContainer> CurrentList = new List<IShowContainer>();
        private List<IShowContainer> NextList = new List<IShowContainer>();

        public override void FindWay(ContainerDictionary dict, int x, int y)
        {
        }

        private void FindWayType2_(ContainerDictionary dict)
        {
            foreach (var con in CurrentList)
            {
                var x = con.X;
                var y = con.Y;
                for (int i = 0; i < 4; i++)
                {
                    var curX = x + Direction.DirectionArray[i].X_;
                    var curY = y + Direction.DirectionArray[i].Y_;
                    //如果是终点
                    if (curX == endX && curY == endY)
                    {
                        wayResult = dict[x, y].Way;
                        CurrentList.Clear();
                        return;
                    }
                    //如果超出范围
                    if (
                    curX < 0 || curX > dict.TotalX || curY < 0 || curY > dict.TotalY//坐标超出范围
                    || !dict[curX, curY].CanPass//该坐标无法通过
                    || dict[curX, curY].IsChecked//该坐标已经检查过了
                    )
                        continue;
                    //添加入过程列表
                    wayProcedure.Add(dict[curX, curY]);
                    dict[curX, curY].StartDistance = dict[x, y].StartDistance + 1;//设置距离
                    dict[curX, curY].IsChecked = true;

                    dict[curX, curY].Way = Extension.Clone(dict[x, y].Way);//设置当前格子路径，当前格子路径为其前一个格子的路径
                    dict[curX, curY].Way.Add(new Coordinate { X_ = curX, Y_ = curY });//加上其自身

                    NextList.Add(dict[curX, curY]);
                }
            }
            CurrentList = NextList;
            NextList = new List<IShowContainer>();
        }

    }
}
