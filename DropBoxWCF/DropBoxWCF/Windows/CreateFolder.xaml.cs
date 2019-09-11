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
    public partial class CreateFolder : Window
    {
        public CreateFolder()
        {
            InitializeComponent();
        }

        public string FolderName { get; set; } = string.Empty;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text.Length > 0)
            {
                FolderName = tbName.Text;
                this.Close();
            }
            else
                MessageBox.Show("Вы не ввели имя папки!", "Инфо", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
