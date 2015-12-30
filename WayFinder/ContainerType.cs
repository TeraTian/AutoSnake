using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayLib
{
    public enum ContainerType
    {
        Start = 0,
        End = 1,
        Wall = 2,
        Road = 3
    }
    public class TypeProperty
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public bool CanPass { get; set; }
        public Color ContainerColor { get; set; }
    }
    public class ProtertyArray
    {
        public static List<TypeProperty> Array = new List<TypeProperty>()
        {
            new TypeProperty(){Name="Start",Weight=0,CanPass=false,ContainerColor = Color.Red},
            new TypeProperty(){Name="End",Weight=0,CanPass=false,ContainerColor = Color.Blue},
            new TypeProperty(){Name="Wall",Weight=0,CanPass=false,ContainerColor = Color.Black},
            new TypeProperty(){Name="Road",Weight=0,CanPass=true,ContainerColor = Color.White}
        };
    }
}
