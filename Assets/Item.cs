using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MultiRougeLike.HexMap.Scripts
{

    public class Item
    {
        public enum Types
        {
            HeadArmor,
            BodyArmor,
            HandArmor,
            LegsArmor,
            FootArmor,
            MeleeWeapon,
            RangedWeapon
        }
        public enum BuffsType
        {
            None,
            Strength,
            Dexterity,
            Constitution,
            Intelligence,
            Wisdom,
            Charisma

        }
        public Types Type;
        public List<(BuffsType, int)> Buffs;
        public string Name;
        public string Description;
        public Item(string name, string description) { 
            Name = name;
            Description = description;
            Type = Types.MeleeWeapon;
            Buffs = new List<(BuffsType, int)>() {(BuffsType.None,0) };
        }
    }
}
