using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WCE.Game
{
    public class ChickenLife
    {
        private Texture2D _chickenLifeTexture;
        private Vector2 _chicken1Position;
        private Vector2 _chicken2Position;
        private Vector2 _chicken3Position;

        private int _width;
        private int _height;

        private int _lifes;

        public ChickenLife(GraphicsDevice graphicsDevice, Point rightButtomPoint, int lifes = 0)
        {
            _width = rightButtomPoint.X;
            _height = rightButtomPoint.Y;

            _lifes = lifes;

            InitPositions();

            if(_chickenLifeTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/life_chicken.png"))
                {
                    _chickenLifeTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
        }

        private void InitPositions()
        {
            float x1 = _width * 0.6f; float y1 = _height * 0.15f;
            float x2 = _width * 0.67f; float y2 = _height * 0.15f;
            float x3 = _width * 0.74f; float y3 = _height * 0.15f;

            _chicken1Position = new Vector2((int)x1, (int)y1);
            _chicken2Position = new Vector2((int)x2, (int)y2);
            _chicken3Position = new Vector2((int)x3, (int)y3);
        }

        internal void Update(int lifes)
        {
            _lifes = lifes;
        }

        internal void Draw(SpriteBatch spriteBatch, float xScale, float yScale)
        {
            var scale = xScale <= yScale ? xScale : yScale;

            switch(_lifes)
            {
                case 1:
                    DrawTexture(spriteBatch, _chicken1Position, scale);
                    break;

                case 2:
                    DrawTexture(spriteBatch, _chicken1Position, scale);
                    DrawTexture(spriteBatch, _chicken2Position, scale);
                    break;

                case 3:
                    DrawTexture(spriteBatch, _chicken1Position, scale);
                    DrawTexture(spriteBatch, _chicken2Position, scale);
                    DrawTexture(spriteBatch, _chicken3Position, scale);
                    break;

                default:
                    break;
            }
        }

        private void DrawTexture(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            spriteBatch.Draw(_chickenLifeTexture, position, null, Color.Gray, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }
    }
}