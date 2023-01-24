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
        RectangleF collisionRectangle;
        public Vector2 shipPosition;
        
        Texture2D shipTexture;

        readonly float shipSpeed = 500f;
        readonly float shipTurnSpeed = 12f;
        float shipAngle = 0;
        float angleOffset = (float)(Math.PI / 180);

        bool isDeath = false;

        Vector2 viewportSize;

        Keys[] controls;

        float halfSize;
        Vector2 maxViewportSize;

        public Spaceship(Viewport pViewport, int id)
        {
            viewportSize = new Vector2(pViewport.Width, pViewport.Height);

            switch (id)
            {
                case 0:
                    controls = new Keys[] { Keys.W, Keys.S, Keys.D, Keys.A };
                    shipPosition = new Vector2(viewportSize.X * 0.1f, viewportSize.Y / 2);
                    break;
                case 1:
                    controls = new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left };
                    shipPosition = new Vector2(viewportSize.X * 0.9f, viewportSize.Y / 2);
                    shipAngle = angleOffset * 180;
                    break;
            }
        }

        public RectangleF CollisionCheck()
        {
            return collisionRectangle;
        }

        public void LoadContent(ContentManager pContent)
        {
            
            shipTexture = pContent.Load<Texture2D>("Spaceship");
            CalculateSizes();
        }

        void CalculateSizes()
        {
            halfSize = (shipTexture.Height > shipTexture.Width) ? shipTexture.Height / 2 : shipTexture.Width / 2;
            maxViewportSize = new(viewportSize.X - halfSize, viewportSize.Y - halfSize);
            collisionRectangle = new RectangleF(shipPosition.X, shipPosition.Y, shipTexture.Width, shipTexture.Height);
            collisionRectangle.Inflate(-3,-3);
        }

        public void ToggleDeath()
        {
            isDeath = true;
        }

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

            collisionRectangle.X = shipPosition.X = Math.Clamp(shipPositionX, halfSize, maxViewportSize.X);
            collisionRectangle.Y = shipPosition.Y = Math.Clamp(shipPositionY, halfSize, maxViewportSize.Y);
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            if (isDeath) return;
            pSpriteBatch.Draw(
                shipTexture, 
                shipPosition,
                null,
                Color.White, 
                shipAngle + angleOffset * 90, 
                new Vector2(shipTexture.Width / 2, shipTexture.Height / 2), 
                Vector2.One, 
                SpriteEffects.None, 
                0f);
        }
    }
}
