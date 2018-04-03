using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightPlatformer
{
    public class Sprite
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        //public Vector2 offset = Vector2.Zero;

        Texture2D texture;

        public Vector2 tileCoordinates = Vector2.Zero;

        public int width = 0;
        public int height = 0;

        public int leftEdge = 0;
        public int rightEdge = 0;
        public int topEdge = 0;
        public int bottomEdge = 0;

        public Vector2 offset;

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
            spriteBatch.Draw(texture, position - offset, Color.White);
        }
    }
}
