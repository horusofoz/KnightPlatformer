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
        Collision collision = new Collision();
        float runSpeed = 15000;

        public Player()
        {
        }

        public void Load(ContentManager content, Game1 theGame)
        {
            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animation.Load(content, "sprites/walk", 12, 20);
            playerSprite.Add(animation, 0, -5);

            game = theGame;

            playerSprite.velocity = Vector2.Zero;
            playerSprite.position = new Vector2(theGame.GraphicsDevice.Viewport.Width / 2, 0);
        }

        public void Update(float deltaTime)
        {
            UpdateInput(deltaTime);
            playerSprite.Update(deltaTime);
            playerSprite.UpdateHitBox();
        }

        private void UpdateInput(float deltaTime)
        {
            Vector2 localAcceleration = new Vector2(0, 0);

            if(Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                localAcceleration.X = -runSpeed;
                playerSprite.SetFlipped(true);
                playerSprite.Play();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                localAcceleration.X = runSpeed;
                playerSprite.SetFlipped(false);
                playerSprite.Play();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true)
            {
                localAcceleration.Y = -runSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) == true)
            {
                localAcceleration.Y = runSpeed;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Left) == true && Keyboard.GetState().IsKeyUp(Keys.Right) == true)
            {
                playerSprite.Pause();
            }

            playerSprite.velocity = localAcceleration * deltaTime;
            playerSprite.position += playerSprite.velocity * deltaTime;

            collision.game = game;
            playerSprite = collision.CollidewithPlatforms(playerSprite, deltaTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            
            playerSprite.Draw(spriteBatch);
        }
    }
}
