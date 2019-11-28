using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    // TODO:
    // Add Print() method
    // Move damage calculations to static func here
    // Damage calc type variable ?
    class Skill
    {
        public enum SkillType
        {
            Physical,
            Magical,
            Effect,
            Buff
        }

        public enum Elements
        {
            Common,
            Wild,
            Light,
            Dark,
            Spirit,
            Demon,
            Mythic
        }

        public enum TargetType
        {
            Single,
            Ally,
            Adjacent,
            Self,
            AllEnemies,
            AllAllies,
            Front,
            Sides
        }

        public struct CombatResult
        {
            public int Damage;
            public int SourceIndex;
            public int TargetIndex;
            public bool CriticalHit;
            public string Source;
            public string Target;
            public string Skill;

            public CombatResult(Unit source, Unit target, Skill skill, int damage, bool crit)
            {
                Damage = damage;
                SourceIndex = source.Index;
                TargetIndex = target.Index;
                CriticalHit = crit;
                Source = source.Name;
                Target = target.Name;
                Skill = skill.Name;
            }
        }

        public static Random random = new Random();

        public static int TargetSingle = 0;
        public static int TargetAll = 1;
        public static int TargetFront = 2;
        public static int TargetAdjacent = 3;
        public static int TargetAlly = 4;
        public static int TargetSelf = 5;
        public static int TargetSides = 6;
        public static int TargetAllies = 7;

        // tier 1 skills
        public static Skill Assault = new Skill("Assault", 55);
        public static Skill Backlash = new Skill("Backlash", 55);
        public static Skill Torment = new Skill("Torment", 60, Skill.SkillType.Magical);
        public static Skill Charge = new Skill("Charge", 45, SkillType.Physical,
            "Has an increased critical hit chance.").SetCritChance(0.2);

        // updated
        public static Skill RecklessCharge = new Skill("Reckless Charge", 100, SkillType.Physical,
            (self, target) =>
            {
                CombatResult cr = DealDamage(self, target, RecklessCharge);

                self.Inflict(new StatusEffect(StatusEffect.Stun, 2));
                if(random.Next(2) == 1)
                {
                    target.Inflict(new StatusEffect(StatusEffect.Stun, 2));
                }
            }, "Has a 50% chance to inflict Stun(2) on target. Inflicts Stun(2) on self.");
        public static Skill TailWag = new Skill("Tail Wag", SkillType.Effect,
            (self, target) =>
            {
                PrintSkill(self, target, TailWag);

                target.DebuffStat(Unit.Speed, 2);
                target.DebuffStat(Unit.Armor, 2);
            }, "Lowers target Spd/Amr by 2.");
        public static Skill Bite = new Skill("Bite", 55, SkillType.Physical,
            (self, target) =>
            {
                CombatResult cr = DealDamage(self, target, Bite);

                if(random.Next(2) == 1)
                {
                    target.DebuffStat(Unit.Armor, 1);
                }
            }, "Has a chance to lower Armor by 1.");
        public static Skill FastSwipe = new Skill("Fast Swipe", 40, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, FastSwipe);
                self.BuffStat(Unit.Speed, 1);
            }, "Increase self Speed(1).");
        public static Skill Gouge = new Skill("Gouge", 50, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, Gouge);
                target.Inflict(new StatusEffect(StatusEffect.Bleed, 2, self, target));
            }, "Inflicts Bleed(2).");
        public static Skill BugBite = new Skill("Bug Bite", 40, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, BugBite);
                target.Inflict(new StatusEffect(StatusEffect.Bleed, 2, self, target));
            }, "Inflicts Bleed(2) and heals unit for 10% max HP.");
        public static Skill BuzzBy = new Skill("Buzz-by", 10, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, BuzzBy);
                self.BuffStat(Unit.Speed, 2);
                self.BuffStat(Unit.Strength, 2);
            }, "Increases self Spd/Str by 2.");
        public static Skill Swarm = new Skill("Swarm", 15, SkillType.Physical,
            (self, target, units) =>
            {
                int numAttacks = random.Next(4) + 5;
                List<Unit> enemies = units.EnemyList;

                for(int i = 0; i < numAttacks; i++)
                {
                    int index = 0;
                            // find enemy index to attack
                            while(!units.BattleOver())
                    {
                        index = random.Next(enemies.Count);
                        if(enemies[index].IsAlive)
                            break;
                    }

                    DealDamage(self, enemies[index], Swarm);
                }
            }, "Randomly damages enemies 5-8 times.")
            .SetTargetType(Skill.TargetAll);
        public static Skill SoulSiphon = new Skill("Soul Siphon", 55, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, SoulSiphon);
                if(!target.IsAlive)
                {
                    self.CurrHP += target.HP / 2;
                }
            }, "If this move kills an enemy, heal self for 50% of target's max health.");
        public static Skill Nullify = new Skill("Nullify", 20, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, Nullify);
                for(int i = 0; i < target.StatTiers.Length; i++)
                {
                    if(target.StatTiers[i] > 0)
                        target.StatTiers[i] = 0;
                }
            }, "Removes all stat bonuses from target.");
        public static Skill Sacrifice = new Skill("Sacrifice", SkillType.Buff,
            (self, target, units) =>
            {
                int healAmount = (int)(self.Str * 0.5);
                for(int i = 0; i < units.Length / 2; i++)
                {
                    if(units[i].IsAlive)
                        units[i].CurrHP += healAmount;
                }
                self.Inflict(new StatusEffect(StatusEffect.Bleed, 2, self, self));
            }, "Heal all allies for 50% of unit's Strength. Inflicts Bleed(2) on self.")
            .SetTargetType(Skill.TargetAllies);
        public static Skill Reprisal = new Skill("Reprisal", 40, SkillType.Physical,
            (self, target) =>
            {
                CombatResult cr = CalculateCombat(self, target, Reprisal);
                if(self.StatusEffects.Count > 0)
                {
                    cr.Damage *= 2;
                }
                target.CurrHP -= cr.Damage;
                PrintCombatResult(cr);
            }, "Deals double damage if self is affected by a status condition.");
        public static Skill TongueLash = new Skill("Tongue Lash", 55, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, TongueLash);
                if(Game1.random.Next(3) == 0)
                {
                    target.DebuffStat(Unit.Speed, 4);
                }
            }, "Has a chance to lower target Speed by 4.");
        public static Skill AdaptiveBlow = new Skill("Adaptive Blow", 40, SkillType.Physical,
            (self, target) =>
            {
                CombatResult cr = CalculateCombat(self, target, AdaptiveBlow);
                if(self.PercentHealth() >= 0.5)
                {
                    cr.Damage *= 2;
                }
                target.CurrHP -= cr.Damage;
                PrintCombatResult(cr);

                if(self.PercentHealth() < 0.5)
                {
                    self.CurrHP += (int)(0.8 * cr.Damage);
                }
            }, "If self HP is < 50%, deals double damage. Otherwise, heals unit for 80% damage dealt.");
        public static Skill Impede = new Skill("Impede", SkillType.Effect,
            (self, target) =>
            {
                target.Inflict(new StatusEffect(StatusEffect.Stun, 4));
                target.BuffStat(Unit.Armor, 2);
                target.BuffStat(Unit.Resistance, 2);
            }, "Inflicts Stun(4). Increases target Amr/Res by 2.");
        public static Skill PiercingGaze = new Skill("Piercing Gaze", 50, SkillType.Physical,
            "Ignores 30% of target Armor.").SetPenetration(0.3);
        public static Skill SideWhip = new Skill("Side Whip", 45, SkillType.Physical,
            (self, target, units) =>
            {
                // select top unit
                int topIndex = units.Capacity / 2;

                for(int i = topIndex; i < units.Capacity; i++)
                {
                    if(units[i].IsAlive)
                    {
                        topIndex = i;
                        break;
                    }
                }

                // select bot unit
                int botIndex = units.Capacity - 1;

                for(int i = botIndex; i >= topIndex; i--)
                {
                    if(units[i].IsAlive)
                    {
                        botIndex = i;
                        break;
                    }
                }

                DealDamage(self, units[topIndex], SideWhip);
                DealDamage(self, units[botIndex], SideWhip);
            }, "Damages the top and bottom enemy. Has a boosted crit chance.")
            .SetTargetType(Skill.TargetSides)
            .SetCritChance(0.2);
        public static Skill Maelstrom = new Skill("Maelstrom", 45, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, Maelstrom);
                self.BuffStat(Unit.Armor, 2);
                self.BuffStat(Unit.Resistance, 2);
            }, "Increases unit's Amr/Res by 2.");
        public static Skill RockSmash = new Skill("Rock Smash", 55, SkillType.Physical,
            "Has an increased critical strike chance.").SetCritChance(0.25);
        public static Skill Earthquake = new Skill("Earthquake", 150, SkillType.Physical,
            (self, target, units) =>
            {
                foreach(Unit unit in units.EnemyList)
                {
                    CombatResult cr = DealDamage(self, unit, Earthquake);

                    if(random.Next(10) == 0)
                    {
                        unit.Inflict(new StatusEffect(StatusEffect.Stun, 1));
                    }
                }
            }, "Damages all enemies and has a chance to inflict Stun(1).")
            .SetTargetType(Skill.TargetAll);
        public static Skill Drain = new Skill("Drain", 55, SkillType.Magical,
            (self, target) =>
            {
                CombatResult cr = DealDamage(self, target, Drain);
                self.CurrHP += cr.Damage;
            }, "Heals self for half damage dealt.");
        public static Skill Smooch = new Skill("Smooch", 40, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, Smooch);
                target.DebuffStat(Unit.Resistance, 2);
            });
        public static Skill BigMagicChurro = new Skill("Big Magic Churro", SkillType.Buff,
            (self, target) =>
            {
                PrintSkill(self, target, BigMagicChurro);
                int heal = (int)(self.Fcs * 0.40);
                target.CurrHP += heal;
                self.CurrHP += heal;
                target.BuffStat(Unit.Speed, 2);
            }, "Heal self and target = 45% Focus and increase target Speed(1).")
            .SetTargetType(Skill.TargetAlly);
        public static Skill SonicBuzz = new Skill("Sonic Buzz", 65, SkillType.Magical);
        public static Skill CorrodingBite = new Skill("Corroding Bite", 40, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, CorrodingBite);
                target.DebuffStat(Unit.Armor, 2);
                target.DebuffStat(Unit.Resistance, 2);
            }, "Decrease target Amr/Res(2).");
        public static Skill Incantation = new Skill("Incantation", SkillType.Buff,
            (self, target) =>
            {
                target.CurrHP += (int)(target.HP * 0.2);
                target.BuffStat(Unit.Strength, 1);
                target.BuffStat(Unit.Focus, 1);
                target.BuffStat(Unit.Armor, 1);
                target.BuffStat(Unit.Resistance, 1);
            }, "Heal target ally (15% unit's Health) and increase Str/Fcs/Amr/Res(1).")
            .SetTargetType(Skill.TargetAlly);
        public static Skill ToxicCloud = new Skill("Toxic Cloud", 10, SkillType.Magical,
            (self, target, units) =>
            {
                foreach(Unit unit in units.EnemyList)
                {
                    DealDamage(self, unit, ToxicCloud);
                    unit.Inflict(new StatusEffect(StatusEffect.Poison, 2, self, target));
                }
            }, "Damage all enemies and inflicts Poison(2).")
            .SetTargetType(Skill.TargetAll);
        public static Skill VorpalStrike = new Skill("Vorpal Strike", 35, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, VorpalStrike);
                switch(random.Next(4))
                {
                    case 0:
                        target.Inflict(new StatusEffect(StatusEffect.Bleed, 2, self, target));
                        break;
                    case 1:
                        target.Inflict(new StatusEffect(StatusEffect.Burn, 2, self, target));
                        break;
                    case 2:
                        target.Inflict(new StatusEffect(StatusEffect.Poison, 2, self, target));
                        break;
                }
            }, "Randomly inflicts Burn(2), Bleed(2), or Poison(2).");
        public static Skill Retaliate = new Skill("Retaliate", 30, SkillType.Magical,
            (self, target) =>
            {
                double temp = self.PercentHealth();
                double damageMod = 6.0 * (1.0 - temp);
                if(damageMod < 1.0)
                    damageMod = 1.0;
                DealDamage(self, target, Retaliate, damageMod);
            }, "Deals more damage the less health this unit has.");
        public static Skill Ignite = new Skill("Ignite", 50, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, Ignite);
                target.Inflict(new StatusEffect(StatusEffect.Burn, 3, self, target));
            }, "Inflicts Burn(3).");
        public static Skill PsychicShackle = new Skill("Psychic Shackle", 85, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, PsychicShackle);
                target.DebuffStat(Unit.Speed, 2);
            }, "Decrease target Speed by 2.");
        public static Skill FallingIce = new Skill("Falling Ice", 55, SkillType.Physical,
            (self, target, units) =>
            {
                int targetIndex = target.Index;

                DealDamage(self, target, FallingIce);

                if(targetIndex - 1 > 0 &&
                    units[targetIndex - 1].IsEnemy)
                {
                    DealDamage(self, units[targetIndex - 1], FallingIce, 0.5);
                }
                if(targetIndex + 1 < units.Length &&
                    units[targetIndex + 1].IsAlive)
                {
                    DealDamage(self, units[targetIndex + 1], FallingIce, 0.5);
                }
            }, "Deals 50% damage to adjacent units.")
            .SetTargetType(Skill.TargetAdjacent);
        public static Skill FlashFreeze = new Skill("Flash Freeze", 30, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, FlashFreeze);
                target.Inflict(new StatusEffect(StatusEffect.Stun, 2));
            }, "Inflicts Stun(2).");
        public static Skill HeatUp = new Skill("Heat Up", 0, SkillType.Buff,
            (self, target) =>
            {
                self.BuffStat(Unit.Speed, 1);
                self.BuffStat(Unit.Strength, 1);
                self.BuffStat(Unit.Focus, 1);
            }, "Increases self Spd/Str/Fcs by 1.")
            .SetTargetType(Skill.TargetSelf);
        public static Skill Frenzy = new Skill("Frenzy", 100, SkillType.Physical,
            (self, target) =>
            {
                DealDamage(self, target, Frenzy);
                self.CurrHP = (int)(self.CurrHP * 0.8);
            }, "Damages self for 20% of current health.");
        public static Skill Ingrain = new Skill("Ingrain", SkillType.Buff,
            (self, target) =>
            {
                self.BuffStat(Unit.Armor, 3);
                self.BuffStat(Unit.Resistance, 3);
            }, "Increase self Amr/Res by 3.")
            .SetTargetType(TargetSelf);
        public static Skill PoisonSpike = new Skill("Poison Spike", 10, SkillType.Magical,
            (self, target) =>
            {
                DealDamage(self, target, PoisonSpike);
                target.Inflict(new StatusEffect(StatusEffect.Poison, 4, self, target));
            }, "Inflicts Poison(4).");
        public static Skill Infuse = new Skill("Infuse", SkillType.Buff,
            (self, target) =>
            {
                for(int i = 0; i < target.StatTiers.Length; i++)
                {
                    if(target.StatTiers[i] > 0)
                    {
                        target.StatTiers[i] *= 2;
                    }
                }
                target.UpdateStats();
            }, "Doubles all stat mods on target.");
        public static Skill PhantasmalRend = new Skill("Phantasmal Rend", 55, SkillType.Magical,
            (self, target) =>
            {
                double rendMod = 1.0;
                for(int i = 0; i < target.StatTiers.Length; i++)
                {
                    if(target.StatTiers[i] > 0)
                    {
                        rendMod += target.StatTiers[i] * 0.5;
                        target.StatTiers[i] = 0;
                    }
                }
                target.UpdateStats();
                DealDamage(self, target, PhantasmalRend, rendMod);
            }, "Deals more damage the more stat buffs target has and removes all stat buffs.");

        public delegate void SkillEffect(Unit self, Unit target);
        public delegate void SkillEffectAll(Unit self, Unit target, UnitList units);
        public delegate int SkillDamageTarget(Unit self, Unit target, Skill skill);
        public delegate int SkillDamageAndHeal(Unit self, Unit target, Skill skill);
        public delegate int SkillDamageEnemies(Unit self, List<Unit> enemies, Skill skill);

        public static SkillDamageTarget DefaultDamageTarget = (self, target, skill) =>
        {
            int damage = 0;
            int attack = 0;
            int defense = 0;
            int power = skill.Power;
            int level = self.Level;
            double crit = 1.0;

            if(skill.Type == Skill.SkillType.Physical)
            {
                attack = self.Str;
                defense = target.Amr;
            }
            else if(skill.Type == Skill.SkillType.Magical)
            {
                attack = self.Fcs;
                defense = target.Res;
            }

            if(random.Next((int)(1/skill.CritChance)) == 0)
            {
                crit = skill.CritDamage;
            }

            defense = (int)(defense * (1.0 - skill.Penetration));

            damage = (int)((power * attack / defense * level / 100) * (random.Next(15) / 100 + 1.35) * crit);

            if(damage < 1)
                damage = 1;

            target.CurrHP -= damage;

            Console.WriteLine($"{self.Name} used {skill.Name} on {target.Name} for {damage} damage!");

            if(crit > 1.0)
                Console.WriteLine($"Critical hit!");

            return damage;
        };
        public static SkillDamageAndHeal DefaultDamageAndHeal = (self, target, skill) =>
        {
            int damage = 0;
            int attack = 0;
            int defense = 0;
            int heal = 0;
            int power = skill.Power;
            int level = self.Level;
            double crit = 1.0;

            if(skill.Type == Skill.SkillType.Physical)
            {
                attack = self.Str;
                defense = target.Amr;
            }
            else if(skill.Type == Skill.SkillType.Magical)
            {
                attack = self.Fcs;
                defense = target.Res;
            }

            if(random.Next((int)(1 / skill.CritChance)) == 0)
            {
                crit = skill.CritDamage;
            }

            defense = (int)(defense * (1.0 - skill.Penetration));

            damage = (int)((power * attack / defense * level / 100) * (random.Next(15) / 100 + 1.35) * crit);

            if(damage < 1)
                damage = 1;

            target.CurrHP -= damage;
            heal = (int)(damage * skill.HealPercent);
            self.CurrHP += heal;

            Console.WriteLine($"{self.Name} used {skill.Name} on {target.Name} for {damage} damage!");
            Console.WriteLine($"{self.Name} healed {heal} health!");

            if(crit > 1.0)
                Console.WriteLine($"Critical hit!");

            return damage;
        };

        /// <summary>
        /// Apply default single target damage formula and returns damage value as an int.
        /// Does not apply damage to target unit.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static CombatResult CalculateCombat(Unit self, Unit target, Skill skill)
        {
            int damage = 0;
            int attack = 0;
            int defense = 0;
            int power = skill.Power;
            int level = self.Level;
            double critMod = 1.0;
            bool crit = random.Next((int)(1 / skill.CritChance)) == 0;
            //bool cri

            // check if skill inflicts physical or magical damage
            if(skill.Type == SkillType.Physical)
            {
                attack = self.Str;
                defense = target.Amr;
            }
            else if(skill.Type == SkillType.Magical)
            {
                attack = self.Fcs;
                defense = target.Res;
            }

            // check for critical strike
            // TODO: make version without crit chance for AI calculations
            if(crit)
            {
                critMod = skill.CritDamage;
            }

            // calculate defense modified by penetration
            defense = (int)(defense * (1.0 - skill.Penetration));

            // calculate skill damage
            damage = (int)((power * attack / defense * level / 100.0) * (random.Next(15) / 100.0 + 1.35) * critMod);

            if(damage < 1)
                damage = 1;

            // create combat result struct
            CombatResult combatResult = new CombatResult(self, target, skill, damage, crit);

            return combatResult;
        }

        public static CombatResult CalculateCombat(Unit self, Unit target, Skill skill, int damage)
        {
            if(damage < 1)
                damage = 1;

            // create combat result struct
            CombatResult combatResult = new CombatResult(self, target, skill, damage, false);

            return combatResult;
        }

        /// <summary>
        /// Calculate damage using default damage calculation, then decreases target health and prints result.
        /// </summary>
        private static CombatResult DealDamage(Unit self, Unit target, Skill skill)
        {
            CombatResult cr = CalculateCombat(self, target, skill);
            target.CurrHP -= cr.Damage;
            PrintCombatResult(cr);
            return cr;
        }

        /// <summary>
        /// Decreases target health by set amount and prints result.
        /// </summary>
        private static CombatResult DealDamage(Unit self, Unit target, Skill skill, int damage)
        {
            //CombatResult cr = CalculateCombat(self, target, skill, damage);
            CombatResult cr = CalculateCombat(self, target, skill);
            cr.Damage = damage;
            cr.CriticalHit = false;
            target.CurrHP -= cr.Damage;
            PrintCombatResult(cr);
            return cr;
        }

        /// <summary>
        /// Calculate damage using default damage calculation multiplied by passed in modifier, then 
        /// decreases target health and prints result.
        /// </summary>
        private static CombatResult DealDamage(Unit self, Unit target, Skill skill, double modifier)
        {
            CombatResult cr = CalculateCombat(self, target, skill);
            cr.Damage = (int)(cr.Damage * modifier);
            target.CurrHP -= cr.Damage;
            PrintCombatResult(cr);
            return cr;
        }

        /// <summary>
        /// Prints CombatResult struct to console.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="combatResult"></param>
        private static void PrintCombatResult(CombatResult combatResult)
        {
            string sourceName = $"{combatResult.Source}({combatResult.SourceIndex})";
            string skillName = combatResult.Skill;
            string targetName = $"{combatResult.Target}({combatResult.TargetIndex})";
            Console.WriteLine($"{sourceName} used {skillName} on {targetName} for {combatResult.Damage} damage!");
            if(combatResult.CriticalHit)
            {
                Console.WriteLine("Critical hit!");
            }
        }

        private static void PrintSkill(Unit self, Skill skill)
        {
            Console.WriteLine($"{self.Name}({self.Index}) used {skill.Name}!");
        }

        private static void PrintSkill(Unit self, Unit target, Skill skill)
        {
            Console.WriteLine($"{self.Name}({self.Index}) used {skill.Name} on {target.Name}({target.Index})!");
        }

        public string Name;
        private int tier;
        private int tiers;
        private string description = "";
        public int Power { get; }
        public double Penetration { get; set; }
        public SkillType Type { get; set; }
        public SkillEffect Effect { get; set; }
        public SkillEffectAll EffectAll { get; set; }
        private SkillDamageTarget damageTarget;
        private SkillDamageAndHeal damageAndHeal;
        private SkillDamageEnemies damageEnemies;
        public double CritChance { get; private set; } = 0.05;
        public double CritDamage { get; private set; } = 1.50;
        public double HealPercent { get; private set; } = 0.0;

        private int targetType = 0;

        public string Description
        {
            get
            {
                string result = $"{Name}";
                if(Power > 0)
                    result += $"\n      P {Power}";
                else
                    result += $"\n      P --";
                if(description.Length > 0)
                    result += "\n" + description;

                return result;
            }
            set { description = value; }
        }

        public int GetTargetType
        {
            get { return targetType; }
        }

        public Skill(String name, int power)
        {
            tier = 0;
            Name = name;
            this.Power = power;
            Penetration = 0.0;
            Type = SkillType.Physical;
            damageTarget = Skill.DefaultDamageTarget;
            description = "";
        }

        public Skill(String name, int power, SkillType type) : this(name, power)
        {
            Type = type;
        }

        public Skill(String name, int power, SkillEffect effect) : this(name, power)
        {
            Effect = effect;
        }

        public Skill(String name, int power, SkillType type, SkillEffect effect) : this(name, power)
        {
            Type = type;
            Effect = effect;
        }

        public Skill(String name, SkillEffect effect) : this(name, 0)
        {
            Type = SkillType.Effect;
            Effect = effect;
        }

        public Skill(String name, SkillType type, SkillEffect effect) : this(name, 0)
        {
            Type = type;
            Effect = effect;
        }

        public Skill(String name, SkillType type, SkillEffect effect, string description) : this(name, 0)
        {
            Type = type;
            Effect = effect;
            this.description = description;
        }

        public Skill(String name, int power, SkillType type, SkillEffect effect, string description)
            : this(name, power)
        {
            Type = type;
            Effect = effect;
            this.description = description;
        }

        public Skill(String name, SkillEffectAll effect) : this(name, 0)
        {
            Type = SkillType.Effect;
            EffectAll = effect;
        }

        public Skill(String name, SkillType type, SkillEffectAll effect) : this(name, 0)
        {
            Type = type;
            EffectAll = effect;
        }

        public Skill(String name, SkillType type, SkillEffectAll effect, string description) : this(name, 0)
        {
            Type = type;
            EffectAll = effect;
            this.description = description;
        }

        public Skill(String name, int power, SkillType type, string description) : this(name, power)
        {
            Type = type;
            this.description = description;
        }

        public Skill(String name, int power, SkillType type, SkillEffectAll effect, string description)
            : this(name, power)
        {
            Type = type;
            EffectAll = effect;
            this.description = description;
        }

        public Skill SetTargetType(int targetType)
        {
            this.targetType = targetType;

            return this;
        }

        /// <summary>
        /// Set pentration to a value between 0.0 and 1.0; represents the percent of target's
        /// defensive stat to ignore.
        /// </summary>
        /// <param name="pen">Penetration value.</param>
        /// <returns>Reference to this Skill.</returns>
        public Skill SetPenetration(double pen)
        {
            this.Penetration = pen;

            return this;
        }

        /// <summary>
        /// Set critical hit chance to a value between 0.0 and 1.0; default value is 0.05.
        /// </summary>
        /// <param name="crit">Critical hit chance value.</param>
        /// <returns>Reference to this Skill.</returns>
        public Skill SetCritChance(double crit)
        {
            this.CritChance = crit;

            return this;
        }

        public Skill SetCritDamage(double damage)
        {
            this.CritDamage = damage;

            return this;
        }

        public int Invoke(Unit self, Unit target, UnitList unitList)
        {
            if(Effect != null)
            {
                Effect?.Invoke(self, target);
            }
            else if(EffectAll != null)
            {
                EffectAll?.Invoke(self, target, unitList);
            }
            else
            {
                // default damage
                CombatResult combatResult = DealDamage(self, target, this);
            }

            return 0;
        }
    }
}
