using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class EnemyShip : GameObject
    {
        int FireChance = 100;

        public bool CanFire { get; set; }

        /**
        Default constructor.
        */
        public EnemyShip() : base()
        {
            ObjectType = eType.Enemy;
            CanFire = false;
        }

        /**
        Enemyship constructer.
        x: X position of the enemyship in screen space.
        y: Y position of the enemyship in screen space.
        width: Width of the enemyship in pixels.
        height: Height of the enemyship in pixels.
        _alive: Whether the ship is initially alive(default) or dead.
        */
        public EnemyShip(int x, int y, int width, int height, bool _alive = true) : base(x, y, width, height, _alive)
        {
            ObjectType = eType.Enemy;
            CanFire = false;
        }

        public override void Tick(Engine sender)
        {
            if (CanFire && Engine.rand.Next(FireChance) == 0)
            {
                sender.SpawnBomb(Transform.X, Transform.Y + Transform.Height);
                sender.PlaySound(Engine.eSound.Bomb);
            }
        }

        public override void OnCollide(GameObject other)
        {
            if(other.ObjectType == eType.Missile)
            {
                Alive = false;
            }
        }
    }
}
