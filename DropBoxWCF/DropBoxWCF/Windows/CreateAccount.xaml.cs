using DropBoxWCF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DropBoxWCF.Windows
{
    public partial class CreateAccount : Window
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private async void bReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbLogin.Text.Length > 0 && tbName.Text.Length > 0 && tbEmail.Text.Length > 0 && pbPassword.Password.Length > 0)
                {// Reg code

                    bool result = false;

                    bReg.IsEnabled = false;

                    if (CloudWorker.Register(new WcfClassesLib.User { Login = tbLogin.Text, Email = tbEmail.Text, Name = tbName.Text, Password = pbPassword.Password }) == true)
                        result = true;


                    if (result == true)
                    {
                        MessageBox.Show("Successful registration!", "DropBox", MessageBoxButton.OK, MessageBoxImage.Information);

                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Some errors occured.\nMaybe, this login is already busy?", "DropBox", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
                else
                {
                    MessageBox.Show("You should fill all fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            bReg.IsEnabled = true;
        }
    }
}
