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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace GamePlan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DbController.InitDb();
            buildGameList();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnminimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnmaximize_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            } else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void listGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string title = (string)listGames.SelectedItem;
            if(title != null)
            {
                BsonDocument game = DbController.GetOne(title);
                textBoxNotes.Text = (string)game.GetValue("notes");
            }
        }

        private void btnSaveNote_Click(object sender, RoutedEventArgs e)
        {
            string title = (string)listGames.SelectedItem;
            string newBody = textBoxNotes.Text;
            DbController.UpdateNote(title, newBody);
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            if(listGames.SelectedItem != null)
            {
                textBoxRename.Text = listGames.SelectedItem.ToString();
                textBoxRename.CaretIndex = System.Int32.MaxValue;
                textBoxRename.Visibility = Visibility.Visible;
                textBoxRename.Focus();
            }
        }

        private void textBoxRenameOnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string oldTitle = (string)listGames.SelectedItem;
                string newTitle = textBoxRename.Text;
                DbController.UpdateGameTitle(oldTitle, newTitle);
                buildGameList();
                textBoxRename.Visibility = Visibility.Hidden;
            }
        }

        private void buildGameList()
        {
            var games = DbController.GetAll();
            List<string> titles = new List<string>();
            foreach (var game in games)
            {
                titles.Add((string)game.GetValue("title"));
            }
            listGames.ItemsSource = titles;
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            textBoxNew.Text = "";
            textBoxNew.Visibility = Visibility.Visible;
            textBoxNew.Focus();
        }

        private void textBoxNewOnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string title = textBoxNew.Text;
                DbController.InsertOne(title);
                textBoxNew.Visibility = Visibility.Hidden;
                buildGameList();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listGames.SelectedItem != null)
            {
                string title = (string)listGames.SelectedItem;
                DbController.DeleteOne(title);
                buildGameList();
            }
        }
    }
}
