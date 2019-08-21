using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public static class WaterLevel
    {
        public enum Key
        {
            normal,
            low,
            high
        }

        public static Key ParseKey(string s)
        {
            return (Key)Enum.Parse(typeof(Key), s);
        }
    }
}
