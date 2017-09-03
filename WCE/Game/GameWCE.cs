using Android.App;
using Android.Content;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Timers;
using WCE.Game;

namespace WCE
{
    public class GameWCE : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CustomButton _playPauseButton;
        private CustomButton _stopButton;

        private Wolf _wolf;
        private List<Egg> _eggs = new List<Egg>();
        private List<Egg> _eggsToAdd = new List<Egg>();
        private List<Egg> _eggsToRemove = new List<Egg>();
        private ChickenLife _chickenLife;
        private BrockenChicken _leftBrockenChicken;
        private BrockenChicken _rightBrockenChicken;

        private Texture2D _playTexture;
        private Texture2D _pauseTexture;
        private Texture2D _stopTexture;
        private Texture2D _looserTexture;
        private Texture2D _looser2Texture;
        private Texture2D _pauseT;



        private bool _needUpdateLeft;
        private bool _needUpdateRight;

        private bool _isBusy;

        private Timer _eggesTimer;
        private int _interval = 2000;

        private int _heightPixels;
        private int _widthPixels;
        private Texture2D convasTexture;
        private float _xScale;
        private float _yScale;
        private Context _context;
        private static SpriteFont _font;
        private int _score = 0;
        private int _lifes = 3;

        private float _xPlayPause;
        private float _xStop;
        private float _y;

        private bool _isPause = false;
        public bool IsPaused
        {
            get
            {
                return _isPause;
            }
            set
            {
                _isPause = value;

                if(!_isPause)
                    _eggesTimer.Start();

                foreach(var egg in _eggs)
                    egg.IsPaused = _isPause;
            }
        }

        private bool _isStop = false;

        private TouchLocationState _currentTouchState;

        public SpriteBatch SpriteBatch { get => _spriteBatch; set => _spriteBatch = value; }

        public GameWCE(Context context, int heightPixels, int widthPixels)
        {
            _heightPixels = heightPixels;
            _widthPixels = widthPixels;

            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = true,
                SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight
            };

            InitEggesTimer();

