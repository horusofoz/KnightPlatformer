using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace KnightPlatformer
{
    public class Chest
    {
        public Sprite chestSprite = new Sprite();
        Collision collision = new Collision();
        Game1 game = null;

        public void Load(ContentManager content, Game1 theGame)
        {
            chestSprite.Load(content, "sprites/chest", true);
            game = theGame;
            chestSprite.velocity = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            chestSprite.Draw(spriteBatch);
        }

    }
}
