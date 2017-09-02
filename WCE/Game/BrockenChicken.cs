using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;
using System;

namespace WCE.Game
{
    public class BrockenChicken
    {
        private Texture2D _brockenEggTexture;
        private Texture2D _chicken1Texture;
        private Texture2D _chicken2Texture;
        private Texture2D _chicken3Texture;
        private Texture2D _chicken4Texture;

        private Vector2 _brockenEggPosition;
        private Vector2 _chicken1Position;
        private Vector2 _chicken2Position;
        private Vector2 _chicken3Position;
        private Vector2 _chicken4Position;

        private int _chickenNumber = 0;

        private int _width;
        private int _height;

        public bool IsComplete { get; private set; } = false;

        private Side _side;

        private SpriteEffects SpriteEffect
        {
            get
            {
                return _side == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }

        private Timer _timer;

        public BrockenChicken( GraphicsDevice graphicsDevice, Side chickenSide, Point rightButtomPoint)
        {
            _side = chickenSide;

            _width = rightButtomPoint.X;
            _height = rightButtomPoint.Y;

            InitPositions(chickenSide);

            #region init textures

            if(_brockenEggTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/brocken_egg.png"))
                {
                    _brockenEggTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_chicken1Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/chicken1.png"))
                {
                    _chicken1Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_chicken2Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/chicken2.png"))
                {
                    _chicken2Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_chicken3Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/chicken3.png"))
                {
                    _chicken3Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_chicken4Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/chicken4.png"))
                {
                    _chicken4Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            #endregion

            _timer = new Timer(200)
            {
                AutoReset = false
            };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(_chickenNumber < 5)
            {
                _timer.Start();
                ++_chickenNumber;
            }
            else
            {
                _chickenNumber = 0;
                IsComplete = true;
            }
        }

        private void InitPositions(Side side)
        {
            float x1 = 0, x2 = 0, x3 = 0, x4 = 0, x = 0;
            float y1 = 0, y2 = 0, y3 = 0, y4 = 0, y = 0;
            switch(side)
            {
                case Side.Left:
                    x = _width * 0.13f; y = _height * 0.75f;
                    x1 = _width * 0.1f; y1 = _height * 0.8f;
                    x2 = _width * 0.07f; y2 = _height * 0.8f;
                    x3 = _width * 0.04f; y3 = _height * 0.8f;
                    x4 = _width * 0.01f; y4 = _height * 0.8f;
                    break;
                case Side.Right:
                    x = _width * 0.69f; y = _height * 0.75f;
                    x1 = _width * 0.75f; y1 = _height * 0.8f;
                    x2 = _width * 0.81f; y2 = _height * 0.8f;
                    x3 = _width * 0.87f; y3 = _height * 0.8f;
                    x4 = _width * 0.93f; y4 = _height * 0.8f;
                    break;
            }

            _brockenEggPosition = new Vector2((int)x, (int)y);
            _chicken1Position = new Vector2((int)x1, (int)y1);
            _chicken2Position = new Vector2((int)x2, (int)y2);
            _chicken3Position = new Vector2((int)x3, (int)y3);
            _chicken4Position = new Vector2((int)x4, (int)y4);
        }

        internal void Update(bool needUpdate)
        {
            if(needUpdate)
            {
                _timer.Start();
                _chickenNumber = 0;
            }
        }

        internal void Draw(SpriteBatch spriteBatch, float xScale, float yScale)
        {
            var scale = xScale <= yScale ? xScale : yScale;

            switch(_chickenNumber)
            {
                case 1:
                    DrawTexture(spriteBatch, _brockenEggTexture, _brockenEggPosition, scale);
                    break;

                case 2:
                    DrawTexture(spriteBatch, _chicken1Texture, _chicken1Position, scale);
                    break;

                case 3:
                    DrawTexture(spriteBatch, _chicken2Texture, _chicken2Position, scale);
                    break;

                case 4:
                    DrawTexture(spriteBatch, _chicken3Texture, _chicken3Position, scale);
                    break;

                case 5:
                    DrawTexture(spriteBatch, _chicken4Texture, _chicken4Position, scale);
                    break;

                default:
                    break;
            }
        }

        private void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float scale)
        {
            spriteBatch.Draw(texture, position, null, Color.Gray, 0, new Vector2(0, 0), scale, SpriteEffect, 0);
        }


    }
}