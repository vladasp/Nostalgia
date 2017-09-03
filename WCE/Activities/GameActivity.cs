using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;

namespace WCE
{
    [Activity(Theme = "@style/Theme.Game",
            MainLauncher = true,
            AlwaysRetainTaskState = true,
            LaunchMode = LaunchMode.SingleInstance,
            ScreenOrientation = ScreenOrientation.ReverseLandscape,
            ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class GameActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        private GameWCE game;
        private DisplayMetrics display;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            display = Resources.DisplayMetrics;
        }

        protected override void OnStart()
        {
            InitGame();
            base.OnStart();
        }

        private void InitGame()
        {
            game = new GameWCE(this, display.HeightPixels, display.WidthPixels);
            SetContentView((View)game.Services.GetService(typeof(View)));
            game.Run();
        }

        public override void OnBackPressed()
        {
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
            base.OnBackPressed();
        }
    }
}

