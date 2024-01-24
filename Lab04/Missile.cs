using System;
using GameAlgoT2310;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Lab04
{
    /*
     * The movement in this file is using lorenz attractors algorithme to give a motion that feels random but it’s actually a defined pattern 
     * The Link for more info about lorenz attractors : https://en.wikipedia.org/wiki/Lorenz_system
     * 
     */
    public class Missile : GameObject, ICollidable
    {
        public Texture2D Texture;
        public GameObject Shooter;
        public float Speed;
        public Vector2 Velocity;
        public Rectangle Bound;
        public SoundEffect ExplosionSoundEffect;

        // Lorenz Attractor parameters
        float a = 10.0f;
        float b = 28.0f;
        float c = 8.0f / 3.0f;
        float Z = 0.0f;

        public Vector2 IniPos;
        public Vector2 B2Pos;
        public Vector2 LIniPos;
        public Vector2 DePos;
        public Vector2 PreDePos;



        // Timer
        double timer = 0;
        double disappearTime = 5; // seconds
        public Missile(Spaceship shooter, Vector2 startingPosition) : base()
        {
            Shooter = shooter;
            Position = startingPosition;
        }
        public override void Initialize()
        {
            LoadContent();

            Origin.X = Texture.Width/2; Origin.Y = Texture.Height/2;
            IniPos = Position;
            B2Pos = new Vector2(0.04f,0.04f);
            LIniPos = Position - B2Pos ;
            PreDePos = Vector2.Zero;
            Orientation = Shooter.Orientation;
           
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
     
            LorenzMove();
            DeCode();
            FinalPos();

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

        private void LorenzMove()
        {
          
            // Update position using Lorenz attractor
            float dt = 0.006f;
                float dx = a * (B2Pos.Y - B2Pos.X) * dt;
                float dy = (B2Pos.X * (b - Z) - B2Pos.Y) * dt;
                float dz = (B2Pos.X * B2Pos.Y - c * Z) * dt;

                B2Pos.X += dx / 3f;
                B2Pos.Y += dy * 5f;    
                Z += dz / 3.0f; 
        }

        private void DeCode() 
        {
            DePos = B2Pos + LIniPos;
            Vector2 temp = PreDePos - DePos;
            if (-0.03f < temp.X && temp.X < 0.03f && -0.03f < temp.Y && temp.Y < 0.03f)
            {
                // Destroy this missile
                GameObjectCollection.DeInstantiate(this);
            }
            PreDePos = DePos;
        }

        private void FinalPos()
        {
            Vector2 defPos =  Shooter.Position - IniPos;
            Position = defPos + DePos ;
        }

    }
}
