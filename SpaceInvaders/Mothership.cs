using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /**
    The player character.
    */
    class Mothership : GameObject
    {
        int Speed = Engine.Width/120;

        /**
        Default constructor.
        */
        public Mothership() : base()
        {
            ObjectType = eType.Mothership;
            SetSprite(new Bitmap("../../Assets/Mothership.bmp"));
        }

        /**
        Mothership constructer.
        x: X position of the mothership in screen space.
        y: Y position of the mothership in screen space.
        width: Width of the mothership in pixels.
        height: Height of the mothership in pixels.
        _missiles: List of references to pooled missiles.
        _alive: Whether the ship is initially alive(default) or dead.
        */
        public Mothership(int x, int y, int width, int height, bool _alive = true) : base(x, y, width, height, _alive)
        {
            ObjectType = eType.Mothership;
            SetSprite(new Bitmap("../../Assets/Mothership.bmp"));
        }

        /**
        Keyboard input handler.
        k: the input keycode
        */
        public void KeyHandler(Keys k, Engine sender)
        {
            //Move left
            if(k == Keys.Left)
            {
                Move(-1* Math.Min(Speed,Transform.X), 0);
            }

            //Move Right
            if(k == Keys.Right)
            {
                Move(Math.Min(Speed, Engine.Width-(Transform.X+Transform.Width)), 0);
            }

            //Fire
            if(k == Keys.Up)
            {
                Fire(sender);
            }
        }

        /**
        Spawns a missile from the resource pool above the mothership.
        */
        void Fire(Engine sender)
        {
            sender.PlaySound(Engine.eSound.Fire);
            sender.SpawnMissile(Transform.X+Transform.Width/2 - Engine.Width/200, Transform.Y - Transform.Height);
        }

        public override void OnCollide(GameObject other)
        {
            if(other.ObjectType == eType.Enemy)
            {
                Alive = false;
            }
        }
    }
}
