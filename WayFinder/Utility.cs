using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WayLib
{
    public static class Utility
    {
        public static bool TryFindType(string typeName, out Type t)
        {
            t = null;
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = a.GetType(typeName);
                if (t != null)
                    break;
            }
            return t != null;
        }
    }
}