            _context = context;
        }

        private void InitEggesTimer()
        {
            _eggesTimer = new Timer(_interval)
            {
                AutoReset = true
            };
            _eggesTimer.Elapsed += EggesTimer_Elapsed;
            _eggesTimer.Start();
        }

        private void EggesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(_lifes > 0 && !IsPaused)
            {
                if(!_isBusy)
                {
                    _eggs.Add(new Egg(GraphicsDevice, GenerateState(), new Point(_widthPixels, _heightPixels)));
                }
                else
                {
                    _eggsToAdd.Add(new Egg(GraphicsDevice, GenerateState(), new Point(_widthPixels, _heightPixels)));
                }
            }
            else
            {
                IsPaused = _playPauseButton.Selected = _stopButton.Selected = _isStop = true;
                _eggesTimer.Stop();
            }
        }

        private State GenerateState()
        {
            var random = new Random();
            var numer = random.Next(4);
            return (State)numer;
        }

        protected override void Initialize()
        {
            _wolf = new Wolf(GraphicsDevice, new Point(_widthPixels, _heightPixels));

            _chickenLife = new ChickenLife(GraphicsDevice, new Point(_widthPixels, _heightPixels));

            _leftBrockenChicken = new BrockenChicken(GraphicsDevice, Side.Left, new Point(_widthPixels, _heightPixels));
            _rightBrockenChicken = new BrockenChicken(GraphicsDevice, Side.Right, new Point(_widthPixels, _heightPixels));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "Content/";

            using(var stream = TitleContainer.OpenStream("Content/convas.png"))
            {
                convasTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
            }

            if(_pauseTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/pause_btn.png"))
                {
                    _pauseTexture = Texture2D.FromStream(GraphicsDevice, stream);
                }
            }

            if(_playTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/play_btn.png"))
                {
                    _playTexture = Texture2D.FromStream(GraphicsDevice, stream);
                }
            }

            if(_stopTexture == null)
            {
                using(var stream = TitleContainer.OpenStream("Content/stop_btn.png"))
                {
                    _stopTexture = Texture2D.FromStream(GraphicsDevice, stream);
                }
            }

            _xPlayPause = _widthPixels * 0.43f;
            _xStop = _widthPixels * 0.59f;
            _y = _heightPixels * 0.02f;
            var playPauseVector = new Vector2((int)_xPlayPause, (int)_y);
            var stopVector = new Vector2((int)_xStop, (int)_y);

            _playPauseButton = new CustomButton(GraphicsDevice, _pauseTexture, _playTexture, playPauseVector);
            _stopButton = new CustomButton(GraphicsDevice, _stopTexture, _stopTexture, stopVector);

            _font = Content.Load<SpriteFont>("Font");

            _xScale = _widthPixels / (float)convasTexture.Width;
            _yScale = _heightPixels / (float)convasTexture.Height;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateTimer(gameTime.TotalGameTime.TotalSeconds);

            var touchCollection = TouchPanel.GetState();

            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                var builder = new AlertDialog.Builder(_context)
                    .SetMessage("Exit?")
                    .SetPositiveButton("Yes", delegate
                    {
                        Exit();
                    })
                    .SetNegativeButton("Cancel", delegate
                    {
                    }).Show();
            }

            _chickenLife.Update(3 - _lifes);

            _rightBrockenChicken.Update(_needUpdateRight);
            _needUpdateRight = false;
            _leftBrockenChicken.Update(_needUpdateLeft);
            _needUpdateLeft = false;

            var scale = _xScale > _yScale ? _xScale : _yScale;

            if(touchCollection.Count > 0)
            {
                var x = touchCollection[0].Position.X;
                var y = touchCollection[0].Position.Y;
                if(_xPlayPause < x && _xPlayPause + scale * _pauseTexture.Width > x && scale * _pauseTexture.Height > y)
                {
                    if(touchCollection[0].State == TouchLocationState.Pressed)
                    {
                        IsPaused = !IsPaused;
                        _playPauseButton.Selected = IsPaused;

                        if(!IsPaused)
                        {
                            if(_isStop)
                            {
                                _isStop = false;
                            }
                            if(_lifes == 0)
                            {
                                InitStartConditions();
                            }
                        }
                    }
                }
                else if(!IsPaused && _xStop < x && _xStop + scale * _stopTexture.Width > x && scale * _stopTexture.Height > y)
                {
                    if(touchCollection[0].State == TouchLocationState.Pressed)
                    {
                        IsPaused = _playPauseButton.Selected = _stopButton.Selected = _isStop = true;
                        InitStartConditions();
                        _eggs.Clear();
                        _eggsToAdd.Clear();
                        _eggsToRemove.Clear();
                    }
                }
                else
                {
                    if(!IsPaused)
                        _wolf.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        private void InitStartConditions()
        {
            _lifes = 3;
            _interval = 2000;
            _score = 0;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            SpriteBatch.Begin();

            SpriteBatch.Draw(convasTexture, new Rectangle(0, 0, _widthPixels, _heightPixels), Color.White);
            SpriteBatch.DrawString(_font, "Score: " + _score.ToString(), new Vector2(220, 20), Color.DarkSlateGray, 0, new Vector2(), 1.5f, SpriteEffects.None, 0f);
            //SpriteBatch.DrawString(_font, "Time: " + _interval.ToString()/*gameTime.TotalGameTime.ToString(@"mm\:ss")*/, new Vector2(220, 75), Color.DarkSlateGray, 0, new Vector2(), 1.5f, SpriteEffects.None, 0f);

            _wolf.Draw(SpriteBatch, _xScale, _yScale);

            _chickenLife.Draw(SpriteBatch, _xScale, _yScale);

            _isBusy = true;
            try
            {
                foreach(var egg in _eggs)
                {
                    if(!egg.IsComplete)
                    {
                        egg.Draw(SpriteBatch, _xScale, _yScale);
                    }
                    else
                    {
                        if(_wolf.WolfState != egg.EggState)
                        {
                            --_lifes;
                            if(egg.EggState == State.TopLeft || egg.EggState == State.BottomLeft)
                            {
                                _needUpdateLeft = true;
                            }
                            else
                            {
                                _needUpdateRight = true;
                            }
                        }
                        else
                        {
                            ++_score;
                        }

                        _eggsToRemove.Add(egg);
                    }
                }
            }
            catch { }
            _isBusy = false;

            _leftBrockenChicken.Draw(SpriteBatch, _xScale, _yScale);

            _rightBrockenChicken.Draw(SpriteBatch, _xScale, _yScale);

            if(!_isStop)
                _stopButton.Draw(SpriteBatch, _xScale, _yScale);

            _playPauseButton.Draw(SpriteBatch, _xScale, _yScale);

            UpdateEggs();

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateTimer(double totalSeconds)
        {
            if((int)totalSeconds % 10 == 0)
            {
                if(_interval > 600 && !IsPaused)
                {
                    _interval -= 2;
                    _eggesTimer.Interval = _interval;
                    _eggesTimer.Start();
                }
            }
        }

        private void UpdateEggs()
        {
            if(_lifes == 0)
            {
                _eggs.Clear();
                _eggsToRemove.Clear();
                _eggsToAdd.Clear();
            }

            if(_eggsToRemove.Count > 0)
            {
                foreach(var item in _eggsToRemove)
                    _eggs.Remove(item);

                _eggsToRemove.Clear();
            }

            if(_eggsToAdd.Count > 0)
            {
                _eggs.AddRange(_eggsToAdd);
                _eggsToAdd.Clear();
            }
        }
    }
}
