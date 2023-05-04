using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /**
    Handles all the games functionality, rendering, updating, etc.
    */

    public sealed class Engine
    {
        private static Engine instance = null;

        public static Engine Instance
        {
            get
            {
                return instance;
            }
        }

        public static Random rand = new Random();
        public static int Width = 3840;
        public static int Height = 2160;

        public enum eState
        {
            MainMenu,
            Playing, 
            Win,
            Loss
        }

        public enum eSound
        {
            Fire,
            Explosion,
            Bomb
        }

        Graphics g; // Form rendering context 
        Bitmap backBuffer; //Backbuffer image
        Graphics backBufferGraphics; //Backbuffer rendering context
        List<GameObject> gameObjects; //List of all gameobjects
        Mothership mship; //Player character
        Fleet fleet;
        Missile[] missiles; //References to missiles in the resource pool
        List<Bomb> bombs;
        Menu main;
        bool paused = false;
        eState currentState;
        int count = 0;
        SoundPlayer[] soundEffects;
        BonusShip bonusShip;

        /**
        Constructor for the engine.
        _g: The context to render to.
        */
        public Engine(Graphics _g)
        {
            instance = this;

            g = _g;
            backBuffer = new Bitmap(Width, Height);
            backBufferGraphics = Graphics.FromImage(backBuffer);
            gameObjects = new List<GameObject>();
            soundEffects = new SoundPlayer[3];

            //Initialize the pool of missiles
            missiles = new Missile[15];
            for(int i = 0; i<15; i++)
            {
                missiles[i] = new Missile(0,0,Width/100, Width/50);
                gameObjects.Add(missiles[i]);
            }

            bombs = new List<Bomb>();

            //Test fleet
            fleet = new Fleet(Width/2, 0, 5, 5);
            gameObjects.Add(fleet);
            gameObjects.AddRange(fleet.GetShips());


            //Create the player character
            mship = new Mothership(Width/2 - Width/100, Height - Width/25 ,Width/25, Width/50);
            gameObjects.Add(mship);

            currentState = eState.MainMenu;

            main = new Menu();
            main.Add(new GUILabel("Start", Width/2, Height/2, Width/10, Height/50), new Menu.Action(e => e.SetState(eState.Playing)));
            main.Add(new GUILabel("Quit", Width/2, Height/2 + Height/25, Width/10, Height/50), new Menu.Action(e => e.Quit()));

            bonusShip = new BonusShip(-100, 0, 100, 100);
            gameObjects.Add(bonusShip);

            LoadSounds();
        }

        public void LoadSounds()
        {
            soundEffects[(int)eSound.Fire] = new SoundPlayer("C:/Windows/media/Windows Ding.wav");
            soundEffects[(int)eSound.Bomb] = new SoundPlayer("C:/Windows/media/Windows Ding.wav");
            soundEffects[(int)eSound.Explosion] = new SoundPlayer("C:/Windows/media/Windows Ding.wav");

            foreach (SoundPlayer sp in soundEffects)
            {
                sp.Load();
            }
        }

        public void Reset()
        {
            currentState = eState.MainMenu;
            mship.Alive = true;
            fleet.Reset();
            foreach(Bomb b in bombs)
            {
                b.Alive = false;
            }
            foreach(Missile m in missiles)
            {
                m.Alive = false;
            }
        }

        /**
        Advances the games state by one step.
        */
        public void Tick()
        {
            if (currentState == eState.Playing && !paused)
            {
                //Update each game object
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Tick(this);
                }
                CheckCollisions();

                if (!mship.Alive) currentState = eState.Loss;
                if (fleet.IsDead()) currentState = eState.Win;
            }
        }

        /**
        Checks all game objects pairwise for collisions.
        */
        public void CheckCollisions()
        {
            foreach(GameObject g1 in gameObjects)
            {
                foreach(GameObject g2 in gameObjects)
                {
                    if (g1.Equals(g2)) continue;

                    //If the objects are colliding call their collision handlers
                    if (g1.Collides(g2))
                    {
                        g1.OnCollide(g2);
                        g2.OnCollide(g1);
                    }
                }
            }
        }

        /**
        Process keyboard input.
        k: The keycode of the input key.
        */
        public void KeyHandler(Keys k)
        {
            if (currentState == eState.Playing && k == Keys.Space) paused = !paused;

            if (currentState == eState.MainMenu)
            {
                main.KeyHandler(k, this);
            }
            else if (currentState == eState.Playing && !paused)
            {
                //Pass the event information to the mothership
                mship.KeyHandler(k, this);
            }
        }
        /**
        Render and display the current game state
        */
        public void Render()
        {
            //Clear the backbuffer
            backBufferGraphics.Clear(Color.Black);

            if(currentState == eState.MainMenu)
            {
                //backBufferGraphics.DrawString("Press Space to Start", new Font("sans", 100),
                //                               Brushes.White, 0, 0);
                main.Render(backBufferGraphics);
            }

            else if (currentState == eState.Playing)
            {
                //Draw each gameobject
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Render(backBufferGraphics);
                }
                if (paused)
                {
                    backBufferGraphics.DrawString("Paused", new Font("sans", Width/50), Brushes.White, 0, 0);
                }
            }

            else if (currentState == eState.Win)
            {
                backBufferGraphics.DrawString("You Win", new Font("sans", Width/50),
                                               Brushes.White, Width/2 - 3.5f*Width/50, Height/2 - Width/50);
                count++;
                if (count > 180)
                {
                    count = 0;
                    Reset();
                }
            }

            else if (currentState == eState.Loss)
            {
                backBufferGraphics.DrawString("You Lose", new Font("sans", Width / 50),
                                               Brushes.White, Width / 2 - 3.5f*Width/50, Height / 2 - Width / 50);
                count++;
                if (count > 180)
                {
                    count = 0;
                    Reset();
                }
            }
            //Draw the backbuffer to the front buffer
            g.DrawImage(backBuffer, 0, 0, 1024, 576);
        }

        public void SpawnBomb(int x, int y)
        {
            foreach(Bomb bomb in bombs)
            {
                if (!bomb.Alive)
                {
                    bomb.Alive = true;
                    bomb.SetPosition(x, y);
                    return;
                }
            }
            Bomb b = new Bomb(x, y, Width/100, Width/50);
            bombs.Add(b);
            gameObjects.Add(b);
        }

        public void SpawnMissile(int x, int y)
        {
            //Consider each missile in the pool
            foreach (Missile missile in missiles)
            {
                //If it is no longer alive claim it
                if (!missile.Alive)
                {
                    missile.Alive = true;
                    missile.SetPosition(x, y);
                    break;
                }
            }
        }

        void SpawnBonusShip()
        {
            if (!bonusShip.Alive)
            {
                bonusShip.SetPosition(-100, 0);
                bonusShip.Alive = true;
            }
        }

        public void PlaySound(eSound sound)
        {
            soundEffects[(int)sound].Play();
        }

        public void SetState(eState newState)
        {
            currentState = newState;
        }

        public void Quit()
        {
            Application.Exit();
        }
    }
}
