using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAlgoT2310;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab04
{
    public class Background : GameObject
    {
        public Texture2D BackgroundTexture;
        public Rectangle BackgroundBounds;


        public Background(string name) : base(name) 
        {
        }

        public override void Initialize()
        {
            BackgroundTexture = _game.Content.Load<Texture2D>("purple");
            BackgroundBounds.Height = _game.Graphics.PreferredBackBufferHeight;
            BackgroundBounds.Width = _game.Graphics.PreferredBackBufferWidth;
        }

        public override void Draw()
        {
            _game.SpriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            _game.SpriteBatch.Draw(BackgroundTexture, Vector2.Zero, BackgroundBounds, Color.White);
            _game.SpriteBatch.End();
        }

    }
}
