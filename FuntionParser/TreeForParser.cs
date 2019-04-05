using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuntionParser
{
    public struct Tree
    {
        public int left_child;
        public int right_child;
        public string value;
        public int parent;
        public void Init(int left_child, int right_child, string value, int parent)
        {
            this.left_child = left_child;
            this.right_child = right_child;
            this.value = value;
            this.parent = parent;
        }

    }
}
