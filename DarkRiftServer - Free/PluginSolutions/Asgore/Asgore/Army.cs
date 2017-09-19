using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asgore
{
    struct Army
    {
        public short ArmyId;
        public byte Movement;
        public short Size;
        public short HpPerUnit;
        public short Hp;
        public short Dmg;
        public short Armor;
        public short ArmorPen;
        public byte Initiative;

        public Army (short armyId, byte movement, short size, short hpPerUnit, short dmg, short armor, short armorPen, byte initiative)
        {
            ArmyId = armyId;
            Movement = movement;
            Size = size;
            HpPerUnit = hpPerUnit;
            Hp = (short)(hpPerUnit * size);
            Dmg = dmg;
            Armor = armor;
            ArmorPen = armorPen;
            Initiative = initiative;
        }

        public static Army CreateArmy(ushort armyId, short size)
        {
            switch (armyId)
            {
                case 0:
                    return new Army(0, 1, size, 1, 1, 0, 0, 2);
            }
            return new Army(0, 1, size, 1, 1, 0, 0, 2);
        }

        public void TakeDmg(short incomingDmg, short enemyPen)
        {
            short reducedArmor = (short)(Armor - enemyPen);
            if (reducedArmor < 0)
                reducedArmor = 0;
            short dmgInput = (short)(incomingDmg - reducedArmor);
            if (dmgInput < 0)
                dmgInput = 0;
            Hp -= dmgInput;
            Size =(short)( Math.Ceiling((double)Hp / (double)HpPerUnit));
        }

        public static Army CombineArmies(Army army1, Army army2)
        {
            Army CombinedArmy = new Army(army1.ArmyId, army1.Movement, (short)(army1.Size + army2.Size), army1.HpPerUnit, army1.Dmg, army1.Armor, army1.ArmorPen, army1.Initiative);
            return CombinedArmy;
        }

        public static List<Army> SplitArmy(Army army, short engagingArmyCount)
        {
            List<Army> armylist = new List<Army>();
            Army BaseArmy = new Army(army.ArmyId, army.Movement, (short)(army.Size - engagingArmyCount), army.HpPerUnit, army.Dmg, army.Armor, army.ArmorPen, army.Initiative);
            Army EngagingArmy = new Army(army.ArmyId, army.Movement, engagingArmyCount, army.HpPerUnit, army.Dmg, army.Armor, army.ArmorPen, army.Initiative);
            armylist.Add(BaseArmy);
            armylist.Add(EngagingArmy);
            return armylist;
        }
    }
}
