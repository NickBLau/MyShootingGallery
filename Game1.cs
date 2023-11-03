
//namespaces
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace MyShootingGallery
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        readonly List<SoundEffect> soundEffects;

        Texture2D targetSprite;
        Texture2D rareTargetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;
        Texture2D bulletSprite;
        Texture2D bombSprite;
        SpriteFont gameFont;

        Vector2 targetPosition = new Vector2(350, 250);
        Vector2 rareTargetPosition = new Vector2(0, 600);
        Vector2 bombPosition = new Vector2(0, 600);
        const int targetRadius = 45;
        const int bombRadius = 45;

        

        MouseState mState;
        bool mReleased = true;
        bool rareTargetActive = false;
        bool bombActive = false;
       
        bool noAmmo = false;
        bool gameOver = false;
        bool reload = false;
       
        int score = 0;
        int ammo = 7;
     
        int timeoutDuration = 1000;
      

        float timer = 100f;
        float rareTargetElapsedTime = 0f;
        float bombElapsedTime = 0f;
        private void restartGame()
        {
            score = 0;
            ammo = 7;
            timer = 5f;
            gameOver = false;
            mReleased = true;
            noAmmo = false;

        }

        private void reloading()
        {
            ammo = 7;
            mReleased = true;
            noAmmo = false;

        }


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            soundEffects = new List<SoundEffect>();
            IsMouseVisible = false;
            
        }
         
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Song song = Content.Load<Song>("yt5s.io - HoloCure OST - Suspect (Extended Loop) (128 kbps)");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.09f;
            MediaPlayer.IsRepeating = true;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            soundEffects.Add(Content.Load<SoundEffect>("gun-gunshot-02"));
            soundEffects.Add(Content.Load<SoundEffect>("ding"));
            soundEffects.Add(Content.Load<SoundEffect>("hit-metal"));
            soundEffects.Add(Content.Load<SoundEffect>("reload"));
            soundEffects.Add(Content.Load<SoundEffect>("game-explosion"));
            soundEffects.Add(Content.Load<SoundEffect>("noAmmo"));

       
            
            // TODO: use this.Content to load your game content here
            targetSprite = Content.Load<Texture2D>("target");
            rareTargetSprite = Content.Load<Texture2D>("target");
            bulletSprite = Content.Load<Texture2D>("bullet");
        
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            bombSprite = Content.Load<Texture2D>("Bomb");

            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.R ))
                reloading();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter) && gameOver == true)
                restartGame();

            Random random = new Random();
            int chance = random.Next(0, 100);
            // TODO: Add your update logic here
            // Is executed for every single frame of the game
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer < 0)
            {
                mReleased = false;
            }

            rareTargetElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (chance < 1 && !rareTargetActive)
            {
                rareTargetActive = true;
            }

            if (rareTargetElapsedTime >= 2f)
            {
                rareTargetActive = false;
                rareTargetElapsedTime = 0f;
                Random rand = new Random();
                rareTargetPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth - 50);
                rareTargetPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight - 50);



            }

     
            // TODO: Add your update logic here
            // Is executed for every single frame of the game
            bombElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (chance < 50 && !bombActive)
            {
                bombActive = true;
            }

            bombElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (bombElapsedTime >= 4f)
            {
               
                bombElapsedTime = 0f;
                Random rand = new Random();
                bombPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth - 50);
                bombPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight - 50);



            }



            mState = Mouse.GetState();
         

            if (ammo <= 0)
            {
                noAmmo = true;
            }
               if (mState.LeftButton == ButtonState.Pressed && mReleased == true && noAmmo == true) 
                {
                    var instance = soundEffects[5].CreateInstance();
                    instance.Play();
                  
                }
                if (mState.LeftButton == ButtonState.Pressed && mReleased == true && noAmmo == false)
                  {
                          var instance = soundEffects[0].CreateInstance();
                          ammo = ammo - 1;

           
                    
               
            instance.Play();
            }

           if (mState.LeftButton == ButtonState.Pressed && mReleased == true && noAmmo == false)

            {
              
                float mouseTargetDist = Vector2.Distance(targetPosition, mState.Position.ToVector2());
                float mouseBombDist = Vector2.Distance(bombPosition, mState.Position.ToVector2());
                float mouseRareTargetDist = Vector2.Distance(rareTargetPosition, mState.Position.ToVector2());
                if (mouseTargetDist < targetRadius)
                {
                    var instance = soundEffects[1].CreateInstance();

                    instance.Play();
                    score = score + 100;
                    timer = 5;

                    Random rand = new Random();

                    targetPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth - 50);
                    targetPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight - 50);

                }

              
                
                    
                    if (mouseRareTargetDist < targetRadius)
                    {
                        var instance = soundEffects[2].CreateInstance();
                        instance.Play();
                        score = score + 500;


                    
                        rareTargetElapsedTime = 0f;

                        Random rand = new Random();
                        rareTargetPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth - 50);
                        rareTargetPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight - 50);
                    }
                

                else if (mouseBombDist < bombRadius)
                {
                    var instance = soundEffects[4].CreateInstance();
                    instance.Play();
                    score = score - 200;
                    bombElapsedTime = 0f;

                    Random rand = new Random();
                    bombPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth - 50);
                    bombPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight - 50);
                }

            

                else if (mouseBombDist > bombRadius && mouseTargetDist > targetRadius && mouseRareTargetDist > targetRadius)
                {
                    score = score - 50;
                }

                mReleased = false;
            }
            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
   
       


                mReleased = false;
            }


            if (mState.LeftButton == ButtonState.Released)
            {
                
                mReleased = true;
            }

            if (timer <=  0)
            {
                gameOver = true;

            }

            base.Update(gameTime);
        }

     

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
      
            //_spriteBatch.Draw(targetSprite, new Vector2(0,0), Color.White);
           // _spriteBatch.Draw(targetSprite, new Vector2(150, 0), Color.Green);
          

            Vector2 bulletPosition = new Vector2(25, 450);
            int bulletSpacing = 20;
            for (int i = 0; i < ammo; i++)
            {
                Vector2 currentBulletPosition = bulletPosition + new Vector2(i * bulletSpacing, 0);
                _spriteBatch.Draw(bulletSprite, new Rectangle((int)currentBulletPosition.X, (int)currentBulletPosition.Y, 25, 25), Color.White);
            }

       
            

            if (gameOver == false) { _spriteBatch.DrawString(gameFont, "Time remaning: " + timer.ToString(), new Vector2(20, 50), Color.White); 
            _spriteBatch.DrawString(gameFont, "Your score: " + score.ToString(), new Vector2(20, 10), Color.White);
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
                _spriteBatch.Draw(bombSprite, new Rectangle((int)bombPosition.X - bombRadius, (int)bombPosition.Y - bombRadius, 2 * bombRadius, 2 * bombRadius), Color.White);
                if (rareTargetActive)
                {
                    _spriteBatch.Draw(rareTargetSprite, new Vector2(rareTargetPosition.X - targetRadius, rareTargetPosition.Y - targetRadius), Color.Gold);
                }
            }
            else
            {
                _spriteBatch.DrawString(gameFont, "Game over! you score was: " + score.ToString(), new Vector2(200, 180), Color.White);
                _spriteBatch.DrawString(gameFont, "press enter to try again", new Vector2(200, 230), Color.White);
            }





            if (ammo >= 0) { _spriteBatch.DrawString(gameFont, ammo.ToString(), new Vector2(25, 400), Color.White); }
            else
            {
                _spriteBatch.DrawString(gameFont, "0", new Vector2(25, 400), Color.White);
            }
            
            _spriteBatch.Draw(crosshairsSprite, new Vector2(mState.X - 25, mState.Y - 25), Color.White);

            _spriteBatch.End();  
            base.Draw(gameTime);
        }
    }
}