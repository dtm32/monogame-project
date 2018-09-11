using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Units
{
    class GreyKnight : Unit
    {
        public GreyKnight(Texture2D texture)
            : base(texture)
        {
            // Initialize level, stats, and skills here
            level = 1;

            stats = new Stats(50, 50, 50, 50, 50, 50);


        }
    }
}
