using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace WCE
{
    [Activity(Theme = "@style/Theme.Main", LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.SensorLandscape)]
    public class MainActivity : Activity
    {
        private Button _startGameButton;
        private Button _exitButton;
        private Button _optionsButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);

            SetContentView(Resource.Layout.menu_layout);

            _startGameButton = FindViewById<Button>(Resource.Id.buttonStartGame);
            _exitButton = FindViewById<Button>(Resource.Id.buttonExit);
            _optionsButton = FindViewById<Button>(Resource.Id.buttonOptions);

            _startGameButton.Click += StartGameButton_Click;
            _exitButton.Click += ExitButton_Click;
            _optionsButton.Click += OptionsButton_Click;
        }

        private void OptionsButton_Click(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "No options yet", ToastLength.Long).Show();
        }

        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            Finish();
        }

        protected override void OnStart()
        {
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
            base.OnStart();
        }

        private void StartGameButton_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(GameActivity));
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        public override void OnBackPressed()
        {
            var builder = new AlertDialog.Builder(this)
                    .SetMessage("Выйти из игры?")
                    .SetNegativeButton("Cancel", delegate { })
                    .SetPositiveButton("Yes", delegate { Finish(); });

            var dialog = builder.Create();
            var drawable = new GradientDrawable();
            drawable.SetCornerRadius(25);
            drawable.SetColor(Color.Black);
            dialog.Window.SetBackgroundDrawable(drawable);
            dialog.Show();
        }
    }
}