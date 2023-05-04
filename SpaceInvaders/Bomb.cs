using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /**
    Projectiles fired by the enemies
    */
    class Bomb : GameObject
    {
        int Speed = Engine.Height / 60;

        /**
        Constructors
        */
        public Bomb() : base()
        {
            Alive = true;
            ObjectType = eType.Enemy;
            SetSprite(new Bitmap("../../Assets/Bomb.bmp"));
        }
        public Bomb(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Alive = true;
            ObjectType = eType.Enemy;
            SetSprite(new Bitmap("../../Assets/Bomb.bmp"));
        }

        public override void OnCollide(GameObject other)
        {
            if (other.ObjectType == eType.Mothership || other.ObjectType == eType.Missile)
            {
                Alive = false;
            }
        }

        //Updates the missiles state by one step
        public override void Tick(Engine sender)
        {
            if (!Alive) return;

            //Move Down
            Move(0, Speed);

            //Set the bomb to dead if it's off screen
            if (Transform.Y > Engine.Height)
            {
                Alive = false;
            }
        }
    }
}
