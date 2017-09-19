using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asgore
{
    public class Cards
    {
        public ushort Id;
        public byte Movement;
        public short Hp;
        public short Dmg;
        public Position CardPosition;
        public short Spawn;
        public short Regen;
        public State CardState;
        public ushort ArmyId;
        public SpecialAttack Attack;

        public Cards (ushort id, byte movement, short hp, short dmg, Position cardPosition, short spawn, short regen, State cardState, ushort armyId, ushort specialAttackId)
        {
            Id = id;
            Movement = movement;
            Hp = hp;
            Dmg = dmg;
            CardPosition = cardPosition;
            Spawn = spawn;
            Regen = regen;
            CardState = cardState;
            ArmyId = armyId;
            Attack = SpecialAttack.createSpecialAttack(specialAttackId);
        }
        public enum State
        {
            NORMAL = 0, STANDBY = 1, DODGE = 2, SPECIAL = 3
        }
        public enum Position
        {
            OWN = 0, FIELD = 1, ENEMY = 2
        }
        public static Cards createCharacter(ushort Id)
        {
            switch (Id)
            {
                case 0:
                    return new Cards(0, 1, 5, 1, Position.OWN, 50, 50, State.NORMAL, 1, 1);
            }
            return new Cards(0, 1, 5, 1, Position.OWN, 50, 50, State.NORMAL, 1, 1);
        }
    }
    public class SpecialAttack
    {
        public ushort Id;
        public byte Cooldown;
        public SpecialAttack (ushort id, byte cooldown)
        {
            Id = id;
            Cooldown = cooldown;
        }
        public virtual void cast(Cards caster)
        {

        }
        public static SpecialAttack createSpecialAttack (ushort Id)
        {
            switch (Id)
            {
                case 0:
                    return new SpecialAttack(0,255);
                case 1:
                    return new TestAttack();
            }
            return new SpecialAttack(0,255);
        }
    }
    public class TestAttack : SpecialAttack
    {
        public TestAttack () : base(1,2) { }
        public override void cast(Cards caster)
        {
            caster.Hp++;
        }
    }
}
