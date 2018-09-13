using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameData.Skills;
using Microsoft.Xna.Framework.Graphics;

namespace Game.GameData.Units
{
    class Crimson_Knight : Unit
    {
        public Crimson_Knight(Texture2D unitTexture) : base(unitTexture)
        {
            // Initialize level, stats, and skills here
            level = 1;

            stats = new Stats(95, 20, 75, 10, 35, 30);

            skillList.Add(new Health(3));
            skillList.Add(new Steady_March(3));
        }
    }
}
