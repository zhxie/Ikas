using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Base
    {
        public int Error { get; }

        public Base()
        {
            Error = -1;
        }
        public Base(int error)
        {
            Error = error;
        }
    }
}
