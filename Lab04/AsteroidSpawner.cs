using GameAlgoT2310;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04
{
    public class AsteroidSpawner : GameObject
    {
        private float _lastSpawnTime;
        private float _coolDownSeconds;
        private int   _batchCount; // number of asteriods to spawn
                                   // per batch

        public AsteroidSpawner() : base()
        {
            _batchCount = 5;
        }

        public override void Initialize()
        {
            _lastSpawnTime = 0f;
            _coolDownSeconds = 3f;
        }

        public override void Update()
        {
            if (_lastSpawnTime + _coolDownSeconds <= ScalableGameTime.RealTime)
            {
                Asteroid[] asteroids = new Asteroid[_batchCount];
                for (int i = 0; i < asteroids.Length; i++)
                {
                    asteroids[i] = new Asteroid();
                    asteroids[i].Initialize();
                }

                // Update last spawn time
                _lastSpawnTime = ScalableGameTime.RealTime;
            }
        }
    }
}
