using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
    internal class Spaceship
    {
        // Custom variables
        public Texture2D shipTexture;

        Vector2 shipPosition;
        float shipSpeed;
        float shipTurnSpeed;
        float shipAngle;

        GraphicsDeviceManager _graphics;

        public SpriteBatch _spriteBatch;

        public Spaceship(GraphicsDeviceManager _graphics)
        {
            this._graphics = _graphics;
        }

        public void Initialize()
        {
            shipPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            shipSpeed = 150f;
            shipTurnSpeed = 3f;
        }

        public void LoadContent()
        {
            // _spriteBatch = new SpriteBatch(GraphicsDevice); // <- GraphicsDevice werkt niet, mogen er wel meerdere zijn?
            // shipTexture = Content.Load<Texture2D>("Spaceship"); // <- Content bestaat niet in huidige context
        }

        public void UpdateMovement(GameTime _gameTime)
        {
            float shipPositionX = shipPosition.X;
            float shipPositionY = shipPosition.Y;

            Matrix rotMatrixZ = Matrix.CreateRotationZ(shipAngle);

            KeyboardState kState = Keyboard.GetState();

            float speedMultiplier = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.W))
            {
                shipPositionX += rotMatrixZ.M12 * shipSpeed * speedMultiplier;
                shipPositionY -= rotMatrixZ.M11 * shipSpeed * speedMultiplier;
            }

            if (kState.IsKeyDown(Keys.S))
            {
                shipPositionX -= rotMatrixZ.M12 * shipSpeed * speedMultiplier;
                shipPositionY += rotMatrixZ.M11 * shipSpeed * speedMultiplier;
            }

            if (kState.IsKeyDown(Keys.D))
            {
                shipAngle += shipTurnSpeed * speedMultiplier;
            }

            if (kState.IsKeyDown(Keys.A))
            {
                shipAngle -= shipTurnSpeed * speedMultiplier;
            }


            float heighestSize = (shipTexture.Height > shipTexture.Width) ? shipTexture.Height : shipTexture.Width;
            float halfSize = heighestSize / 2;

            float maxSizeWidth = _graphics.PreferredBackBufferWidth - halfSize;
            float maxSizeHeight = _graphics.PreferredBackBufferHeight - halfSize;

            shipPosition.X = shipPositionX;
            shipPosition.Y = shipPositionY;

            shipPosition.X = (shipPosition.X > maxSizeWidth) ? maxSizeWidth : (shipPosition.X < halfSize) ? halfSize : shipPositionX;
            shipPosition.Y = (shipPosition.Y > maxSizeHeight) ? maxSizeHeight : (shipPosition.Y < halfSize) ? halfSize : shipPositionY;
        }

        public void Draw()
        {
            _spriteBatch.Draw(shipTexture, shipPosition, null, Color.White, shipAngle, new Vector2(shipTexture.Width / 2, shipTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
