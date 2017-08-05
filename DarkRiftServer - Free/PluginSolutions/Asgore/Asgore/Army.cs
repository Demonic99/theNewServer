using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asgore
{
    class Army
    {
        public byte Movement;
        public short Size;
        public short HpPerUnit;
        public short Hp;
        public short Dmg;
        public short Armor;
        public short ArmorPen;
        public byte Initiative;

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
    }
}
