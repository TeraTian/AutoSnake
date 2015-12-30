using ReallyAutoSnake.Container;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WayLib;

namespace ReallyAutoSnake
{
    public class Snake
    {
        public LabelContainer Head { get; set; }
        public List<LabelContainer> Body { get; set; }
        public LabelContainer Tail { get; set; }

        public LabelContainer LastTail { get; set; }
        public List<LabelContainer> LastBody { get; set; }
        public LabelContainer LastHead { get; set; }

        public Color snakeColor = Color.Black;
        public Color noneColor = Color.White;
        public Color TailColor = Color.Blue;
        public Color HeadColor = Color.Green;

        private int moveSpeed = 30;

        private Dictionary<string, Label> fillDict;

        public Snake(LabelContainer head, LabelContainer tail, List<LabelContainer> body, Dictionary<string, Label> fillDict)
        {
            this.Head = head;
            this.Tail = tail;
            this.Body = body;
            this.fillDict = fillDict;
        }

        public void MoveOneStep(LabelContainer next)
        {
            //Thread.Sleep(moveSpeed);
            LastTail = Tail;
            LastHead = Head;
            LastBody = Body;

            if (next.SnakeType != SnakeTypeEnum.Apple)
            {
                Tail.SnakeType = SnakeTypeEnum.None;
                Tail.BackColor = noneColor;
                Tail = Body.Dequeue();

                var key = string.Format("{0}{1}{2}{3}",
                    Tail.X > LastTail.X ? LastTail.X : Tail.X,
                    Tail.Y > LastTail.Y ? LastTail.Y : Tail.Y,
                    Tail.X > LastTail.X ? Tail.X : LastTail.X,
                    Tail.Y > LastTail.Y ? Tail.Y : LastTail.Y);
                var fill = fillDict[key];
                fill.BackColor = noneColor;

                Tail.BackColor = TailColor;
                Tail.SnakeType = SnakeTypeEnum.Tail;

            }

            Head.SnakeType = SnakeTypeEnum.Body;
            Body.Enqueue(Head);
            Head.BackColor = snakeColor;
            Head = next;
            Head.SnakeType = SnakeTypeEnum.Head;
            Head.BackColor = HeadColor;

            var key2 = string.Format("{0}{1}{2}{3}",
                    Head.X > LastHead.X ? LastHead.X : Head.X,
                    Head.Y > LastHead.Y ? LastHead.Y : Head.Y,
                    Head.X > LastHead.X ? Head.X : LastHead.X,
                    Head.Y > LastHead.Y ? Head.Y : LastHead.Y);
            var fill2 = fillDict[key2];
            fill2.BackColor = snakeColor;
        }

        public void BackOneStep()
        {
            Head.SnakeType = SnakeTypeEnum.None;
            Head.BackColor = noneColor;
            Head = LastHead;

            Body = LastBody;

            Tail = LastTail;
            Tail.SnakeType = SnakeTypeEnum.Tail;
            Tail.BackColor = snakeColor;

        }


        public void Move(List<Coordinate> road, ContainerDict dict)
        {
            foreach (var r in road)
            {
                MoveOneStep(dict[r.X_, r.Y_]);
            }
        }

        /// <summary>
        /// 寻找当前头到当前尾的路径
        /// </summary>
        public void FromHeadToTail(ContainerDictionary dict)
        {
            dict.SetContainerType(ContainerType.Start, Head.X, Head.Y);
            dict.SetContainerType(ContainerType.End, Tail.X, Tail.Y);
            foreach (var d in Body)
            {
                dict.SetContainerType(ContainerType.Wall, d.X, d.Y);
            }
        }
        /// <summary>
        /// 寻找当前头到当前苹果的路径
        /// </summary>
        public void FromHeadToApple(ContainerDictionary dict, LabelContainer apple)
        {
            dict.SetContainerType(ContainerType.Start, Head.X, Head.Y);
            dict.SetContainerType(ContainerType.End, apple.X, apple.Y);

            dict.SetContainerType(ContainerType.Road, Tail.X, Tail.Y);
            foreach (var d in Body)
            {
                dict.SetContainerType(ContainerType.Wall, d.X, d.Y);
            }
        }
        /// <summary>
        /// 寻找当前尾到当前苹果的路径
        /// </summary>
        public void FromTailToApple(ContainerDictionary dict, LabelContainer apple)
        {
            dict.SetContainerType(ContainerType.Start, Tail.X, Tail.Y);
            dict.SetContainerType(ContainerType.End, apple.X, apple.Y);

            dict.SetContainerType(ContainerType.Wall, Head.X, Head.Y);
            foreach (var d in Body)
            {
                dict.SetContainerType(ContainerType.Wall, d.X, d.Y);
            }
        }


