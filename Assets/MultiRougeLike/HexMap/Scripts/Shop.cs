using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MultiRougeLike.HexMap.Scripts
{
    public class Shop
    {
        public class ItemPrice
        {
            public Item Item;
            public int Price;
            public ItemPrice(Item item, int price)
            {
                Item = item;
                Price = price;
            }
        }
        public List<ItemPrice> Items = new();

        public Shop()
        {
            for (int i = 0; i < 5; i++)
            {
                Items.Add(new ItemPrice(new Item("item" + i, "desc" + i), 50 + i * 100));
            }
        }
    }
}
