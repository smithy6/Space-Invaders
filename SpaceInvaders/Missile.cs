using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /**
    Projectiles fired by the player
    */
    class Missile : GameObject
    {
        int Speed = -Engine.Height / 60;

        /**
        Constructors
        */
        public Missile() : base()
        {
            ObjectType = eType.Missile;
            SetSprite(new Bitmap("../../Assets/Missile.bmp"));
        }
        public Missile(int x, int y, int width, int height) : base(x, y, width, height)
        {
            ObjectType = eType.Missile;
            SetSprite(new Bitmap("../../Assets/Missile.bmp"));
        }

        public override void OnCollide(GameObject other)
        {
            if (other.ObjectType == eType.Enemy)
            {
                Alive = false;
                Engine.Instance.PlaySound(Engine.eSound.Explosion);
            }
        }

        //Updates the missiles state by one step
        public override void Tick(Engine sender)
        {
            if (!Alive) return;

            //Move up
            Move(0, Speed);

            //Set the missile to dead if it's off screen
            if (Transform.Y < -1*Transform.Height)
            {
                Alive = false;
            }
        }
    }
}
