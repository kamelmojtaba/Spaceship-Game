using GameAlgoT2310;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04
{
    public class Spaceship : GameObject
    {
        public Texture2D Texture;
        public float Speed;
        public Vector2 Velocity;

        //Shooting
        public float LastFireTime;
        public float CoolingPeriod;
        public float FiringRate;

        public Spaceship(string name) : base(name)
        {

        }

        public override void Initialize()
        {
            LoadContent();

            Origin.X = Texture.Width / 2f;
            Origin.Y = Texture.Height / 2f;
            Position.X = _game.Graphics.PreferredBackBufferWidth / 2f;
            Position.Y = _game.Graphics.PreferredBackBufferHeight / 2f;
            Speed = 100f;

            //shooting
            LastFireTime = 0;
            FiringRate = 5; // 5 missiles per second 
            CoolingPeriod = 1/FiringRate;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("spaceShips_009_right");
        }

        public override void Update()
        {
            Velocity = Vector2.Zero;

            // mouse control 
            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();
            Vector2 direction = mousePosition - Position;
            Orientation = (float)Math.Atan2(direction.Y, direction.X);



            // Vertical directions
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Velocity += -Vector2.UnitY;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Velocity += Vector2.UnitY;
            }
            // Horizontal directions
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Velocity += -Vector2.UnitX;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Velocity += Vector2.UnitX;
            }

            if (Velocity.Length() > 0.0001f)
            {
                Velocity = Vector2.Normalize(Velocity) * Speed;
            }
            shoot();
            Position += Velocity * ScalableGameTime.DeltaTime;
        }

        public override void Draw()
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Texture, Position, null, Color.White
                                  , Orientation, Origin, Scale, SpriteEffects.None
                                  , 0f);
            _game.SpriteBatch.End();
        }

        private void shoot()
        {
            // Shoot if left mouse button pressed
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if(LastFireTime + CoolingPeriod <= ScalableGameTime.RealTime)
                {
                    Vector2 temp = new Vector2(Position.X, Position.Y + 1);
                    Missile missile = new Missile(this, temp);
                    missile.Initialize();

                    LastFireTime = ScalableGameTime.RealTime;
                }
            }
        }
    }
}
