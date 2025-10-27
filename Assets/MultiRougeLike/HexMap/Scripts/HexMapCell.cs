using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MultiRougeLike.HexMap.Scripts
{
    public class HexMapCell : Cell
    {
        public bool blocked = false;
        public bool city = false;
        public Lazy<City> townData = new Lazy<City>();
    }
}
