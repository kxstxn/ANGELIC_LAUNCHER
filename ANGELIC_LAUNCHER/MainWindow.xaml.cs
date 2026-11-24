using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CmlLib;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using System.Text.Json;
using Microsoft.Windows.Themes;
using System.IO;
using System.Net.Security;

namespace ANGELIC_LAUNCHER
{
    public partial class MainWindow : Window
    {
        private readonly string _settingsPath = "settings.json";
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Load;
        }

        private async void MainWindow_Load(object sender, RoutedEventArgs e)
        {
            var path = new MinecraftPath();
            var launcher = new MinecraftLauncher(path);
            var versions = await launcher.GetAllVersionsAsync();

            foreach (var version in versions)
            {
                if (version.Type == "release")
                {
                    VersionsCombobox.Items.Add(version.Name);
                }
            }
            LoadSettings();
        }
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var default_server = DefaultServerTextBox.Text;
            StartButton.IsEnabled = false;
            SaveSettings();
            var path = new MinecraftPath();
            var launcher = new MinecraftLauncher(path);

            var selected_version = VersionsCombobox.SelectedItem.ToString();
            if (NicknameTextBox.Text == "")
            {
                NicknameTextBox.Text = "Angelic_User";
            }
            var Nickname = NicknameTextBox.Text;

            var loadOption = new MLaunchOption
            {
                Session = MSession.CreateOfflineSession(Nickname),
                MaximumRamMb = 4096,
                FullScreen = true,
                ServerIp = string.IsNullOrWhiteSpace(default_server) ? null:default_server,
            };

            launcher.ByteProgressChanged += (s, args) => {

                double percent = args.ToRatio() * 100;

                Dispatcher.Invoke(() =>
                {
                    DownloadProgressBar.Value = percent;
                    PercentLabel.Content = $"{percent.ToString("0.00")}%";
                    });
            
            };

            await launcher.InstallAsync(selected_version);

            var process = await launcher.BuildProcessAsync(selected_version, loadOption);
            process.Start();
            StartButton.IsEnabled = true;
        }

        private void LoadSettings()
        {
            if (!File.Exists(_settingsPath)) return;

            string json = File.ReadAllText(_settingsPath);
            var settings = JsonSerializer.Deserialize<LauncherSettings>(json);

            if (settings != null)
            {
                NicknameTextBox.Text = settings.Nickname;
            }
            if (!string.IsNullOrEmpty(settings.LatestVersion))
            {
                VersionsCombobox.SelectedItem = settings.LatestVersion;
            }
            if (!string.IsNullOrEmpty(settings.DefaultServer))
            {
                DefaultServerTextBox.Text = settings.DefaultServer;
            } 
        }
        private void SaveSettings()
        {
            var Settings = new LauncherSettings
            {
                Nickname = NicknameTextBox.Text,
                LatestVersion = VersionsCombobox.SelectedItem.ToString() ?? "",
                DefaultServer = DefaultServerTextBox.Text,
            };

            string json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsPath, json);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
