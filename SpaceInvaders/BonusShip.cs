using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class BonusShip : GameObject
    {
        int speed = 10;

        public BonusShip() : base()
        {
            ObjectType = eType.Enemy;
        }

        public BonusShip(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            ObjectType = eType.Enemy;
        }

        public override void OnCollide(GameObject other)
        {
            if(other.ObjectType == eType.Missile)
            {
                //TODO: increase score
            }
        }

        public override void Tick(Engine sender)
        {
            Move(speed, 0);

            if (Transform.X > Engine.Width)
            {
                Alive = false;
            }
        }
    }
}
