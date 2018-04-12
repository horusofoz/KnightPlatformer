using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightPlatformer
{
    public class Player
    {
        public Sprite playerSprite = new Sprite();

        Game1 game = null;
        Collision collision = new Collision();
        float runSpeed = 250;
        float maxRunSpeed = 500;
        float friction = 500;
        float terminalVelocity = 500;
        public float jumpStrength = 50000;

        SoundEffect jumpSound;
        SoundEffectInstance jumpSoundInstance;

        public Player()
        {
        }

        public void Load(ContentManager content, Game1 theGame)
        {
            playerSprite.Load(content, "sprites/hero", true);
            AnimatedTexture animation = new AnimatedTexture(playerSprite.offset, 0, 1, 1);

            animation.Load(content, "sprites/walk", 12, 20);

            jumpSound = content.Load<SoundEffect>("sounds/Jump");
            jumpSoundInstance = jumpSound.CreateInstance();

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

            if (collision.IsColliding(playerSprite, game.goal.chestSprite))
            {
                game.Exit();
            }

            for(int i = 0; i < game.enemies.Count; i++)
            {
                playerSprite = collision.CollideWithMonster(this, game.enemies[i], deltaTime, game);
            }
        }

        private void UpdateInput(float deltaTime)
        {
            bool wasMovingLeft = playerSprite.velocity.X < 0;
            bool wasMovingRight = playerSprite.velocity.X > 0;

            Vector2 localAcceleration = game.gravity;

            if(Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                localAcceleration.X += -runSpeed;
                playerSprite.SetFlipped(true);
                playerSprite.Play();
            }
            else if(wasMovingLeft == true)
            {
                localAcceleration.X += friction;
                playerSprite.Pause();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                localAcceleration.X += runSpeed;
                playerSprite.SetFlipped(false);
                playerSprite.Play();
            }
            else if (wasMovingRight == true)
            {
                localAcceleration.X += -friction;
                playerSprite.Pause();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) == true && playerSprite.canJump == true)
            {
                playerSprite.canJump = false;
                localAcceleration.Y -= jumpStrength;
                jumpSoundInstance.Play();
            }

            playerSprite.velocity += localAcceleration * deltaTime;

            if (playerSprite.velocity.X > maxRunSpeed)
            {
                playerSprite.velocity.X = maxRunSpeed;
            }
            else if (playerSprite.velocity.X < -maxRunSpeed)
            {
                playerSprite.velocity.X = -maxRunSpeed;
            }

            if (wasMovingLeft && (playerSprite.velocity.X > 0) || wasMovingRight && (playerSprite.velocity.X < 0))
            {
                // clamp at zero to prevent friction from making us slide
                playerSprite.velocity.X = 0;
            }

            if (playerSprite.velocity.Y > terminalVelocity)
            {
                playerSprite.velocity.Y = terminalVelocity;
            }

            playerSprite.position += playerSprite.velocity * deltaTime;
            
            collision.game = game;
            playerSprite = collision.CollideWithPlatforms(playerSprite, deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            playerSprite.Draw(spriteBatch);
        }
    }
}
