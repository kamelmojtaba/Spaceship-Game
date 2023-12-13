using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAlgoT2310;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Lab04
{

    public class Missile : GameObject
    {
        public Texture2D Texture;
        public GameObject Shooter;
        public float Speed;
        public Vector2 Velocity;

        public Rectangle Bound;
        public SoundEffect ExplosionSoundEffect;
        public Missile(Spaceship shooter, Vector2 startingPosition) : base()
        {
            Shooter = shooter;
            Position = startingPosition;
        }
        public override void Initialize()
        {
            LoadContent();

            Origin.X = Texture.Width/2; Origin.Y = Texture.Height/2;
            Orientation = Shooter.Orientation;
            Speed = 200f;
            Vector2 direction = new Vector2((float) Math.Cos(Orientation), (float) Math.Sin(Orientation));
            Velocity = Speed * direction;


            // Specify what collision to detect
            _game.CollisionEngine.Listen(typeof(Background), typeof(Missile)
                                        , CollisionEngine.NotAABB);

            // Specify the bound
            Bound.Width = Texture.Width;
            Bound.Height = Texture.Height;
            Bound.Location = (Position - Origin).ToPoint();
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("crosshair179");
            ExplosionSoundEffect = _game.Content.Load<SoundEffect>("enemyExplosion");
        }
        public override void Update()
        {
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

        public string GetGroupName()
        {
            return this.GetType().Name;
        }

        public Rectangle GetBound()
        {
            Bound.Location = (Position - Origin).ToPoint();
            return Bound;
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            if (collisionInfo.Other is Background)
            {
                // Destroy this missile
                ExplosionSoundEffect.Play();
                GameObjectCollection.DeInstantiate(this);
            }
            else if (collisionInfo.Other is Asteroid)
            {
                // Destroy this missile
                GameObjectCollection.DeInstantiate(this);
            }
        }



    }
}
