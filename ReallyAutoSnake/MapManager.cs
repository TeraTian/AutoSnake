using ReallyAutoSnake.Container;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WayLib;

namespace ReallyAutoSnake
{
    public class MapManager
    {
        //总体思路，则只需要保证头能找到尾，蛇就不会死
        //首先是寻路功能，总是寻找到苹果的最近路径
        //如果有最近路径，则去判断走最近路径到苹果后，是否还能找到到尾巴的路径
        //如果没有到苹果的最近路径，则向离苹果最远的一格走一步，再作判断

        public ContainerDict dict;
        public Dictionary<string, Label> fillDict;
        private Snake snake;
        private long moveSpeed = 40;
        private bool LaterPeriod
        {
            get
            {
                return (snake.Body.Count() + 2) > 0.90 * dict.Count();
            }
        }

        public LabelContainer Apple { get; set; }
        private TextBox t { get; set; }
        private bool ttt { get; set; }
        public MapManager(ContainerDict dict, Dictionary<string, Label> fillDict, TextBox t)
        {
            this.dict = dict;
            this.fillDict = fillDict;
            this.t = t;
            this.ttt = false;
        }
        private WayFinder CreateNewFinder()
        {
            return new Type4Finder();
        }

        public bool Complete { get; set; }

        //存储每一个可多选格子的选项，尽可能每次选择不同路径，这样在后期不会陷入死循环
        //应当只有无法找到最佳路径，并且到后期才使用
        private Dictionary<Tuple<int, int>, List<MapHelper>> choiseDict = new Dictionary<Tuple<int, int>, List<MapHelper>>();

        /// <summary>
        /// 初始化蛇
        /// </summary>
        public void InitializeSnake()
        {
            dict[0, 0].SnakeType = SnakeTypeEnum.Tail;
            dict[0, 1].SnakeType = SnakeTypeEnum.Body;
            dict[0, 2].SnakeType = SnakeTypeEnum.Body;
            dict[0, 3].SnakeType = SnakeTypeEnum.Head;


            dict[0, 0].BackColor = Color.GreenYellow;
            dict[0, 1].BackColor = Color.GreenYellow;
            dict[0, 2].BackColor = Color.GreenYellow;
            dict[0, 3].BackColor = Color.GreenYellow;

            var temp = new List<LabelContainer>();
            temp.Enqueue(dict[0, 1]);
            temp.Enqueue(dict[0, 2]);
            snake = new Snake(dict[0, 3], dict[0, 0], temp, fillDict);
        }

        public async Task<string> Run()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            await RunToApple();
            sw.Stop();
            if (sw.Elapsed.Ticks / 10000 < moveSpeed)
                Thread.Sleep((int)(moveSpeed - sw.Elapsed.Ticks / 10000));
            return string.Empty;
        }

        /// <summary>
        /// 跑向苹果的策略1
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RunToApple()
        {
            return await TryFindWay();
        }


