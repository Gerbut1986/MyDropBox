using DropBoxWCF.Classes;
using MaterialDesignThemes.Wpf;
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
    public partial class ElementMove : Window
    {
        private object sender;
        private string login;
        private string hash;

        public ElementMove(string login, string hash)
        {
            InitializeComponent();

            this.login = login;
            this.hash = hash;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.SelectAll();
        }

        private void bSearch_Click(object sender, RoutedEventArgs e)
        {
            var collection = CloudWorker.getFoldersByName(tbSearch.Text, login);

            foreach (var el in collection)
            {
                var sp = new StackPanel { Orientation = Orientation.Horizontal };
                sp.Children.Add(new PackIcon { Kind = PackIconKind.Folder, Foreground = new SolidColorBrush(Colors.Orange), Width = 30, Height = 30 });
                sp.Children.Add(new TextBlock { Text = el.Name, FontSize = 12 });
                
                var b = new Button { Content = sp, Tag =el.Hash };
                b.Click += B_Click;

                lbRes.Items.Add(b);
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            CloudWorker.MoveElement(hash,((Button)sender).Tag.ToString());
        }
    }
}
