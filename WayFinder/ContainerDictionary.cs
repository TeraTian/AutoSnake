using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayLib
{
    public class ContainerDictionary : IEnumerable<IShowContainer>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">x最大索引</param>
        /// <param name="y">y最大索引</param>
        public ContainerDictionary(int x, int y)
        {
            TotalX = x;
            TotalY = y;
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    this[i, j] = new ShowContainer(i, j);
                }
            }
        }

        private Dictionary<Tuple<int, int>, IShowContainer> dict = new Dictionary<Tuple<int, int>, IShowContainer>();
        public IShowContainer StartContainer { get; set; }
        public IShowContainer EndContainer { get; set; }
        public IShowContainer LastStartContainer { get; set; }
        public IShowContainer LastEndContainer { get; set; }
        //地图区块的属性数组
        //下表和地图区块的枚举数值相对应
        private List<TypeProperty> proArray = ProtertyArray.Array;

        //最大的坐标值，用来控制范围
        public int TotalX { get; set; }
        public int TotalY { get; set; }
        //索引器
        public IShowContainer this[int i, int j]
        {
            get { return dict.ContainsKey(new Tuple<int, int>(i, j)) ? dict[new Tuple<int, int>(i, j)] : null; }
            set
            {
                if (dict.ContainsKey(new Tuple<int, int>(i, j)))
                    dict[new Tuple<int, int>(i, j)] = value;
                else
                    dict.Add(new Tuple<int, int>(i, j), value);
            }
        }
        /// <summary>
        /// 重置所有属性，可以重新部署界面
        /// </summary>
        public void Reset()
        {
            foreach (var d in dict)
            {
                var container = d.Value;
                container.IsChecked = false;
                container.StartDistance = 0;
                container.EndDistance = 0;
                container.CanPass = true;
                container.StartDistance = 0;
                container.ContainerColor = Color.White;
                container.Type = ContainerType.Road;
                container.Way = null;
            }
            this.StartContainer = null;
            this.EndContainer = null;
            this.LastStartContainer = null;
            this.LastStartContainer = null;
        }
        /// <summary>
        /// 清空寻路相关属性，可以重新寻找路径，不改变界面
        /// </summary>
        public void ReStartSet()
        {
            foreach (var d in dict)
            {
                var container = d.Value;
                container.IsChecked = false;
                container.StartDistance = 0;
                container.EndDistance = 0;
                container.Way = null;
            }
        }

        /// <summary>
        /// 添加或者更新Dictionary中的对象
        /// </summary>
        /// <param name="container"></param>
        public void AddOrUpdate(IShowContainer container)
        {
            this[container.X, container.Y] = container;
        }
        /// <summary>
        /// 显示可以通过的格子的个数
        /// </summary>
        public int CanPassCount()
        {
            var l = dict.Where(d => d.Value.CanPass).Count();
            return l;
        }
        /// <summary>
        /// 设置某个区块的类型
        /// </summary>
        /// <param name="type">区块类型，枚举类型</param>
        /// <param name="x">坐标X</param>
        /// <param name="y">坐标Y</param>
        public void SetContainerType(ContainerType type, int x, int y)
        {
            if (type == ContainerType.Start)
            {
                if (StartContainer != null)
                {
                    //将现在的start变回road
                    StartContainer.CanPass = proArray[(int)ContainerType.Road].CanPass;
                    StartContainer.Weight = proArray[(int)ContainerType.Road].Weight;
                    StartContainer.ContainerColor = proArray[(int)ContainerType.Road].ContainerColor;
                    StartContainer.Type = ContainerType.Road;
                    LastStartContainer = StartContainer;
                }
                StartContainer = this[x, y];
            }
            if (type == ContainerType.End)
            {
                if (EndContainer != null)
                {
                    //将现在的end变回road
                    EndContainer.CanPass = proArray[(int)ContainerType.Road].CanPass;
                    EndContainer.Weight = proArray[(int)ContainerType.Road].Weight;
                    EndContainer.ContainerColor = proArray[(int)ContainerType.Road].ContainerColor;
                    EndContainer.Type = ContainerType.Road;
                    LastEndContainer = EndContainer;
                }
                EndContainer = this[x, y];
            }

            this[x, y].CanPass = proArray[(int)type].CanPass;
            this[x, y].Weight = proArray[(int)type].Weight;
            this[x, y].ContainerColor = proArray[(int)type].ContainerColor;
            this[x, y].Type = type;
        }

        public IEnumerator<IShowContainer> GetEnumerator()
        {
            foreach (var d in dict)
            {
                yield return d.Value;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
