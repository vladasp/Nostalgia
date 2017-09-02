using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Widget;
using Android.Graphics.Drawables;
using System.Threading;

namespace WCE.Activities
{
    [Activity(NoHistory = true, 
        MainLauncher = true, 
        Theme = "@style/Theme.Splash", 
        LaunchMode = LaunchMode.SingleInstance, 
        ScreenOrientation = ScreenOrientation.SensorLandscape)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splash_screen);

            ThreadPool.QueueUserWorkItem(o => LoadActivity());
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageAnim);

            AnimationDrawable animation = (AnimationDrawable)imageView.Drawable;

            animation.Start();
        }

        private void LoadActivity()
        {
            System.Threading.Thread.Sleep(3000);
            RunOnUiThread(() => 
            {
                StartActivity(typeof(MainActivity));
                OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
            });
        }
    }
}