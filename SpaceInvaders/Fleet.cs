using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /**
    Game object for managing multiple enemy ships.
    */
    class Fleet : GameObject
    {
        int x;
        int y;
        int Rows;
        int Columns;
        int Direction;
        EnemyShip[][] EnemyShips;

        public Fleet(int _x, int _y, int _Rows, int _Columns) : base()
        {
            Direction = 1;
            Alive = true;
            ObjectType = eType.Enemy;
            x = _x;
            y = _y;
            Rows = _Rows;
            Columns = _Columns;
            EnemyShips = new EnemyShip[Columns][];
            for(int i = 0; i<Columns; i++)
            {
                EnemyShips[i] = new EnemyShip[Rows];
                for(int j = 0; j<Rows; j++)
                {
                    EnemyShips[i][j] = new EnemyShip(x + Engine.Width/25 * i, y + Engine.Width/25 * j, Engine.Width/50, Engine.Width/50);
                }
            }
            for(int i = 0; i<Columns; i++)
            {
                EnemyShips[i][Rows-1].CanFire = true;
            }
        }

        public List<GameObject> GetShips()
        {
            List<GameObject> res = new List<GameObject>();
            foreach (EnemyShip[] row in EnemyShips)
            {
                foreach (EnemyShip enemyShip in row)
                {
                    res.Add(enemyShip);
                }
            }
            return res;
        }

        public override void Tick(Engine sender)
        {
            int left = Engine.Width, right = 0;
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (EnemyShips[i][j].Alive)
                    {
                        left = Math.Min(left, x + i * Engine.Width/25);
                        right = Math.Max(right, x + i * Engine.Width/25 + Engine.Width/50);
                    }
                }
            }
            if (left<0 || right > Engine.Width) Direction *= -1;
            foreach (EnemyShip[] row in EnemyShips)
            {
                foreach (EnemyShip enemyShip in row)
                {
                    enemyShip.Move(Direction*20,0);
                }
            }
            x += Direction*20;

            //Set the lowest living ships to fire
            for(int i = 0; i<Columns; i++)
            {
                for(int j = Rows-1; j>=0; j--)
                {
                    if (EnemyShips[i][j].Alive)
                    {
                        EnemyShips[i][j].CanFire = true;
                        break;
                    }
                    else
                    {
                        EnemyShips[i][j].CanFire = false;
                    }
                }
            }
        }

        public bool IsDead()
        {
            foreach (EnemyShip[] row in EnemyShips)
            {
                foreach (EnemyShip enemyShip in row)
                {
                    if (enemyShip.Alive) return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            Direction = 1;
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    EnemyShips[i][j].SetPosition(x + Engine.Width/25 * i, y + Engine.Width/25 * j);
                    EnemyShips[i][j].CanFire = false;
                    EnemyShips[i][j].Alive = true;
                }
            }
            for (int i = 0; i < Columns; i++)
            {
                EnemyShips[i][Rows - 1].CanFire = true;
            }
        }

        //public override void Render(Graphics g)
        //{
        //    foreach (EnemyShip[] row in EnemyShips)
        //    {
        //        foreach (EnemyShip enemyShip in row)
        //        {
        //            enemyShip.Render(g);
        //        }
        //    }
        //}

        //public override bool Collides(GameObject other)
        //{
        //    foreach (EnemyShip[] row in EnemyShips)
        //    {
        //        foreach (EnemyShip enemyShip in row)
        //        {
        //            if (enemyShip.Collides(other)) return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
