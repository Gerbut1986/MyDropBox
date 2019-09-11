using DropBoxWCF.Classes;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using WcfClassesLib;

namespace DropBoxWCF.Windows
{
    public partial class Lobby : Window
    {
        public Lobby()
        {
            InitializeComponent();

        }

        public Lobby(User user)
        {
            InitializeComponent();

            this.user = user;
            tblUserName.Text = "Current user: " + user.Name;

            _currentFolder = DropBoxWCF.Classes.CloudWorker.getRootFolder(user.Login); //
        }

        public User user = new User();
        private Folder _currentFolderStorage = new Folder();
        public Folder _currentFolder
        {
            get => _currentFolderStorage;
            set
            {
                try
                {
                    //foreach (var el in value.Content)
                    //{

                    wpFileSystemExplorer.Children.Clear();

                    List<File> lFiles = value.Content.Item2;
                    List<Folder> lFolders = value.Content.Item1;

                    int buttonHeight = 100;
                    int buttonWidth = 95;

                    //if (el is Folder)
                    foreach (var el in lFolders)
                    {
                        var ui_element = new StackPanel { Orientation = Orientation.Vertical };
                        ui_element.Children.Add(new PackIcon { HorizontalAlignment = HorizontalAlignment.Center, Kind = PackIconKind.Folder, Width = 64, Height = 64, Foreground = new SolidColorBrush(Colors.Orange) });
                        ui_element.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Center, Text = el.Name, FontSize = 14, Foreground = new SolidColorBrush(Colors.Black), FontWeight = FontWeights.Light });

                        var uiButton = new Button { Margin = new Thickness(7, 10, 5, 0), Tag = el, Content = ui_element, Style = Resources["MaterialDesignFlatButton"] as Style, Height = buttonHeight, Width = buttonWidth };
                        uiButton.Click += FileSystemUI_Selected;
                        uiButton.LostFocus += FileSystemUI_Unselected;
                        uiButton.MouseDoubleClick += FileSystemUI_DoubleClicked;

                        uiButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2F2F2"));

                        // ContextMenu
                        ContextMenu cm = new ContextMenu();
                        //cm.Items.Add(new Button { Style = Resources["MaterialDesignFlatButton"] as Style, Width = 120, Content = "Переместить", Tag = 1 });
                        Button b = new Button { Style = Resources["MaterialDesignFlatButton"] as Style, Width = 120, Content = "Delete", Foreground = new SolidColorBrush(Colors.Red), Tag = el };
                        b.Click += bDeleteFile;
                        cm.Items.Add(b);
                        //cm.Items.Add();
                        //foreach (var b in cm.Items)
                        //{
                        //    (b as Button).Click += ContextMenuObserver;
                        //}
                        uiButton.ContextMenu = cm;

                        wpFileSystemExplorer.Children.Add(uiButton);
                    }
                    foreach (var el in lFiles)
                    {
                        var ui_element = new StackPanel { Orientation = Orientation.Vertical };

                        PackIcon pi = UIWorker.GetPackIconByFileExtension("C:\\" + el.Name);
                        pi.HorizontalAlignment = HorizontalAlignment.Center;
                        pi.Width = 64; pi.Height = 64;
                        pi.Foreground = new SolidColorBrush(Colors.LightSkyBlue);
                        ui_element.Children.Add(pi);

                        ui_element.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Center, Text = el.Name, FontSize = 13, Foreground = new SolidColorBrush(Colors.Black), FontWeight = FontWeights.Light });

                        var uiButton = new Button { Margin = new Thickness(7, 10, 5, 0), Tag = el, Content = ui_element, Style = Resources["MaterialDesignFlatButton"] as Style, Height = buttonHeight, Width = buttonWidth };
                        uiButton.Click += FileSystemUI_Selected;
                        uiButton.LostFocus += FileSystemUI_Unselected;
                        uiButton.MouseDoubleClick += FileSystemUI_DoubleClicked;
                        uiButton.IsEnabled = false;

                        uiButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2F2F2"));


                        wpFileSystemExplorer.Children.Add(uiButton);
                    }
                    //}

                    _currentFolderStorage = value;
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
            }
        }

        public string SelectedHash { get; set; }

        private void ContextMenuObserver(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag)
            {
                case 1:
                    {
                        bMoveFile_Click(sender, e);
                        break;
                    }
                case 2:
                    {
                        bDeleteFile(sender, e);
                        break;
                    }
            }
        }

        private void FileSystemUI_Unselected(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2F2F2"));
        }
        private void FileSystemUI_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("clicked");

            if (((Button)sender).Tag is Folder)
                _currentFolder = CloudWorker.getFolderByHash(((Folder)((Button)sender).Tag).Hash.ToString(),user.Login);

        }
        private void FileSystemUI_Selected(object sender, RoutedEventArgs e)
        {
            SelectedHash = (((FileSystemElement)((Button)sender).Tag).Hash.ToString());
            ((Button)sender).Background = new SolidColorBrush(Colors.LightGray);
        }

        private void bAddFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog() { Title = "Выберите файл, который нужно загрузить в облако..." };
                if (ofd.ShowDialog() == true)
                {
                    byte[] file = System.IO.File.ReadAllBytes(ofd.FileName);
                    DropBoxWCF.Classes.CloudWorker.UploadFile(user.Login, ofd.FileName, _currentFolder.Hash, file, this);
                    _currentFolder = CloudWorker.getFolderByHash(_currentFolder.Hash,user.Login);
                }
            }
            catch { }
        }
        private void bCloudNavigation_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFolder.Hash.Equals("root").Equals(false))
                _currentFolder = CloudWorker.getFolderByHash(_currentFolder.ParentHash,user.Login);
        }
        private void bAddDir_Click(object sender, RoutedEventArgs e)
        {
            CreateFolder folder = new CreateFolder();
            folder.ShowDialog();
            if (folder.FolderName != string.Empty)
                CloudWorker.CreateFolder(folder.FolderName, _currentFolder.Hash, user.Login, this);
        }
        private void SnackbarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            sbMessage.IsActive = false;
        }
        private void bMoveFile_Click(object sender, RoutedEventArgs e)
        {
            ElementMove elMove = new ElementMove(user.Login, SelectedHash);
            elMove.ShowDialog();
        }
        private void bDeleteFile(object sender, RoutedEventArgs e)
        {
            try
            {
                CloudWorker.DeleteElement(((FileSystemElement)((Button)sender).Tag).Hash.ToString(), this);
            }
            catch { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}
