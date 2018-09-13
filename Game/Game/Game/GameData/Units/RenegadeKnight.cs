using Game.GameData.Skills;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameData.Units
{
    class RenegadeKnight : Unit
    {
        public RenegadeKnight(Texture2D texture)
            : base(texture)
        {
            // Initialize level, stats, and skills here
            level = 1;

            stats = new Stats(50, 50, 50, 50, 50, 50);

            skillList.Add(new Strength(3));
            skillList.Add(new Deadly_Bite(3));
        }
    }
}
