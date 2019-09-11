using DropBoxWCF.Classes;
using DropBoxWCF.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WcfClassesLib;

namespace DropBoxWCF
{
    public partial class MainWindow : Window
    {
        #region CONSTRUCTORS
        // ---------------------- CONSTRUCTORS ----------------------
        public MainWindow()
        {
            InitializeComponent();

            CLog.Log("Открытие меню входа", 1.0);
            try
            {
                App.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            }
            catch (Exception ex) { CLog.Log(ex.Message, 1); Console.ReadLine(); }

            dlg_tbWrongPass.Text = "Login or password is uncorrect.\nCheck it and try again.";
        }
        #endregion CONSTRUCTORS

        #region VARIABLES
        // ---------------------- VARIABLES ----------------------
        private delegate void NoArgDelegate();
        #endregion VARIABLES

        #region EVENTS
        // ---------------------- EVENTS ----------------------
        private void Window_DragWin(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        private void lForgotPass_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).TextDecorations = TextDecorations.Underline;
        }
        private void lForgotPass_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).TextDecorations = null;
        }
        private async void bLogin_Click(object sender, RoutedEventArgs e)
        {
            // Check code
            string userName = string.Empty;
            if (tbLogin.Text.Length != 0 && pbPass.Password.Length != 0)
            {

                userName = DropBoxWCF.Classes.CloudWorker.Auth(tbLogin.Text, pbPass.Password);

                if (userName != string.Empty)
                {

                    Lobby lobby = new Lobby(new User { Name = userName, Login = tbLogin.Text });
                    lobby.Show();
                    this.Close();
                }
                else
                {
                    dlgWrongPass.IsOpen = true;
                }
            }
            else
            {
                dlgIncorrectInput.IsOpen = true;
            }



        }
        private void tbLogin_KeyUp(object sender, KeyEventArgs e)
        {
        }
        private void tbPass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tbLogin.Text.Length > 0)
            {
                bLogin_Click(null, null);
            }
        }
        private void lCreateAccount_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CreateAccount ca = new CreateAccount();
            ca.Show();
            this.Close();
        }
        #endregion EVENTS

        #region FUNCTIONS
        // ---------------------- FUNCTIONS ----------------------
        public async Task UserLoginProcessAsync(object data)
        {
            for (int i = 0; i < 1000; i += 2)
            {
                Thread.Sleep(1);

            }
            Console.WriteLine("end");

            pbLoginProcess.Dispatcher.Invoke(new Action(() => { pbLoginProcess.Visibility = Visibility.Hidden; }));
        }
        #endregion FUNCTIONS
    }
}
