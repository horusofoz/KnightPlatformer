using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightPlatformer
{
    class Collision
    {
        public Game1 game;

        public bool IsColliding(Sprite hero, Sprite otherSprite)
        {
            if(hero.rightEdge < otherSprite.leftEdge ||
                hero.leftEdge > otherSprite.rightEdge ||
                hero.bottomEdge < otherSprite.topEdge ||
                hero.topEdge > otherSprite.bottomEdge)
            {
                return false;
            }
            return true;
        }
    }
}
