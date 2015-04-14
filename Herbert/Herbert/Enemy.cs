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
        Vector2 mStartPosition;
        Vector2 mSpeed;
        Vector2 mDirection;
        public int Health = 80;

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "enemy");
            Scale = 1f;
        }
        public void Update(GameTime theGameTime)
        {
          
                base.Update(theGameTime, mSpeed, mDirection);
           
          
        }


        public override void Draw(SpriteBatch theSpriteBatch)
        {
                base.Draw(theSpriteBatch);
        }

        public void Spawn( Vector2 theSpeed, Vector2 theDirection)
        {
            Position =  new Vector2(250,250);
            mStartPosition = new Vector2(250, 250);
            mSpeed = theSpeed;
            mDirection = theDirection;
        }

      

    }
}