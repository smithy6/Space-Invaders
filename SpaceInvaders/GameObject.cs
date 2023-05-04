using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /**
    Any object in the game engine
    */
    class GameObject
    {
        /**
        Types of GameObjects
        */
        public enum eType
        {
            Default,
            Mothership,
            Enemy,
            Missile
        }

        protected Rectangle Transform; //The objects position and size information
        public bool Alive { get; set; } //Life flag
        public eType ObjectType { get; set; } //Object type flag       
        Bitmap Sprite;

        /**
        Default constructor.
        _alive: Whether or not the object is initially alive or dead(default).
        */
        public GameObject(bool _alive = false)
        {
            Transform = new Rectangle(0, 0, 0, 0);
            Alive = _alive;
            ObjectType = eType.Default;
            SetSprite(new Bitmap("../../Assets/Default.bmp"));
        }

        /**
        Game Object constructer.
        x: X position of the object in screen space.
        y: Y position of the object in screen space.
        width: Width of the object in pixels.
        height: Height of the object in pixels.
        _alive: Whether the object is initially alive or dead(default).
        */
        public GameObject(int x, int y, int width, int height, bool _alive = false)
        {
            Transform = new Rectangle(x, y, width, height);
            Alive = _alive;
            ObjectType = eType.Default;
            SetSprite(new Bitmap("../../Assets/Default.bmp"));
        }

        /**
        Updates the gameobject by one step.
        */
        public virtual void Tick(Engine sender) { }

        /**
        Handles the objects behaviour on collision.
        */
        public virtual void OnCollide(GameObject other) { }

        /**
        Returns whether or not two objects are alive and colliding.
        other: The object to check collision with.
        */
        public bool Collides(GameObject other)
        {
            return Alive && other.Alive && Transform.IntersectsWith(other.Transform);
        }

        /**
        Renders the object to a given context.
        g: the context to render to.
        */
        public void Render(Graphics g)
        {
            if (!Alive) return;

            g.DrawImage(Sprite, Transform);
        }

        /**
        Move the object by a discrete number of pixels.
        dx: number of pixels to move in the x direction.
        dy: the number of pixels to move in the y direction.
        */
        public void Move(int dx, int dy)
        {
            Transform.X += dx;
            Transform.Y += dy;
        }

        /**
        Sets the objects position.
        x: The objects new x position in screen space.
        y: the objects new y position in screen space.
        */
        public void SetPosition(int x, int y)
        {
            Transform.X = x;
            Transform.Y = y;
        }

        public void SetSprite(Bitmap _Sprite)
        {
            Sprite = _Sprite;
            Sprite.MakeTransparent(Color.FromArgb(105,105,0));
        }
    }
}
