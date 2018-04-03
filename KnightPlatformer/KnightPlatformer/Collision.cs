using Microsoft.Xna.Framework;
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

        public Sprite CollidewithPlatforms(Sprite hero, float deltaTime)
        {
            // Create a copy of the hero that will move to where the hero will be next frame, to PREDICT if the hero will overlap an obstacle
            Sprite playerPrediction = new Sprite();
            playerPrediction.position = hero.position;
            playerPrediction.width = hero.width;
            playerPrediction.height = hero.height;
            playerPrediction.offset = hero.offset;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.velocity * deltaTime;

            int playerColumn = (int)playerPrediction.position.X / game.tileHeight;
            int playerRow = (int)playerPrediction.position.Y / game.tileHeight;
            Vector2 playerTile = new Vector2(playerColumn, playerRow);

            Vector2 leftTile = new Vector2(playerTile.X - 1, playerTile.Y);
            Vector2 rightTile = new Vector2(playerTile.X + 1, playerTile.Y);
            Vector2 topTile = new Vector2(playerTile.X, playerTile.Y - 1);
            Vector2 bottomTile = new Vector2(playerTile.X, playerTile.Y + 1);

            Vector2 bottomLeftTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            Vector2 bottomRightTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            Vector2 topLeftTile = new Vector2(playerTile.X - 1, playerTile.Y - 1);
            Vector2 topRightTile = new Vector2(playerTile.X - 1, playerTile.Y - 1);

            bool leftCheck = CheckForTile(leftTile);
            bool rightCheck = CheckForTile(rightTile);
            bool topCheck = CheckForTile(topTile);
            bool bottomCheck = CheckForTile(bottomTile);

            bool bottomLeftCheck = CheckForTile(bottomLeftTile);
            bool bottomRightCheck = CheckForTile(bottomRightTile);
            bool topLeftCheck = CheckForTile(topLeftTile);
            bool topRightCheck = CheckForTile(topRightTile);

            // Check for collisions with tiles left of the player
            if(leftCheck == true)
            {
                hero = CollideLeft(hero, leftTile, playerPrediction);
            }
            if (rightCheck == true)
            {
                hero = CollideRight(hero, rightTile, playerPrediction);
            }
            if (bottomCheck == true)
            {
                hero = CollideBelow(hero, bottomTile, playerPrediction);
            }
            if (topCheck == true)
            {
                hero = CollideAbove(hero, topTile, playerPrediction);
            }

            if(leftCheck == false && bottomCheck == false && bottomLeftCheck == true)
            {
                hero = CollideBottomDiagonals(hero, bottomLeftTile, playerPrediction);
            }
            if(rightCheck == false && bottomCheck == false && bottomRightCheck == true)
            {
                hero = CollideBottomDiagonals(hero, bottomRightTile, playerPrediction);
            }
            if(leftCheck == false && topCheck == false && topLeftCheck == true)
            {
                hero = CollideAboveDiagonals(hero, topLeftTile, playerPrediction);
            }
            if(rightCheck == false && topCheck == false && topRightCheck == true)
            {
                hero = CollideAboveDiagonals(hero, topRightTile, playerPrediction);
            }

            return hero;
        }

        // Checks if there is a tile at the specified coordinates
        bool CheckForTile(Vector2 coordinates)
        {
            int column = (int)coordinates.X;
            int row = (int)coordinates.Y;

            if(column < 0 || column > game.levelTileWidth - 1)
            {
                return false;
            }
            if (row < 0 || row > game.levelTileHeight - 1)
            {
                return false;
            }

            Sprite temp = game.levelGrid[column, row];

            if(temp != null)
            {
                return true;
            }

            return false;
        }

        Sprite CollideLeft(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if(IsColliding(playerPrediction, tile) == true)
            {
                hero.position.X = tile.rightEdge + hero.offset.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        Sprite CollideRight(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if(IsColliding(playerPrediction, tile) == true)
            {
                hero.position.X = tile.leftEdge - hero.offset.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        Sprite CollideAbove(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true)
            {
                hero.position.Y = tile.bottomEdge + hero.offset.Y;
                hero.velocity.Y = 0;
            }

            return hero;
        }

        Sprite CollideBelow(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if(IsColliding(playerPrediction, tile) == true)
            {
                hero.position.Y = tile.topEdge - hero.offset.Y;
                hero.velocity.Y = 0;
            }

            return hero;
        }

        Sprite CollideBottomDiagonals(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int topEdgeDistance = Math.Abs(tile.topEdge - playerPrediction.bottomEdge);

            if(IsColliding(playerPrediction, tile) == true)
            {
                if(topEdgeDistance < rightEdgeDistance && topEdgeDistance < leftEdgeDistance)
                {
                    // If top edge closest, collision is happening above the platform
                    hero.position.Y = tile.topEdge - hero.offset.Y;
                    hero.velocity.Y = 0;
                }
                else if(rightEdgeDistance < leftEdgeDistance)
                {
                    // if right edge closest, collision is happening to the right of the platform
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    hero.velocity.X = 0;
                }
                else
                {
                    // else if left edge closest, collision happening left of the platform
                    hero.position.X = tile.leftEdge - hero.offset.X;
                    hero.velocity.X = 0;
                }
            }

            return hero;
        }

        Sprite CollideAboveDiagonals(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int bottomEdgeDistance = Math.Abs(tile.bottomEdge - playerPrediction.topEdge);

            if (IsColliding(playerPrediction, tile) == true)
            {
                if (bottomEdgeDistance < rightEdgeDistance && bottomEdgeDistance < leftEdgeDistance)
                {
                    // If top edge closest and overlapping on top edge
                    hero.position.Y = tile.topEdge - hero.offset.Y;
                    hero.velocity.Y = 0;
                }
                else if (rightEdgeDistance < leftEdgeDistance)
                {
                    // else if the left edge closes and overlapping on left edge
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    hero.velocity.X = 0;
                }
                else
                {
                    // else if right edge closest and overlapping on the right edge
                    hero.position.X = tile.leftEdge - hero.offset.X;
                    hero.velocity.X = 0;
                }
            }

            return hero;
        }
    }
}