        /// <summary>
        /// 在尾巴到苹果的路径找到后，寻找头到苹果
        /// </summary>
        public void FromAppleToHead(ContainerDictionary dict, LabelContainer apple, List<Coordinate> road)
        {
            var newSnake = new List<Coordinate>();
            newSnake.Add(new Coordinate { X_ = Tail.X, Y_ = Tail.Y });
            foreach (var body in Body)
            {
                newSnake.Add(new Coordinate { X_ = body.X, Y_ = body.Y });
            }
            foreach (var r in road)
            {
                newSnake.Add(new Coordinate { X_ = r.X_, Y_ = r.Y_ });
            }

            dict.SetContainerType(ContainerType.Start, Head.X, Head.Y);
            dict.SetContainerType(ContainerType.End, apple.X, apple.Y);
            foreach (var r in newSnake)
            {
                dict.SetContainerType(ContainerType.Wall, r.X_, r.Y_);
            }
        }
        /// <summary>
        /// 寻找吃掉苹果之后，头到尾的路径
        /// </summary>
        public void FromAppleToTail(ContainerDictionary dict, LabelContainer apple, List<Coordinate> road)
        {
            var newSnake = new List<Coordinate>();
            newSnake.Add(new Coordinate { X_ = Tail.X, Y_ = Tail.Y });
            foreach (var body in Body)
            {
                newSnake.Add(new Coordinate { X_ = body.X, Y_ = body.Y });
            }
            newSnake.Add(new Coordinate { X_ = Head.X, Y_ = Head.Y });
            foreach (var r in road)
            {
                newSnake.Add(new Coordinate { X_ = r.X_, Y_ = r.Y_ });
            }
            var snakeLength = Body.Count + 2;//蛇长
            var roadLength = newSnake.Count;//总长
            //移除多余的
            newSnake.RemoveRange(0, roadLength - snakeLength);

            dict.SetContainerType(ContainerType.Start, apple.X, apple.Y);
            dict.SetContainerType(ContainerType.End, newSnake[0].X_, newSnake[0].Y_);
            newSnake.RemoveAt(0);
            foreach (var r in newSnake)
            {
                dict.SetContainerType(ContainerType.Wall, r.X_, r.Y_);
            }
        }
        /// <summary>
        /// 寻找走到下一的时候，头到尾的路径
        /// </summary>
        public void FromNextHeadToTail(ContainerDictionary dict, LabelContainer next)
        {
            dict.SetContainerType(ContainerType.Start, next.X, next.Y);
            dict.SetContainerType(ContainerType.Wall, Head.X, Head.Y);
            foreach (var i in this.Body)
            {
                dict.SetContainerType(ContainerType.Wall, i.X, i.Y);
            }
            if (next.SnakeType != SnakeTypeEnum.Apple)
                dict.SetContainerType(ContainerType.End, this.Body[0].X, this.Body[0].Y);
            else
                dict.SetContainerType(ContainerType.End, this.Tail.X, this.Tail.Y);
        }
    }

    public static class ListExten
    {
        public static T Dequeue<T>(this List<T> list)
        {
            var result = list[0];
            list.RemoveAt(0);
            return result;
        }
        public static void Enqueue<T>(this List<T> list, T model)
        {
            list.Add(model);
        }
    }
}
