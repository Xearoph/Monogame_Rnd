﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GameManager gameManager;

        public Game1()
        {
            // Added starting resolution (720p)
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameManager = new(_graphics.GraphicsDevice.Viewport, Content);
            base.Initialize();
        }

        public static Texture2D pixel;
        protected override void LoadContent()
        {
            pixel = new(GraphicsDevice, 1, 1);
            Color[] pixelData = new Color[1] { new Color(255, 0, 0, 80) };
            pixel.SetData(pixelData);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameManager.Update(gameTime);

            // Reset game if there are no players
            if (gameManager.ResetGame()) gameManager = new(_graphics.GraphicsDevice.Viewport, Content);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color background = new(4, 21, 28);
            GraphicsDevice.Clear(background);

            _spriteBatch.Begin();

            // Draw all players sprites
            gameManager.DrawPlayers(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}