// These using statements make it easier to use the code that MonoGame has to offer.
// They are prefixed with Microsoft.Xna.Framework because MonoGame is an open source re-implementation of Microsoft's XNA framework, and in order to maintain compatibility with the XNA code, it uses the same namespaces. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoGame
{
    // The Game Class definition - the heart of any MonoGame project.
    // The main Game1 class inherits from the Game class, which provides all the core methods for your game (ie. Load/Unload Content, Update, Draw etc.). You usually only have one Game class per game, so its name is not that important.
    public class Game1 : Game
    {
        List<Spaceship> spaceship = new();

        // The two default variables that the blank template starts with are the GraphicsDeviceManager and SpriteBatch. Both of these variables that are used for drawing to the screen.
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // The Game constructor and key variables - which tell the project how to start.
        // The main game constructor is used to initialize the starting variables. In this case, a new GraphicsDeviceManager is created, and the root directory containing the game's content files is set.
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        // The Initialize method - to initialize the game upon its startup.
        // The Initialize method is called after the constructor but before the main game loop (Update/Draw). This is where you can query any required services and load any non-graphic related content.
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            AddPlayers(2);
            base.Initialize();
        }

        void AddPlayers(int pNumberOfPlayers)
        {
            for (int i = 0; i < pNumberOfPlayers; i++)
            {
                spaceship.Add(new Spaceship(_graphics.GraphicsDevice.Viewport, i));
            }
        }

        // The Load and Unload Content methods - which are used to add and remove assets from the running game from the Content project.
        // The LoadContent method is used to load your game content. It is called only once per game, within the Initialize method, before the main game loop starts.
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            spaceship.ForEach(s => s.LoadContent(Content));
            
        }

        // The Update method - which is called on a regular interval to update your game state, e.g. take player inputs, move ships, or animate entities.
        // The Update method is called multiple times per second, and it is used to update your game state (checking for collisions, gathering input, playing audio, etc.).
        int i = 0;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            spaceship.ForEach(s => s.UpdateMovement(gameTime));

            if (spaceship[0].CollisionCheck().IntersectsWith(spaceship[1].CollisionCheck()))
                spaceship[1].ToggleDeath();

            base.Update(gameTime);
        }

        // The Draw method - which is called on a regular interval to take the current game state and draw your game entities to the screen.
        // Similar to the Update method, the Draw method is also called multiple times per second. This, as the name suggests, is responsible for drawing content to the screen.
        protected override void Draw(GameTime gameTime)
        {
            Color background = new(4, 21, 28);
            GraphicsDevice.Clear(background);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            spaceship.ForEach(s => s.Draw(_spriteBatch));

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}