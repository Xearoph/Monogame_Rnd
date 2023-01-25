using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame
{
    internal class GameManager
    {
        // List of players blueprints, e.a. players containing texture, controls, collision
        List<Spaceship> players = new();

        Viewport viewport;
        ContentManager content;
        GameTime gameTime;

        public GameManager(Viewport pViewport, ContentManager pContent)
        {
            viewport = pViewport;
            content = pContent;
        }

        /// <summary>
        /// Generate players and adds them to the playerlist
        /// </summary>
        /// <param name="pNumberOfPlayers"></param>
        void AddPlayers(int pNumberOfPlayers)
        {
            System.Random random = new();
            int taggerId = random.Next(0, pNumberOfPlayers);

            for (int playerId = 0; playerId < pNumberOfPlayers; playerId++)
                players.Add(new Spaceship(viewport, playerId, (taggerId == playerId) ? true : false));

            // Load all players textures (Texture2D)
            players.ForEach(s => s.LoadContent(content));
        }

        public void Update(GameTime pGameTime)
        {
            gameTime = pGameTime;
            if (players.Count == 0)
                SetPlayers();
            else
                UpdatePlayers();
        }

        void UpdatePlayers()
        {

            // Loop through all players for movement and collision checks
            players.ForEach(s => s.UpdateMovement(gameTime));
            players.ForEach(a => players.ForEach(b => a.CollisionCheck(b)));
        }

        void SetPlayers()
        {
            KeyboardState pressedKey = Keyboard.GetState();
            Keys currentKey = (pressedKey.GetPressedKeys().Length > 0) ? pressedKey.GetPressedKeys()[0] : Keys.None;

            int keyToNumeric = ((int)currentKey) - 48;

            if (currentKey == Keys.None || keyToNumeric > 6) return;

            AddPlayers(keyToNumeric);
        }

        public void DrawPlayers(SpriteBatch pSpriteBatch)
        {
            players.ForEach(s => s.Draw(pSpriteBatch));
        }
    }
}
