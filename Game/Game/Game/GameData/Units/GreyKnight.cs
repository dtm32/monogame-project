using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameData;
using Game.GameData.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.GameData.Units
{
    class GreyKnight : Unit
    {
        public GreyKnight(Texture2D texture)
            : base(texture)
        {
            // Initialize level, stats, and skills here
            level = 1;

            stats = new Stats(50, 50, 50, 50, 50, 50);

            skillList.Add(new Health(3));
            skillList.Add(new Steady_March(3));
        }
    }
}
