using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

using RectangleF = System.Drawing.RectangleF;

namespace MonoGame
{
    internal class Spaceship
    {
        int playerId;

        Texture2D shipTexture;
        RectangleF shipCollision;
        Vector2 shipPosition;

        Texture2D shipWeaponTexture;
        string shipWeaponTextureName;

        readonly float shipSpeed = 500f;
        readonly float shipTurnSpeed = 12f;
        float shipAngle = 0;
        float angleOffset = (float)(Math.PI / 180);


        Vector2 viewportSize;

        Keys[] controls;

        float halfSize;
        Vector2 maxViewportSize;

        bool tagger;
        bool isDeath = false;

        public Spaceship(Viewport pViewport, int pPlayerId, bool pTagger = false)
        {
            viewportSize = new Vector2(pViewport.Width, pViewport.Height);
            playerId = pPlayerId;
            tagger = pTagger;

            switch (playerId)
            {
                case 0:
                    controls = new Keys[] { Keys.W, Keys.S, Keys.D, Keys.A };
                    shipPosition = new Vector2(viewportSize.X * 0.1f, viewportSize.Y / 2);
                    shipWeaponTextureName = "WeaponBlue";
                    break;
                case 1:
                    controls = new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left };
                    shipPosition = new Vector2(viewportSize.X * 0.9f, viewportSize.Y / 2);
                    shipAngle = angleOffset * 180;
                    shipWeaponTextureName = "WeaponRed";
                    break;
                case 2:
                    controls = new Keys[] { Keys.I, Keys.K, Keys.L, Keys.J };
                    shipPosition = new Vector2(viewportSize.X / 2, viewportSize.Y * 0.9f);
                    shipAngle = angleOffset * -90;
                    shipWeaponTextureName = "WeaponGreen";
                    break;
                case 3:
                    controls = new Keys[] { Keys.NumPad8, Keys.NumPad5, Keys.NumPad6, Keys.NumPad4 };
                    shipPosition = new Vector2(viewportSize.X / 2, viewportSize.Y * 0.1f);
                    shipAngle = angleOffset * 90;
                    shipWeaponTextureName = "WeaponPink";
                    break;
                case 4:
                    controls = new Keys[] { Keys.Home, Keys.End, Keys.PageDown, Keys.Delete };
                    shipPosition = new Vector2(viewportSize.X / 2, viewportSize.Y / 2);
                    shipAngle = angleOffset * 45;
                    shipWeaponTextureName = "WeaponOrange";
                    break;
            }
        }

        public void CollisionCheck(Spaceship other)
        {
            if (this == other || !tagger) return;

            if (shipCollision.IntersectsWith(other.Collider()))
                other.ToggleDeath();
        }

        public RectangleF Collider()
        {
            return shipCollision;
        }

        public void LoadContent(ContentManager pContent)
        {
            shipTexture = pContent.Load<Texture2D>("Spaceship");
            shipWeaponTexture = pContent.Load<Texture2D>(shipWeaponTextureName);
            CalculateSizes();
        }

        void CalculateSizes()
        {
            halfSize = (shipTexture.Height > shipTexture.Width) ? shipTexture.Height / 2 : shipTexture.Width / 2;
            maxViewportSize = new(viewportSize.X - halfSize, viewportSize.Y - halfSize);

            shipCollision = new RectangleF(shipPosition.X, shipPosition.Y, shipTexture.Width, shipTexture.Height);
            shipCollision.Inflate(-3, -3);
        }

        public void ToggleDeath() { isDeath = true; }

        public bool IsDeath() { return isDeath; }

        public void UpdateMovement(GameTime _gameTime)
        {
            float shipPositionX = shipPosition.X;
            float shipPositionY = shipPosition.Y;

            KeyboardState kState = Keyboard.GetState();

            float speedMultiplier = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            float shipXCosSpeed = (float)Math.Cos(shipAngle) * shipSpeed * speedMultiplier;
            float shipYSinSpeed = (float)Math.Sin(shipAngle) * shipSpeed * speedMultiplier;

            float shipTurnSpeedMultiplied = shipTurnSpeed * speedMultiplier;

            if (kState.IsKeyDown(controls[0]))
            {
                shipPositionX += shipXCosSpeed;
                shipPositionY += shipYSinSpeed;
            }

            if (kState.IsKeyDown(controls[1]))
            {
                shipPositionX -= shipXCosSpeed;
                shipPositionY -= shipYSinSpeed;
            }

            if (kState.IsKeyDown(controls[2]))
                shipAngle += shipTurnSpeedMultiplied;

            if (kState.IsKeyDown(controls[3]))
                shipAngle -= shipTurnSpeedMultiplied;

            shipCollision.X = shipPosition.X = Math.Clamp(shipPositionX, halfSize, maxViewportSize.X);
            shipCollision.Y = shipPosition.Y = Math.Clamp(shipPositionY, halfSize, maxViewportSize.Y);
        }



        public void Draw(SpriteBatch pSpriteBatch)
        {
            if (IsDeath()) return;

            Vector2 shipTextureOffset = new(shipTexture.Width / 2, shipTexture.Height / 2);
            Vector2 shipWeaponTextureOffset = new(shipWeaponTexture.Width / 2, shipWeaponTexture.Height + shipTexture.Height / 2);

            //Rectangle bok = new((int)shipPosition.X - shipTexture.Bounds.Width / 2, (int)shipPosition.Y - shipTexture.Bounds.Height / 2, 
            //    shipTexture.Bounds.Width, shipTexture.Bounds.Height);
            //pSpriteBatch.Draw(Game1.pixel, bok, Color.White);

            pSpriteBatch.Draw(
                shipTexture,
                shipPosition,
                null,
                Color.White,
                shipAngle + angleOffset * 90,
                shipTextureOffset,
                Vector2.One,
                SpriteEffects.None,
                0f);

            if (!tagger) return;
            pSpriteBatch.Draw(
                shipWeaponTexture,
                shipPosition,
                null,
                Color.White,
                shipAngle + angleOffset * 90,
                shipWeaponTextureOffset,
                Vector2.One,
                SpriteEffects.None,
                0f);
        }
    }
}
