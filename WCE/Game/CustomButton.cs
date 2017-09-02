using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WCE.Game
{
    public class CustomButton
    {
        private Texture2D _backgroundSelectedTexture;
        private Texture2D _backgroundNormalTexture;

        private Vector2 _backgroundPosition;

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

        public CustomButton(GraphicsDevice graphicsDevice,
            Texture2D backgroundNormalTexture,
            Texture2D backgroundSelectedTexture,
            Vector2 backgroundPosition)
        {
            _backgroundPosition = backgroundPosition;
            _backgroundNormalTexture = backgroundNormalTexture;
            _backgroundSelectedTexture = backgroundSelectedTexture;
        }

        internal void Draw(SpriteBatch spriteBatch, float xScale, float yScale)
        {
            var scale = xScale <= yScale ? xScale : yScale;
            var texture = Selected ? _backgroundSelectedTexture : _backgroundNormalTexture;
            spriteBatch.Draw(texture, _backgroundPosition, null, Color.Gray, 0, new Vector2(0, 0), scale * 1f, SpriteEffects.None, 0);
        }
    }
}