using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WayLib
{
    public abstract class WayFinder
    {
        //起点和终点的坐标
        protected int startX;

        protected int startY;

        protected int endX;

        protected int endY;


        /// <summary>
        /// 路径寻找结果
        /// </summary>
        protected List<Coordinate> wayResult = new List<Coordinate>();
        public List<Coordinate> WayResult { get { return wayResult; } }
        /// <summary>
        /// 路径的寻找过程
        /// </summary>
        protected List<IShowContainer> wayProcedure = new List<IShowContainer>();
        public List<IShowContainer> WayProcedure { get { return wayProcedure; } }

        public string Execute(ContainerDictionary dict)
        {
            var result = string.Empty;
            //判断是否存在起点终点
            if (dict.StartContainer == null)
                return "没有出发点";
            if (dict.EndContainer == null)
                return "没有终点";
            //设定起点终点坐标
            startX = dict.StartContainer.X;
            startY = dict.StartContainer.Y;
            endX = dict.EndContainer.X;
            endY = dict.EndContainer.Y;
            //起点的路径为其自身
            dict.StartContainer.Way = new List<Coordinate> { new Coordinate { X_ = dict.StartContainer.X, Y_ = dict.StartContainer.Y } };

            FindWay(dict, startX, startY);
            if (wayResult.Count == 0)
                return "找不到路径";
            var startCoordinate = wayResult.FirstOrDefault(c => c.X_ == dict.StartContainer.X && c.Y_ == dict.StartContainer.Y);
            var endCoordinate = wayResult.FirstOrDefault(c => c.X_ == dict.EndContainer.X && c.Y_ == dict.EndContainer.Y);
            wayResult.Remove(startCoordinate);
            wayResult.Remove(endCoordinate);
            wayResult = wayResult.Distinct().ToList();
            result = "成功";
            return result;
        }
        public abstract void FindWay(ContainerDictionary dict, int x, int y);       
    }

    public class Direction
    {
        public static Coordinate[] DirectionArray = new Coordinate[4] 
        { 
            new Coordinate { X_ = -1, Y_ = 0 },
            new Coordinate { X_ = 0, Y_ = 1 },
            new Coordinate { X_ = 1, Y_ = 0 },
            new Coordinate { X_ = 0, Y_ = -1 },
        };

        public static Coordinate Left = new Coordinate { X_ = -1, Y_ = 0 };
        public static Coordinate Up = new Coordinate { X_ = 0, Y_ = 1 };
        public static Coordinate Right = new Coordinate { X_ = 1, Y_ = 0 };
        public static Coordinate Down = new Coordinate { X_ = 0, Y_ = -1 };
    }
    [Serializable]
    public class Coordinate
    {
        public int X_ { get; set; }
        public int Y_ { get; set; }
    }

    public static class Extension
    {
        public static T Clone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
    }
}


