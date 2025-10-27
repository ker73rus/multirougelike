using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MultiRougeLike.HexMap.Scripts
{
    public class City
    {
        public string Name { get; set; }
        public Shop shop { get; set; }
        public List<Quest> quests = new List<Quest>();
        public City() {
            Name = "City";
            shop = new Shop();
            for (int i = 0; i < 3; i++)
            {
                quests.Add(new Quest("quest" + i, "Сходи туда принеси это", 50 * i + 100));
            }
        }
    }
}
