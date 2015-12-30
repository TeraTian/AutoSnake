using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WayLib;

namespace AutoSnake.Container
{
    class LabelContainer : System.Windows.Forms.Label, IShowContainer
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        private int _startDistance;
        public int StartDistance { get { return _startDistance; } set { _startDistance = value; } }
        private int _endDistance;
        public int EndDistance { get { return _endDistance; } set { _endDistance = value; } }
        private int _weight;
        public int Weight { get { return _weight; } set { _weight = value; } }
        public bool CanPass { get; set; }
        public ContainerType Type { get; set; }
        public Color ContainerColor { get; set; }
        public bool IsChecked { get; set; }
        public List<Coordinate> Way { get; set; }
        public int FromEnd1 { get; set; }
        public int FromEnd2 { get; set; }
        public List<Coordinate> Way2 { get; set; }
        public bool IsChecked2 { get; set; }
    }
}
