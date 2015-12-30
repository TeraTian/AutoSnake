using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReallyAutoSnake.Container
{
    public class ContainerDict
    {
        public int TotalX { get; set; }
        public int TotalY { get; set; }

        public ContainerDict(int x, int y)
        {
            this.TotalX = x;
            this.TotalY = y;
        }

        private Dictionary<Tuple<int, int>, LabelContainer> dict = new Dictionary<Tuple<int, int>, LabelContainer>();
        //索引器
        public LabelContainer this[int i, int j]
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
        /// 添加或者更新Dictionary中的对象
        /// </summary>
        /// <param name="container"></param>
        public void AddOrUpdate(LabelContainer container)
        {
            this[container.X, container.Y] = container;
        }

        public IEnumerator<LabelContainer> GetEnumerator()
        {
            foreach (var d in dict)
            {
                yield return d.Value;
            }
        }

        public List<LabelContainer> FreeLabel()
        {
            var result = new List<LabelContainer>();
            foreach (var r in this)
            {
                if (r.SnakeType == SnakeTypeEnum.None)
                {
                    result.Add(r);
                }
            }
            return result;
        }

        public int Count()
        {
            return dict.Count;
        }
    }
}
