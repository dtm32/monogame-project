using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameData.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.GameData.Units
{
    class Dark_Knight : Unit
    {
        public Dark_Knight(Texture2D unitTexture) : base(unitTexture)
        {
            // Initialize level, stats, and skills here
            level = 1;

            stats = new Stats(84, 15, 65, 5, 45, 40);

            skillList.Add(new Health(3));
            skillList.Add(new Strength(3));
            skillList.Add(new Steady_March(3));
        }
    }
}
