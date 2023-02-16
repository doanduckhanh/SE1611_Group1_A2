using SE1611_Group1_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

namespace SE1611_Group1_A2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tbAuthor.Text = "SE1611_Group1 - Nguyễn Tiến Nhất | Đoàn Đức Khánh | Nguyễn Hữu Thành | Nguyễn Huy Hoàng";
            BitmapImage bitmap = new BitmapImage(new Uri("Background.png", UriKind.Relative));
            imgBackground.Source = bitmap;
        }

        private void shopping_Click(object sender, RoutedEventArgs e)
        {
            ShoppingWindow shoppingWindow = new ShoppingWindow();
            shoppingWindow.Show();
        }
        private void cart_Click(object sender, RoutedEventArgs e)
        {

        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            if (UserSession.UserName != null) handleLogin();
        }
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            clearUserData();
            handleLogout();
        }


        public void handleLogin()
        {
            menuLogin.Header = "Logout ("+ UserSession.UserName + ")";
            menuLogin.Click -= login_Click;
            menuLogin.Click += logout_Click;
            if(UserSession.Role == 1)
            {
                menuAlbum.IsEnabled = true;
            }
            else
            {
                menuAlbum.IsEnabled = false;
            }
            
        }
        public void handleLogout()
        {
            menuLogin.Header = "Login";
            menuLogin.Click -= logout_Click;
            menuLogin.Click += login_Click;
            menuAlbum.IsEnabled = false;
        }

        public void clearUserData()
        {
            UserSession.UserName = null;
            UserSession.Password = null;
            UserSession.Role = 0;
        }
        private void album_Click(object sender, RoutedEventArgs e)
        {
            AlbumWindow albumWindow = new AlbumWindow();
            albumWindow.Show();
        }
    }
}
