using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Herbert
{
    class Player : Sprite
    {
        double timer = 0;
        private double deltaTime;

        public Rectangle BulletRec;
        public Rectangle EnemyRec;

        public static Vector2 Posistion;

        public Random r = new Random();
        public int rInt;

        const string PLAYER_ASSETNAME = "PlayerG";
        public const int START_POSITION_X = 125;
        public const int START_POSITION_Y = 245;
        public const int PLAYER_SPEED = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        double FireDelayTime = 200;
        double FireDelay;
        bool MayFire;
        double SpawnDelayTime = 1000;
        double SpawnDelay;
        bool MaySpawn;
        int MaxEnemy = 20;

        enum State
        {
            Flying,
            Boosting,
            Death,
            Shield
        }
        State mCurrentState = State.Flying;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        List<Bullet> mBullets = new List<Bullet>();
        List<Enemy> mEnemies = new List<Enemy>();

        KeyboardState mPreviousKeyboardState;

        Vector2 mStartingPosition = Vector2.Zero;

        ContentManager mContentManager;

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Bullet aBullet in mBullets)
            {
                aBullet.LoadContent(theContentManager);
            }

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, PLAYER_ASSETNAME);
            Source = new Rectangle(0, 0, Source.Width, Source.Height);
        }
        public void Update(GameTime theGameTime)
        {

            deltaTime = theGameTime.ElapsedGameTime.TotalMilliseconds;
            timer += deltaTime;

            
            rInt = r.Next(-50, 1200);
            
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
           

            UpdateMovement(aCurrentKeyboardState);
            UpdateBullet(theGameTime, aCurrentKeyboardState);
            UpdateEnemy(theGameTime, aCurrentKeyboardState);
            UpdateHit();

            CanFireTimer(deltaTime);
            MaySpawnTimer(deltaTime);

            mPreviousKeyboardState = aCurrentKeyboardState;

            

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateEnemy(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (Enemy aEnemy in mEnemies)
            {
                aEnemy.Update(theGameTime);
               
            }
            if (MaySpawn == true && mEnemies.Count <= MaxEnemy)
            {
                SpawnEnemy(rInt);
                SpawnDelay = SpawnDelayTime;
            }
        }
        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            
            if (mCurrentState == State.Flying)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    mSpeed.Y = PLAYER_SPEED;
                    mDirection.Y = MOVE_UP;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    mSpeed.Y = PLAYER_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
            }
        }
        private void UpdateBullet(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (Bullet aBullet in mBullets)
            {
                aBullet.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.A) == true && MayFire == true)
            {
                ShootBullet();
                FireDelay = FireDelayTime;
            }
        }
        private void SpawnEnemy(int rInt)
        {
            
            Enemy aEnemy = new Enemy();
            aEnemy.LoadContent(mContentManager);
            aEnemy.Spawn(rInt);
            mEnemies.Add(aEnemy);

        }

        private void ShootBullet()
        {
            if (mCurrentState == State.Flying)
            {
                bool aCreateNew = true;
                foreach (Bullet aBullet in mBullets)
                {
                    if (aBullet.Visible == false)
                    {
                        aCreateNew = false;
                        aBullet.Fire(Position + new Vector2(Size.Width - 20, Size.Height / 2 + 2),
                            new Vector2(500, 0), new Vector2(1, 0));
                        break;
                    }
                }

                if (aCreateNew == true)
                {
                    Bullet aBullet = new Bullet();
                    aBullet.LoadContent(mContentManager);
                    aBullet.Fire(Position + new Vector2(Size.Width - 20, Size.Height / 2 + 2),
                        new Vector2(500, 200), new Vector2(1, 0));
                    mBullets.Add(aBullet);
                }
            }
        }
        private void CanFireTimer(double deltaTime)
        {
            if(FireDelay < 0)
            {
                MayFire = true;
            }
            else
            {
                MayFire = false;
                FireDelay -= deltaTime;
            }
        }
        private void MaySpawnTimer(double deltaTime)
        {
            if (SpawnDelay < 0)
            {
                MaySpawn = true;
            }
            else
            {
                MaySpawn = false;
                SpawnDelay -= deltaTime;
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Bullet aBullet in mBullets)
            {
                aBullet.Draw(theSpriteBatch);
                
            }
            foreach (Enemy aEnemy in mEnemies)
            {
                aEnemy.Draw(theSpriteBatch);
            }
            base.Draw(theSpriteBatch);
        }
        public void UpdateHit()
        {

            foreach (Bullet aBullet in mBullets)
            {
                BulletRec = new Rectangle((int)aBullet.Position.X, (int)aBullet.Position.Y, (int)aBullet.Width, (int)aBullet.Height);
                foreach (Enemy aEnemy in mEnemies)
                {
                    EnemyRec = new Rectangle((int)aEnemy.Position.X, (int)aEnemy.Position.Y, (int)aEnemy.Width, (int)aEnemy.Height);
                    if (BulletRec.Intersects(EnemyRec))
                    {
                        aBullet.Position.X = -500;
                        aBullet.Position.Y = -500;
                     
                        aBullet.Visible = false;
                        aEnemy.Health -= aBullet.Damage;

                    }
                    if (aEnemy.Health <= 0)
                    {
                        aEnemy.Alive = false;
                        aEnemy.Position.X = -500;
                        aEnemy.Position.Y = -500;
                    }

                }
            }      
            
            
        }          
        
    }
}
