using System;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Environment = System.Environment;
using String = Java.Lang.String;
using StringBuilder = System.Text.StringBuilder;

namespace AndroidBlankApp1
{
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
  public class MainActivity : AppCompatActivity
  {
    private MainView _view;
    
    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      _view = new MainView(this);
      _view.OnCreateClick = async () => await CreateManyFiles();
            _view.OnViewClick = async () => { await ViewFiles(); };
      SetContentView(_view);
    }

    private async Task CreateManyFiles()
    {
      _view.LogText = "Начинаем создавать файлы";
      var text = new StringBuilder();

      _view.LogText = $"Создаем текст для файлов";
      
      for (var i = 0; i < 100000; i++)
      {
        text.Append(Guid.NewGuid().ToString());
      }

      var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

      for (var i = 0; i < 10000; i++)
      {
                 
        // File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Personal), text);
        var name = $"{Guid.NewGuid()}.txt";
        _view.LogText = $"Создаем файл {name}";
        await WriteFileOnInternalStorage(BaseContext, name, text.ToString());
        _view.LogText = $"'{name}' создан";
      }


      // создаем всплывающее окно c результатом выволнения записи в файл
      Toast.MakeText(BaseContext, "Запись в файл успешно проведена!", ToastLength.Long).Show();
    }

    public async Task WriteFileOnInternalStorage(Context mcoContext, string fileName, string content)
    {
      var dir = new File(mcoContext.FilesDir, "");

      if (!dir.Exists())
        dir.Mkdir();

      var gpxfile = new File(dir, fileName);
      var writer = new FileWriter(gpxfile);
      await writer.AppendAsync(new String(content));
      await writer.FlushAsync();
      writer.Close();
    }

        private async Task ViewFiles()
        {
            var dir = new File("/data/user/0/AndroidBlankApp1.AndroidBlankApp1/files", "");
            var files = await dir.ListAsync();
            _view.LogText = string.Join('\n', files);
        }
  }

  public class MainView : LinearLayout
  {
    public Action OnCreateClick { get; set; }

    public Action OnViewClick { get; set; }

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
            createBtn.Click += (sender, args) => { OnCreateClick?.Invoke(); };

            AddView(createBtn,
              new ViewGroup.LayoutParams(new LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent)));

            var viewBtn = new Button(Context) { Text = "Просмотр" };
            viewBtn.Click += (sender, args) => { OnViewClick?.Invoke(); };

            AddView(viewBtn,
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