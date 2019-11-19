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

        public IconManager(Texture2D poisonIcon, Texture2D burnIcon, Texture2D bleedIcon)
        {
            this.poisonIcon = poisonIcon;
            this.burnIcon = burnIcon;
            this.bleedIcon = bleedIcon;
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
            }

            return poisonIcon;
        }

    }
}
