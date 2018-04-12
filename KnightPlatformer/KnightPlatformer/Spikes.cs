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
    public class Spikes
    {
        Collision collision = new Collision();
        public Game1 game = null;

        public Sprite spikeSprite = new Sprite();


        public void Load(ContentManager content, Game1 theGame)
        {
            game = theGame;

            spikeSprite.Load(content, "sprites/chest", false);
            game = theGame;
            spikeSprite.velocity = Vector2.Zero;            
        }

        public void Update(float deltaTime)
        {
            // Check for a collision
            
            // If a colision is occurring...


            collision.game = game;

            if(collision.IsColliding(spikeSprite, game.player.playerSprite))
            {
                // do something
                game.lives--;
                game.player.playerSprite.position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, 0);
            }
        }

    }
}
