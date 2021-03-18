using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Forms;

using iRacingProfiles.Components;
using iRacingProfiles.Exceptions;
using iRacingProfiles.Logic;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;
using iRacingProfiles.Properties;
using System.Resources;
using MenuItem = System.Windows.Controls.MenuItem;

namespace iRacingProfiles {
    public partial class MainWindow : Window {

        private IRacingProfiles profiles;

        public MainWindow() {
            profiles = IRacingProfiles.Instance;
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            if(profiles.Config.localization != null) {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(profiles.Config.localization);
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            }
            InitializeComponent();
            reloadUI();
            CenterWindowOnScreen();
        }

        private void CenterWindowOnScreen() {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            if(e.Exception is iRacingProfileException) {
                MessageBox.Show(Internazionalization.Instance.getValueFromKey(e.Exception.Message),
                    "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Handled = true;
            } else {
                e.Handled = false;
            }
        }

        public void reloadUI() {
            comboProfiles.ItemsSource = profiles.GetProfiles().Select(i => i.Name).ToList();
            comboProfiles.SelectedIndex = -1;
            chkActual.IsChecked = false;
            txtRoute.IsEnabled = true;
            btnSelectProfileFile.IsEnabled = true;
            txtName.Text = "";
            txtRoute.Text = "";
            txtiRacingRoute.Text = profiles.Config.IRacingFolder;
            
            chkActual.IsEnabled = true;
            txtName.IsEnabled = true;
            txtRoute.IsEnabled = true;
        }

        private void Button_New_Profile(object sender, RoutedEventArgs e) {
            reloadUI();
        }

        private void Button_Save_Profile(object sender, RoutedEventArgs e) {
            Profile profile = new Profile {
                Name = txtName.Text,
                Content = txtRoute.Text
            };
            profiles.AddProfiles(profile);
            reloadUI();
        }

        private void Button_Select_Profile(object sender, RoutedEventArgs e) {
            if(comboProfiles.SelectedIndex < 0 || comboProfiles.SelectedIndex > profiles.GetProfiles().Count) {
                return;
            }
            Profile p = profiles.GetProfiles().ElementAt(comboProfiles.SelectedIndex);
            profiles.SelectProfile(p);
        }
        
        private void Button_Select_IRacing_Folder(object sender, RoutedEventArgs e) {
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFolderDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK)) {
                txtiRacingRoute.Text = openFolderDialog.SelectedPath;
                IRacingRouteChange();
            }
        }

        private void Button_Select_File_Profile(object sender, RoutedEventArgs e) {
             OpenFileDialog openFileDialog = new OpenFileDialog();
             openFileDialog.Multiselect = false;
             openFileDialog.Filter = "Config files (*.cfg)|*.cfg";
             openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK)) {
                txtRoute.Text = openFileDialog.FileName;
            }
        }

        private void ChkActual_Checked(object sender, RoutedEventArgs e) {
            if (chkActual.IsChecked ?? false) {
                txtRoute.Text = Path.Combine(profiles.Config.IRacingFolder, Constants.IRACING_CONF_NAME);
                txtRoute.IsEnabled = false;
                btnSelectProfileFile.IsEnabled = false;
            } else {
                txtRoute.IsEnabled = true;
                btnSelectProfileFile.IsEnabled = true;
            }
        }

        private void TxtiRacingRoute_LostFocus(object sender, RoutedEventArgs e) {
            IRacingRouteChange();
        }

        private void IRacingRouteChange() {
            profiles.SetIRacingRoute(txtiRacingRoute.Text);
        }

        private void ComboProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(comboProfiles.SelectedIndex < 0) {
                return;
            }

            Profile p = profiles.GetProfiles().ElementAt(comboProfiles.SelectedIndex);
            txtName.Text = p.Name;
            txtRoute.Text = p.Content;
            chkActual.IsChecked = false;

            chkActual.IsEnabled = false;
            txtName.IsEnabled = false;
            txtRoute.IsEnabled = false;
        }

        private void Button_Del_Profile(object sender, RoutedEventArgs e) {
            MessageBoxResult messageBoxResult = MessageBox.Show(Internazionalization.Instance.getValueFromKey("messagebox_del_confirm_msg"),
                Internazionalization.Instance.getValueFromKey("messagebox_del_confirm_title"), MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No) {
                return;
            }
            if (comboProfiles.SelectedIndex < 0) {
                throw new iRacingProfileException("exception_profile_selected");
            }
            Profile p = profiles.GetProfiles().ElementAt(comboProfiles.SelectedIndex);
            profiles.DeleteProfile(p);
            reloadUI();
        }
        

        private void change_lang(object sender, RoutedEventArgs e) {
            string a = ((MenuItem)sender).Tag.ToString();
            profiles.SetLocalization(a);
            Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }
    }
}
