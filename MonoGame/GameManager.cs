using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;

namespace MonoGame
{
    internal class GameManager
    {
        // List of players blueprints, e.a. players containing texture, controls, collision
        List<Spaceship> players = new();

        Viewport viewport;
        ContentManager content;

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
                players.Add(new Spaceship(viewport, playerId, taggerId == playerId)); // wot

            // Load all players textures (Texture2D)
            players.ForEach(player => player.LoadContent(content));
        }

        public void Update(GameTime pGameTime)
        {
            if (players.Count == 0)
                SetPlayers();
            else
                UpdatePlayers(pGameTime);
        }

        void UpdatePlayers(GameTime pGameTime)
        {
            // Loop through all players for movement and collision checks
            players.ForEach(s => s.UpdateMovement(pGameTime));


            players.ForEach(a => players.ForEach(b => a.CollisionCheck(b)));

            for (int i = 0; i < players.Count; i++)
                if (players[i].IsDeath())
                {
                    players.Remove(players[i]);
                    i--;
                }
        }

        public bool ResetGame()
        {
            return players.Count == 1;
        }

        void SetPlayers()
        {
            KeyboardState pressedKey = Keyboard.GetState();
            
            Keys currentKey = (pressedKey.GetPressedKeys().Length > 0) ? pressedKey.GetPressedKeys()[0] : Keys.None;

            int keyToNumeric = ((int)currentKey) - (int)Keys.D0;

            if (currentKey == Keys.None || keyToNumeric > 6 || keyToNumeric < 0) return;

            AddPlayers(keyToNumeric);

            // loop over all keys that are currently pressed and skip those who's value is "none". then calculate their numeric value and lastly select the first key's numeric value
            //int ke = (from key in Keyboard.GetState().GetPressedKeys() let sum = (int)key - (int)Keys.D0 select sum).FirstOrDefault();
        }

        public void DrawPlayers(SpriteBatch pSpriteBatch)
        {
            players.ForEach(player => player.Draw(pSpriteBatch));
        }
    }
}
