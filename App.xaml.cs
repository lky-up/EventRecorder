namespace Timeline
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }
        public App()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "events.db3");

            Database = new DatabaseService(dbPath);

            MainPage = new AppShell();

            
        }
    }
}
