using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Menu
    {

        public delegate void Action(Engine sender);

        List<GUILabel> Items;
        List<Action> Actions;
        int CurrentItem;

        public Menu()
        {
            Items = new List<GUILabel>();
            Actions = new List<Action>();
            CurrentItem = 0;
        }

        public void Render(Graphics g)
        {
            foreach(GUILabel item in Items)
            {
                item.Render(g);
            }
            Rectangle cursor = Items[CurrentItem].Transform;
            cursor.X -= 100;
            cursor.Width = 100;
            //cursor.Height = 100;
            g.FillRectangle(Brushes.White, cursor);
        }

        public void KeyHandler(Keys k, Engine sender)
        {
            if (k == Keys.Up && CurrentItem > 0) CurrentItem--;
            else if (k == Keys.Up) CurrentItem = Items.Count - 1;

            if (k == Keys.Down) CurrentItem = (CurrentItem + 1) % Items.Count;

            if (k == Keys.Space) Actions[CurrentItem](sender);
        }

        public void Add(GUILabel l, Action a)
        {
            Items.Add(l);
            Actions.Add(a);
        }
    }
}
