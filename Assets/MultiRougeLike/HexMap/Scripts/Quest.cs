using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MultiRougeLike.HexMap.Scripts
{
    public class Quest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reward { get; set; }
        public Quest(string  name, string description, int reward) { 
            Name = name;
            Description = description;
            Reward = reward;
        }
    }
}
