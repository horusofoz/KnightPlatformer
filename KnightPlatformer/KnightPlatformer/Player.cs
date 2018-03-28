﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightPlatformer
{
    class Player
    {
        public Sprite playerSprite = new Sprite();

        Game1 game = null;
        float runSpeed = 15000;

        public Player()
        {
        }

        public void Load(ContentManager content, Game1 theGame)
        {
            playerSprite.Load(content, "hero");

            game = theGame;

            playerSprite.velocity = Vector2.Zero;
            playerSprite.position = new Vector2(theGame.GraphicsDevice.Viewport.Width / 2, 0);
        }

        public void Update(float deltaTime)
        {
            UpdateInput(deltaTime);
            playerSprite.Update(deltaTime);
        }

        private void UpdateInput(float deltaTime)
        {
            Vector2 localAcceleration = new Vector2(0, 0);

            if(Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                localAcceleration.X = -runSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                localAcceleration.X = runSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true)
            {
                localAcceleration.Y = -runSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) == true)
            {
                localAcceleration.Y = runSpeed;
            }

            playerSprite.velocity = localAcceleration * deltaTime;
            playerSprite.position += playerSprite.velocity * deltaTime;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            
            playerSprite.Draw(spriteBatch);
        }
    }
}
