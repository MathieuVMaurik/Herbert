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
    class Enemy : Sprite
    {

        public bool Alive = true;
        enum State
        { 
            Attack,
            Reload,
            Death
        }
        State EnemyState = State.Attack;
        Vector2 EnemyPosistion;
        Vector2 EnemySpeed;
        Vector2 EnemyDirection;
        Vector2 EnemyVelocity;
        public int Health = 40;

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "enemy");
            Scale = 1f;
        }
        public void Update(GameTime theGameTime)
        {
            if (EnemyState == State.Attack)
            {
                Attack();
            }



            

                base.Update(theGameTime, EnemySpeed, EnemyDirection);
           
          
        }
        public void Attack()
        {
            this.EnemyVelocity = Player.Posistion + this.EnemyPosistion * this.EnemySpeed;
            this.Position -= this.EnemySpeed;
        }
        
           
        public override void Draw(SpriteBatch theSpriteBatch)
        {
                base.Draw(theSpriteBatch);
        }

        public void Spawn(int rInt)
        {
            Position =  new Vector2(500, rInt);
            EnemyPosistion = new Vector2(250, 250);
        }

      

    }
}