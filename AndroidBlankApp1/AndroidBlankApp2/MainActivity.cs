using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace AndroidBlankApp2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private MainView _view;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _view = new MainView(this);
            SetContentView(_view);

            _view.OnClick = async () => { await ReadFile(); };
        }

        private async Task ReadFile()
        {
            var dir = new File("/data/user/0/AndroidBlankApp1.AndroidBlankApp1/files", "");
            //var dir = new File("/sdcard/Books", "");
            var files = await dir.ListAsync();
            _view.LogText = string.Join('\n', files ?? new[] { "Нет файлов или доступа к папке" });
        }
	}

    public class MainView : LinearLayout
    {
        public Action OnClick { get; set; }

        public string LogText
        {
            get => _logView.Text;
            set => _logView.Text = value;
        }

        private TextView _logView;

        private void InitViews()
        {
            Orientation = Orientation.Vertical;
            var createBtn = new Button(Context) { Text = "Создать" };
            createBtn.Click += (sender, args) => { OnClick?.Invoke(); };

            AddView(createBtn,
              new ViewGroup.LayoutParams(new LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent)));

            _logView = new TextView(Context);
            AddView(_logView,
              new ViewGroup.LayoutParams(new LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent)));
            _logView.Text = "this is text";
        }

        protected MainView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            InitViews();
        }

        public MainView(Context context) : base(context)
        {
            InitViews();
        }

        public MainView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitViews();
        }

        public MainView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitViews();
        }

        public MainView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs,
          defStyleAttr, defStyleRes)
        {
            InitViews();
        }
    }
}
