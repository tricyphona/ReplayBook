using Etirps.RiZhi;
using ModernWpf;
using Rofl.Files;
using Rofl.Requests;
using Rofl.Settings;
using Rofl.UI.Main.Controls;
using Rofl.UI.Main.Utilities;
using Rofl.UI.Main.Views;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Rofl.UI.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private FileManager _files;
        private RequestManager _requests;
        private SettingsManager _settingsManager;
        private RiZhi _log;
        private ReplayPlayer _player;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {   
            // We must load settings before anything else
            var assemblyName = Assembly.GetEntryAssembly()?.GetName();
            _log = new RiZhi()
            {
                FilePrefix = "ReplayBookLog",
                AssemblyName = assemblyName.Name,
                AssemblyVersion = assemblyName.Version.ToString(2)
            };
            _settingsManager = new SettingsManager(_log);
            // Apply appearence theme
            ApplyThemeSetting();
            // Apply language setting
            LanguageHelper.SetProgramLanguage(_settingsManager.Settings.ProgramLanguage);

            if (e.Args.Length == 1)
            {
                var selectedFile = e.Args[0];
                // 0 = directly play, 1 = open in replaybook
                if (_settingsManager.Settings.FileAction == Settings.Models.FileAction.Play)
                {
                    StartDialogHost();
                    await _player.PlayReplay(selectedFile).ConfigureAwait(true);
                    Application.Current.Shutdown();
                }
                else if (_settingsManager.Settings.FileAction == Settings.Models.FileAction.Open)
                {
                    StartMainWindow(true);
                }
                else { }
            }
            else
            {
                StartMainWindow(false);
            }
        }

        private void StartDialogHost()
        {
            // Start a blank/invisible window that will host dialogs, otherwise dialogs are invisible
            var host = new DialogHostWindow();
            host.Show();
        }

        private void StartMainWindow(bool smallMode)
        {
            // Show the splash window
            var mainWindow = new MainWindowContainer(smallMode);
            mainWindow.Show();

            // Create the rest of the requied objects
            CreateCommonObjects();

            var content = new MainWindow(_log, _settingsManager, _requests, _files, _player);

            mainWindow.Content = content;
        }

        private void StartSingleReplayWindow(string path)
        {
            var singleWindow = new SingleReplayWindow(_log, _settingsManager, _requests, _files, _player)
            {
                ReplayFileLocation = path
            };
            singleWindow.Show();
        }

        private void CreateCommonObjects()
        {
            // Create common objects
            try
            {
                _files = new FileManager(_settingsManager.Settings, _log);
                _requests = new RequestManager(_settingsManager.Settings, _log);
                _player = new ReplayPlayer(_files, _settingsManager, _log);
            }
            catch(Exception ex)
            {
                _log.Error(ex.ToString());
                _log.WriteLog();
                throw;
            }
        }

        private void ApplyThemeSetting()
        {
            // Set theme mode
            switch (_settingsManager.Settings.ThemeMode)
            {
                case 0: // system default
                    ThemeManager.Current.ApplicationTheme = null;
                    break;
                case 1: // 
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                    break;
                case 2: // light
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                    break;
            }

            // Set accent color
            if (_settingsManager.Settings.AccentColor == null)
            {
                ThemeManager.Current.AccentColor = null;
            }
            else
            {
                ThemeManager.Current.AccentColor = (Color)ColorConverter.ConvertFromString(_settingsManager.Settings.AccentColor);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if(_log.ErrorFlag)
            {
                _log.WriteLog();
            }
        }
    }
}
