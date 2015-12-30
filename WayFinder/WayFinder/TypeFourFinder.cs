using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayLib
{
    public class Type4Finder:WayFinder
    {
        private List<IShowContainer> EndMinPool = new List<IShowContainer>();//存放到终点最小的集合
        private List<IShowContainer> ContainerPool = new List<IShowContainer>();//存放所有的集合

        public override void FindWay(ContainerDictionary dict, int x, int y)
        {
            dict[x, y].StartDistance = 0;
            EndMinPool.Add(dict[x, y]);
            while (EndMinPool.Count > 0)
                FindWayType4_(dict);
        }

        private void FindWayType4_(ContainerDictionary dict)
        {
            foreach (var con in EndMinPool)
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
                        EndMinPool.Clear();
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
                    dict[curX, curY].EndDistance = Math.Abs(endX - curX) + Math.Abs(endY - curY);
                    dict[curX, curY].IsChecked = true;

                    dict[curX, curY].Way = Extension.Clone(dict[x, y].Way);//设置当前格子路径，当前格子路径为其前一个格子的路径
                    dict[curX, curY].Way.Add(new Coordinate { X_ = curX, Y_ = curY });//加上其自身

                    ContainerPool.Add(dict[curX, curY]);
                }
            }
            if (ContainerPool.Count > 0)
            {
                //将所有的池中的EndDistance的元素选出，并将其从池中删除
                var minDistane = ContainerPool.Min(c => c.EndDistance + c.StartDistance);
                EndMinPool = ContainerPool.Where(c => c.EndDistance + c.StartDistance == minDistane).ToList();
                EndMinPool.ForEach(e => ContainerPool.Remove(e));
            }
            else
                EndMinPool.Clear();
        }
    }
}
