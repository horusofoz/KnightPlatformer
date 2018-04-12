using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KnightPlatformer
{
    public class Sprite
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 offset = Vector2.Zero;
        public Vector2 tileCoordinates = Vector2.Zero;
        public Texture2D texture;

        List<AnimatedTexture> animations = new List<AnimatedTexture>();
        List<Vector2> animationOffsets = new List<Vector2>();
        int currentAnimation = 0;

        SpriteEffects effects = SpriteEffects.None;

        public int width = 0;
        public int height = 0;

        public int leftEdge = 0;
        public int rightEdge = 0;
        public int topEdge = 0;
        public int bottomEdge = 0;

        public bool canJump = false;

        public Sprite()
        {
        }

        public void Load(ContentManager content, string asset, bool useOffset)
        {
            texture = content.Load<Texture2D>(asset);

            width = texture.Bounds.Width;
            height = texture.Bounds.Height;

            if(useOffset == true)
            {
                offset = new Vector2(leftEdge + width / 2, topEdge + height / 2);
            }

            UpdateHitBox();
        }

        public void Update(float deltaTime)
        {
            animations[currentAnimation].UpdateFrame(deltaTime);
        }

        public void UpdateHitBox()
        {
            leftEdge = (int)position.X - (int)offset.X;
            rightEdge = leftEdge + width;
            topEdge = (int)position.Y - (int)offset.Y;
            bottomEdge = topEdge + height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(animations.Count > 0)
            {
                animations[currentAnimation].DrawFrame(spriteBatch, position + animationOffsets[currentAnimation], effects);
            }
            else if(texture != null)
            {
                spriteBatch.Draw(texture, position - offset, Color.White);
            }
        }

        public void SetFlipped(bool state)
        {
            if(state == true)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effects = SpriteEffects.None;
            }
        }

        public void Pause()
        {
            animations[currentAnimation].Pause();
        }

        public void Play()
        {
            animations[currentAnimation].Play();
        }

        public void Add(AnimatedTexture animation, int xOffset = 0, int yOffset = 0)
        {
            animations.Add(animation);
            animationOffsets.Add(new Vector2(xOffset, yOffset));
        }


    }
}
