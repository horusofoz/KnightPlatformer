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
    public class Enemy
    {
        float walkSpeed = 7500;
        public Sprite enemySprite = new Sprite();
        Collision collision = new Collision();
        Game1 game = null;


        

        public void Update(float deltaTime)
        {
            // move the enemy        
            enemySprite.velocity = new Vector2(walkSpeed, 0) * deltaTime;
            enemySprite.position += enemySprite.velocity * deltaTime;
            // check for collisions
            collision.game = game;
            enemySprite = collision.CollideWithPlatforms(enemySprite, deltaTime);

            // if the enemy hits a wall, ie x velocity is 0, change direction
            if (enemySprite.velocity.X == 0)
            {
                walkSpeed *= -1;
            }

            enemySprite.UpdateHitBox();
        }

        public void Load(ContentManager content, Game1 theGame)
        {
            enemySprite.Load(content, "sprites/zombie", true);
            game = theGame;
            enemySprite.velocity = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            enemySprite.Draw(spriteBatch);
        }
    }
}
