using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace WCE.Game
{
    public class Egg
    {
        private Texture2D _egg1Texture;
        private Texture2D _egg2Texture;
        private Texture2D _egg3Texture;
        private Texture2D _egg4Texture;
        private Texture2D _egg5Texture;

        private Vector2 _egg1Position;
        private Vector2 _egg2Position;
        private Vector2 _egg3Position;
        private Vector2 _egg4Position;
        private Vector2 _egg5Position;

        private int _eggNumber = 0;

        private int _width;
        private int _height;

        private Timer _timer;

        public bool IsPaused { get; set; }

        public bool IsComplete { get; private set; } = false;
        public State EggState { get; set; }

        public Egg(GraphicsDevice graphicsDevice, State eggState, Point rightButtomPoint)
        {
            _width = rightButtomPoint.X;
            _height = rightButtomPoint.Y;

            EggState = eggState;

            InitPositions(eggState);

            #region init textures

            if(_egg1Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/egg1.png"))
                {
                    _egg1Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_egg2Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/egg2.png"))
                {
                    _egg2Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_egg3Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/egg3.png"))
                {
                    _egg3Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_egg4Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/egg4.png"))
                {
                    _egg4Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            if(_egg5Texture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/egg5.png"))
                {
                    _egg5Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            #endregion

            _timer = new Timer(300)
            {
                AutoReset = false
            };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
           if(_eggNumber < 5)
            {
                _timer.Start();
                if(!IsPaused)
                    ++_eggNumber;
            }
            else
            {
                _eggNumber = 0;
                IsComplete = true;
            }
        }

        private void InitPositions(State state)
        {
            float x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0;
            float y1 = 0, y2 = 0, y3 = 0, y4 = 0, y5 = 0;
            switch(state)
            {
                case State.TopLeft:
                    x1 = _width * 0.055f; y1 = _height * 0.27f;
                    x2 = _width * 0.09f; y2 = _height * 0.295f;
                    x3 = _width * 0.125f; y3 = _height * 0.32f;
                    x4 = _width * 0.16f; y4 = _height * 0.345f;
                    x5 = _width * 0.195f; y5 = _height * 0.37f;
                    break;
                case State.BottomLeft:
                    x1 = _width * 0.055f; y1 = _height * 0.505f;
                    x2 = _width * 0.09f; y2 = _height * 0.53f;
                    x3 = _width * 0.125f; y3 = _height * 0.555f;
                    x4 = _width * 0.16f; y4 = _height * 0.58f;
                    x5 = _width * 0.195f; y5 = _height * 0.605f;
                    break;
                case State.TopRight:
                    x1 = _width * 0.9f; y1 = _height * 0.265f;
                    x2 = _width * 0.865f; y2 = _height * 0.29f;
                    x3 = _width * 0.83f; y3 = _height * 0.315f;
                    x4 = _width * 0.795f; y4 = _height * 0.35f;
                    x5 = _width * 0.76f; y5 = _height * 0.375f;
                    break;
                case State.BottomRight:
                    x1 = _width * 0.9f; y1 = _height * 0.5f;
                    x2 = _width * 0.865f; y2 = _height * 0.525f;
                    x3 = _width * 0.83f; y3 = _height * 0.55f;
                    x4 = _width * 0.795f; y4 = _height * 0.585f;
                    x5 = _width * 0.76f; y5 = _height * 0.61f;
                    break;
            }

            _egg1Position = new Vector2((int)x1, (int)y1);
            _egg2Position = new Vector2((int)x2, (int)y2);
            _egg3Position = new Vector2((int)x3, (int)y3);
            _egg4Position = new Vector2((int)x4, (int)y4);
            _egg5Position = new Vector2((int)x5, (int)y5);
        }

        internal void Draw(SpriteBatch spriteBatch, float xScale, float yScale)
        {
            var scale = xScale <= yScale ? xScale : yScale;

            switch(_eggNumber)
            {
                case 1:
                    DrawTexture(spriteBatch, _egg1Texture, _egg1Position, scale);
                    break;

                case 2:
                    DrawTexture(spriteBatch, _egg2Texture, _egg2Position, scale);
                    break;

                case 3:
                    DrawTexture(spriteBatch, _egg3Texture, _egg3Position, scale);
                    break;

                case 4:
                    DrawTexture(spriteBatch, _egg4Texture, _egg4Position, scale);
                    break;

                case 5:
                    DrawTexture(spriteBatch, _egg5Texture, _egg5Position, scale);
                    break;

                default:
                    break;
            }
        }

        private void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float scale)
        {
            spriteBatch.Draw(texture, position, null, Color.Gray, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }

    }
}