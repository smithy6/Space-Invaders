using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class GUILabel
    {
        private Rectangle transform;
        string Text;
        int Em;

        public Rectangle Transform
        {
            get
            {
                return transform;
            }

            set
            {
                transform = value;
            }
        }

        public GUILabel(string _Text, int x, int y, int width, int _Em)
        {
            Text = _Text;
            Em = _Em;
            Transform = new Rectangle(x, y, width, (int)(1.5 * Em));
        }

        public void Render(Graphics g)
        {
            g.DrawString(Text, new Font("sans", Em), Brushes.White, Transform);
        }
    }
}
