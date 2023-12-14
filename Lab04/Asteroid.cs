using GameAlgoT2310;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Lab04
{
    public class Asteroid : GameObject, ICollidable
    {

        public Texture2D Texture;
        public float Speed;
        public Vector2 Velocity;
        public float Rotation;
        public Rectangle Bound;
        public SoundEffect ExplosionSoundEffect;
        public Asteroid() : base()
        {
        }
        public override void Initialize()
        {
            LoadContent();

            Origin.X = Texture.Width / 2; Origin.Y = Texture.Height / 2;
            Vector2 windowCentre = new Vector2();
            windowCentre.X = _game.Graphics.PreferredBackBufferWidth / 2f;
            windowCentre.Y = _game.Graphics.PreferredBackBufferHeight / 2f;

            Vector2 displacement = ComputeRandomDisplacement();
            Position = windowCentre + displacement;


            Speed = 100f;
            Position = ComputeRandomPosition();
            Rotation = 2f;

            // Specify collision to detect
            _game.CollisionEngine.Listen(typeof(Asteroid), typeof(Missile)
                                        , CollisionEngine.AABB);

            Bound.Location = (Position - Origin).ToPoint();
            Bound.Width = Texture.Width;
            Bound.Height = Texture.Height;

        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("spaceMeteors_002_small");
            ExplosionSoundEffect = _game.Content.Load<SoundEffect>("enemyExplosion");

        }
        public override void Update()
        {
            Position += Velocity * ScalableGameTime.DeltaTime;
            Orientation += Rotation * ScalableGameTime.DeltaTime;

        }
        public override void Draw()
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Texture, Position, null, Color.White
                                  , Orientation, Origin, Scale, SpriteEffects.None
                                  , 0f);
            _game.SpriteBatch.End();
        }

        private Vector2 ComputeRandomPosition()
        {
            // Find a Random direction
            float x = _game.Random.NextSingle() * 2f -1f;
            float y = _game.Random.NextSingle() * 2f - 1f;
            Vector2 center = new Vector2()
            {
                X = _game.Graphics.PreferredBackBufferWidth / 2,
                Y = _game.Graphics.PreferredBackBufferHeight / 2,
            };
            Vector2 direction = new Vector2(x, y); 
            direction =_game.Graphics.PreferredBackBufferWidth* Vector2.Normalize(direction);

            // compute Velocity
            Velocity = Speed * -Vector2.Normalize(direction);

            return center + direction;
        }

        private Vector2 ComputeRandomDisplacement()
        {
            // Compute random direction
            float x = _game.Random.NextSingle() * 2f - 1f;
            float y = _game.Random.NextSingle() * 2f - 1f;
            Vector2 direction = Vector2.Normalize(new Vector2(x, y));

            // Compute distance from screen centre
            float minDistance = 1.0f * _game.Graphics.PreferredBackBufferWidth;
            float maxDistance = 1.5f * _game.Graphics.PreferredBackBufferWidth;
            float distance = _game.Random.NextSingle() * (maxDistance - minDistance) + minDistance;

            return distance * direction;
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
            if (collisionInfo.Other is Missile)
            {
                // Destroy this missile
                ExplosionSoundEffect.Play();
                GameObjectCollection.DeInstantiate(this);
            }
        }


    }

}

