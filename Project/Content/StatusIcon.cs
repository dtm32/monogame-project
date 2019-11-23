using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Game.Content
{
    class IconManager
    {
        private Texture2D poisonIcon;
        private Texture2D burnIcon;
        private Texture2D bleedIcon;
        private Texture2D stunIcon;
        private Texture2D physicalIcon;
        private Texture2D magicalIcon;
        private Texture2D statusIcon;
        private Texture2D buffIcon;

        public IconManager(Texture2D poisonIcon, Texture2D burnIcon, Texture2D bleedIcon,
            Texture2D stunIcon, Texture2D physicalIcon, Texture2D magicalIcon, Texture2D statusIcon, 
            Texture2D buffIcon)
        {
            this.poisonIcon = poisonIcon;
            this.burnIcon = burnIcon;
            this.bleedIcon = bleedIcon;
            this.stunIcon = stunIcon;
            this.physicalIcon = physicalIcon;
            this.magicalIcon = magicalIcon;
            this.statusIcon = statusIcon;
            this.buffIcon = buffIcon;
        }

        public Texture2D IconTexture(int icon)
        {
            switch(icon)
            {
                case StatusEffect.Poison:
                    return poisonIcon;
                case StatusEffect.Burn:
                    return burnIcon;
                case StatusEffect.Bleed:
                    return bleedIcon;
                case StatusEffect.Stun:
                    return stunIcon;
            }

            return poisonIcon;
        }

        public Texture2D SkillTypeIcon(Skill.SkillType icon)
        {
            switch(icon)
            {
                case Skill.SkillType.Physical:
                    return physicalIcon;
                case Skill.SkillType.Magical:
                    return magicalIcon;
                case Skill.SkillType.Effect:
                    return statusIcon;
                case Skill.SkillType.Buff:
                    return buffIcon;
            }

            return physicalIcon;
        }

    }
}
