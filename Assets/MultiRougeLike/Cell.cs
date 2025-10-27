using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.MultiRougeLike
{
    public class Cell:MonoBehaviour
    {
        public (int x, int y) Position
        {
            get
            {
                return (this.x, this.y);
            }
        }
        public int x;
        public int y;
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
