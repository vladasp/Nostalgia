using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WCE.Game;

namespace WCE
{
    public class Wolf
    {
        private Texture2D _wolfLeftTexture;
        private Texture2D _wolfRightTexture;
        private Texture2D _handLeftTopTexture;
        private Texture2D _handLeftBottomTexture;
        private Texture2D _handRightTopTexture;
        private Texture2D _handRightBottomTexture;

        private bool _isLeft = true;
        private bool _isTop = true;

        public float X { get; set; }
        public float Y { get; set; }

        public State WolfState { get; private set; }

        public Wolf(GraphicsDevice graphicsDevice, Point rightButtomPoint)
        {
            #region init textures

            if(_wolfLeftTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/wolf_left_without_hands.png"))
                {
                    _wolfLeftTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_wolfRightTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/wolf_right_without_hands.png"))
                {
                    _wolfRightTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_handLeftTopTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/hands_left_top.png"))
                {
                    _handLeftTopTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_handLeftBottomTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/hands_left_bottom.png"))
                {
                    _handLeftBottomTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_handRightTopTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/hands_right_top.png"))
                {
                    _handRightTopTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_handRightBottomTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/hands_right_bottom.png"))
                {
                    _handRightBottomTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            #endregion

            X = rightButtomPoint.X / 2;
            Y = rightButtomPoint.Y / 2;
        }

        internal void Draw(SpriteBatch spriteBatch, float xScale, float yScale)
        {
            var centerSprite = new Vector2(X, Y);
            var scale = xScale <= yScale ? xScale : yScale;

            if(_isLeft)
            {
                var wolfPosition = new Vector2()
                {
                    X = centerSprite.X - 2 * _wolfLeftTexture.Width,
                    Y = (int)(centerSprite.Y * 0.73)
                };
                DrawTexture(spriteBatch, _wolfLeftTexture, wolfPosition, scale);

                if(_isTop)
                {
                    WolfState = State.TopLeft;
                    var topHandsPosition = new Vector2()
                    {
                        X = wolfPosition.X - _handLeftTopTexture.Width,
                        Y = wolfPosition.Y
                    };
                    DrawTexture(spriteBatch, _handLeftTopTexture, topHandsPosition, scale);
                }
                else
                {
                    WolfState = State.BottomLeft;
                    var bottomHandsPosition = new Vector2()
                    {
                        X = wolfPosition.X - _handLeftBottomTexture.Width,
                        Y = wolfPosition.Y + _handLeftTopTexture.Height
                    };
                    DrawTexture(spriteBatch, _handLeftBottomTexture, bottomHandsPosition, scale);
                }
            }
            else
            {
                var wolfPosition = new Vector2()
                {
                    X = (int)(centerSprite.X * 1.05),
                    Y = (int)(centerSprite.Y * 0.75)
                };
                DrawTexture(spriteBatch, _wolfRightTexture, wolfPosition, scale);

                if(_isTop)
                {
                    WolfState = State.TopRight;
                    var topHandsPosition = new Vector2()
                    {
                        X = wolfPosition.X + _handRightTopTexture.Width,
                        Y = wolfPosition.Y
                    };
                    DrawTexture(spriteBatch, _handRightTopTexture, topHandsPosition, scale);
                }
                else
                {
                    WolfState = State.BottomRight;
                    var bottomHandsPosition = new Vector2()
                    {
                        X = wolfPosition.X + _handRightBottomTexture.Width,
                        Y = wolfPosition.Y + _handRightTopTexture.Height
                    };
                    DrawTexture(spriteBatch, _handRightBottomTexture, bottomHandsPosition, scale);
                }
            }
        }

        private void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float scale)
        {
            spriteBatch.Draw(texture, position, null, Color.Gray, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }

        private bool IsTouchedLeft()
        {
            var touchCollection = TouchPanel.GetState();

            if(touchCollection.Count > 0)
            {
                return X > touchCollection[0].Position.X;
            }

            return _isLeft;
        }

        private bool IsTouchedTop()
        {
            var touchCollection = TouchPanel.GetState();

            if(touchCollection.Count > 0)
            {
                return Y > touchCollection[0].Position.Y;
            }

            return _isTop;
        }

        public void Update(GameTime gameTime)
        {
            _isLeft = IsTouchedLeft();
            _isTop = IsTouchedTop();
        }
    }
}