        /// <summary>
        /// 走向离苹果最远的格子一步
        /// </summary>
        public bool RunFaraway()
        {
            try
            {
                var head = snake.Head;
                LabelContainer label = null;
                //处理多选项
                var tuple = Tuple.Create(head.X, head.Y);
                List<MapHelper> choiseList;
                //if (choiseDict.ContainsKey(tuple))
                //{
                //    //由于一旦改变了某一个格子的前进方向，就会导致其后的所有选择都失效
                //    //所以必须清空其他选项，并保留当前选项
                //    choiseList = choiseDict[tuple];
                //}
                //else
                //{
                //    choiseList = ManagerMapHelperList();
                //    choiseDict.Add(tuple, choiseList);
                //}
                //if (choiseList.Count > 0)
                //{
                //    var nextCoord = choiseList[0];
                //    var next = dict[nextCoord.Coor.X_, nextCoord.Coor.Y_];

                //    while (true)
                //    {
                //        if (next.SnakeType == SnakeTypeEnum.Tail)
                //            break;
                //        if (!(next == null || next.SnakeType == SnakeTypeEnum.Body))
                //        {
                //            var containerDict = new ContainerDictionary(dict.TotalX, dict.TotalY);
                //            snake.FromNextHeadToTail(containerDict, dict[nextCoord.Coor.X_, nextCoord.Coor.Y_]);//判断走到下一格之后，是否有路找到尾
                //            var finder = CreateNewFinder();
                //            if (finder.Execute(containerDict) == "成功")
                //                break;
                //        }
                //        choiseList.RemoveAt(0);
                //        choiseList.Add(nextCoord);
                //        nextCoord = choiseList[0];
                //        next = dict[nextCoord.Coor.X_, nextCoord.Coor.Y_];
                //    }
                //    choiseList.RemoveAt(0);
                //    choiseList.Add(nextCoord);
                //    label = dict[nextCoord.Coor.X_, nextCoord.Coor.Y_];
                //}
                //choiseList = ManagerMapHelperList();
                //if (choiseList.Count > 0)
                //    label = dict[choiseList[0].Coor.X_, choiseList[0].Coor.Y_];
                var choise = ManagerMapHelper();
                if (choise != null)
                    label = dict[choise.Coor.X_, choise.Coor.Y_];
                //如果能找到这样一个格子，就走一步，如果这样的格子找不到了，就说明游戏结束
                if (label != null)
                {
                    if (label.SnakeType == SnakeTypeEnum.Apple)
                    {
                        this.Apple = null;
                    }
                    snake.MoveOneStep(label);
                    return true;
                }
            }
            catch (Exception e)
            {
            }
            return false;
        }
        private MapHelper ManagerMapHelper()
        {
            MapHelper choise = null;
            var head = snake.Head;
            var distance = 0;
            foreach (var dir in Direction.DirectionArray)
            {
                var curX = head.X + dir.X_;
                var curY = head.Y + dir.Y_;
                if (dict[curX, curY] != null &&
                        (
                        dict[curX, curY].SnakeType == SnakeTypeEnum.None
                        || dict[curX, curY].SnakeType == SnakeTypeEnum.Tail
                        || dict[curX, curY].SnakeType == SnakeTypeEnum.Apple
                        )
                    )//是空格子
                {
                    var containerDict = new ContainerDictionary(dict.TotalX, dict.TotalY);
                    snake.FromNextHeadToTail(containerDict, dict[curX, curY]);//判断走到下一格之后，是否有路找到尾
                    var finder = CreateNewFinder();
                    if (finder.Execute(containerDict) == "成功")
                    {
                        //判断格子到苹果的距离    
                        var dis = 0;
                        //if (this.LaterPeriod)
                        {
                            if (Math.Abs(snake.Tail.X - curX) > Math.Abs(snake.Tail.Y - curY))
                            {
                                dis = (Math.Abs(snake.Tail.X - curX) * 3 + Math.Abs(snake.Tail.Y - curY)) * 2;
                            }
                            else
                                dis = (Math.Abs(snake.Tail.X - curX) + Math.Abs(snake.Tail.Y - curY) * 3) * 2;
                        }
                        //else
                        //{
                        //    if (Math.Abs(this.Apple.X - curX) > Math.Abs(this.Apple.Y - curY))
                        //    {
                        //        dis = (Math.Abs(this.Apple.X - curX) * 3 + Math.Abs(this.Apple.Y - curY)) * 2;
                        //    }
                        //    else
                        //        dis = (Math.Abs(this.Apple.X - curX) + Math.Abs(this.Apple.Y - curY) * 3) * 2;
                        //}
                        //dis += ((Math.Abs(snake.Tail.X - curX) + Math.Abs(snake.Tail.Y - curY)) * 2);
                        //判断格子四周是边界的个数（身体或者边界）
                        var con = 10;
                        foreach (var dirr in Direction.DirectionArray)
                        {
                            var nX = curX + dirr.X_;
                            var nY = curY + dirr.Y_;
                            var nDict = dict[nX, nY];
                            if (nDict == null)
                                con++;
                            else if (nDict.SnakeType == SnakeTypeEnum.Body)
                                con += 2;
                        }
                        //判断蛇方向
                        var di = 0;
                        var nextX = head.X - snake.Body.Last().X;
                        var nextY = head.Y - snake.Body.Last().Y;
                        if (curX == head.X + nextX && curY == head.Y + nextY)
                            di = 2;



                        if (dict[curX, curY].SnakeType == SnakeTypeEnum.Apple)
                        {
                            //dis += 1000;
                        }
                        //添加多选项
                        if (dis + con + di > distance)
                        {
                            distance = dis + con + di;
                            choise = new MapHelper()
                            {
                                Coor = new Coordinate()
                                {
                                    X_ = curX,
                                    Y_ = curY
                                }
                            };
                        }
                    }
                }
            }
            return choise;
        }
        private List<MapHelper> ManagerMapHelperList()
        {
            var choiseList = new List<MapHelper>();
            var head = snake.Head;
            foreach (var dir in Direction.DirectionArray)
            {
                var curX = head.X + dir.X_;
                var curY = head.Y + dir.Y_;
                if (dict[curX, curY] != null &&
                        (
                        dict[curX, curY].SnakeType == SnakeTypeEnum.None
                        || dict[curX, curY].SnakeType == SnakeTypeEnum.Tail
                        || dict[curX, curY].SnakeType == SnakeTypeEnum.Apple
                        )
                    )//是空格子
                {
                    var containerDict = new ContainerDictionary(dict.TotalX, dict.TotalY);
                    snake.FromNextHeadToTail(containerDict, dict[curX, curY]);//判断走到下一格之后，是否有路找到尾
                    var finder = CreateNewFinder();
                    if (finder.Execute(containerDict) == "成功")
                    {
                        //判断格子到苹果的距离    
                        var dis = 0;
                        if (this.LaterPeriod)
                        {
                            if (Math.Abs(snake.Tail.X - curX) > Math.Abs(snake.Tail.Y - curY))
                            {
                                dis = (Math.Abs(snake.Tail.X - curX) * 3 + Math.Abs(snake.Tail.Y - curY)) * 2;
                            }
                            else
                                dis = (Math.Abs(snake.Tail.X - curX) + Math.Abs(snake.Tail.Y - curY) * 3) * 2;
                        }
                        else
                        {
                            if (Math.Abs(this.Apple.X - curX) > Math.Abs(this.Apple.Y - curY))
                            {
                                dis = (Math.Abs(this.Apple.X - curX) * 3 + Math.Abs(this.Apple.Y - curY)) * 2;
                            }
                            else
                                dis = (Math.Abs(this.Apple.X - curX) + Math.Abs(this.Apple.Y - curY) * 3) * 2;
                        }
                        //dis += ((Math.Abs(snake.Tail.X - curX) + Math.Abs(snake.Tail.Y - curY)) * 2);
                        //判断格子四周是边界的个数（身体或者边界）
                        var con = 10;
                        foreach (var dirr in Direction.DirectionArray)
                        {
                            var nX = curX + dirr.X_;
                            var nY = curY + dirr.Y_;
                            var nDict = dict[nX, nY];
                            if (nDict == null)
                                con++;
                            else if (nDict.SnakeType == SnakeTypeEnum.Body)
                                con += 2;
                        }
                        //判断蛇方向
                        var di = 0;
                        var nextX = head.X - snake.Body.Last().X;
                        var nextY = head.Y - snake.Body.Last().Y;
                        if (curX == head.X + nextX && curY == head.Y + nextY)
                            di = 2;

                        //添加多选项
                        choiseList.Add(new MapHelper()
                        {
                            Coor = new Coordinate() { X_ = curX, Y_ = curY },
                            Weight = dis + con + di
                        });
                    }
                }
            }
            choiseList = choiseList.OrderByDescending(i => i.Weight).ToList();
            if (choiseList.Count > 1)
                choiseList.RemoveRange(1, choiseList.Count - 1);
            return choiseList;
        }

        /// <summary>
        /// 吃到苹果后还能否找到到尾巴的路
        /// </summary>
        private async Task<bool> TryFindWay()
        {
            if (this.LaterPeriod)
                return RunFaraway();
            if (!(await TryFindWayFromHeadToAppleToTail() || await TryFindWayFromTailToAppleToHead()))
                return RunFaraway();
            return true;
        }

        /// <summary>
        /// 用于choiseDict的帮助累
        /// </summary>
        private class MapHelper
        {
            /// <summary>
            /// 坐标
            /// </summary>
            public Coordinate Coor { get; set; }
            /// <summary>
            /// 权重值
            /// </summary>
            public int Weight { get; set; }
        }

        #region 寻找最佳路径
        /// <summary>
        /// 头=》苹果=》尾
        /// </summary>
        public async Task<bool> TryFindWayFromHeadToAppleToTail()
        {
            //头=》苹果=》尾
            var con = new ContainerDictionary(dict.TotalX, dict.TotalY);
            snake.FromHeadToApple(con, Apple);
            var finder = CreateNewFinder();
            var str = finder.Execute(con);
            if (str == "成功")
            {
                var road = finder.WayResult;
                var con2 = new ContainerDictionary(dict.TotalX, dict.TotalY);
                snake.FromAppleToTail(con2, this.Apple, road);
                finder = CreateNewFinder();
                str = finder.Execute(con2);
                if (str == "成功")
                {
                    await Task.Run(() =>
                    {
                        road.Add(new Coordinate { X_ = con.EndContainer.X, Y_ = con.EndContainer.Y });
                        snake.MoveOneStep(dict[road[0].X_, road[0].Y_]);
                    });
                    //苹果被吃掉了
                    if (road.Count == 1)
                    {
                        choiseDict = new Dictionary<Tuple<int, int>, List<MapHelper>>();
                        this.Apple = null;
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 尾=》苹果=》头
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TryFindWayFromTailToAppleToHead()
        {
            var con = new ContainerDictionary(dict.TotalX, dict.TotalY);
            snake.FromTailToApple(con, Apple);
            var finder = CreateNewFinder();
            var str = finder.Execute(con);
            if (str == "成功")
            {
                var road = finder.WayResult;
                var con2 = new ContainerDictionary(dict.TotalX, dict.TotalY);
                snake.FromAppleToHead(con2, this.Apple, road);
                finder = CreateNewFinder();
                str = finder.Execute(con2);
                if (str == "成功")
                {
                    road = finder.WayResult;
                    await Task.Run(() =>
                    {
                        road.Add(new Coordinate { X_ = con.EndContainer.X, Y_ = con.EndContainer.Y });
                        snake.MoveOneStep(dict[road[0].X_, road[0].Y_]);
                    });
                    //苹果被吃掉了
                    if (road.Count == 1)
                    {
                        choiseDict = new Dictionary<Tuple<int, int>, List<MapHelper>>();
                        this.Apple = null;
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 创建苹果
        public void CreateApple()
        {
            if (this.Apple == null)
            {
                var free = dict.FreeLabel();
                //如果没有空格子，说明游戏结束
                if (free.Count == 0)
                {
                    Complete = true;
                    return;
                }
                if (this.LaterPeriod)
                {
                    var dis = 0;
                    int appleX = 0;
                    int appleY = 0;
                    foreach (var f in free)
                    {
                        var d = Math.Abs(snake.Head.X - f.X) + Math.Abs(snake.Head.Y - f.Y);
                        if (d > dis)
                        {
                            dis = d;
                            appleX = f.X;
                            appleY = f.Y;
                        }
                    }
                    this.Apple = dict[appleX, appleY];
                    this.Apple.BackColor = Color.Red;
                    this.Apple.SnakeType = SnakeTypeEnum.Apple;
                }
                else
                {
                    var rnd = new Random();
                    var appleIndex = rnd.Next(0, free.Count);
                    this.Apple = free[appleIndex];
                    this.Apple.BackColor = Color.Red;
                    this.Apple.SnakeType = SnakeTypeEnum.Apple;
                }
            }
        }
        #endregion

        #region 测试用代码
        public List<LabelContainer> detailList;
        public void ShowDetail()
        {
            detailList = new List<LabelContainer>();
            var finder = CreateNewFinder();
            if (finder.WayProcedure != null)
            {
                ThreadPool.QueueUserWorkItem(o =>
                    {
                        foreach (var w in finder.WayProcedure)
                        {
                            detailList.Add(dict[w.X, w.Y]);
                            dict[w.X, w.Y].BackColor = Color.Yellow;
                            Thread.Sleep(20);
                        }
                    });
            }
        }
        public void ClearDetail()
        {
            foreach (var l in detailList)
            {
                l.BackColor = Color.White;
            }
        }
        public void OneStepBack()
        {
            snake.BackOneStep();
        }
        #endregion
    }
}
