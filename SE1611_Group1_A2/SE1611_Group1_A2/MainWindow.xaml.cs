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
        }

        private void shopping_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cart_Click(object sender, RoutedEventArgs e)
        {

        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            loginSuccessful();
        }
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            clearUserData();
            logoutSuccessful();
        }


        public void loginSuccessful()
        {
            string? userName = UserSession.UserName;
            
            menuLogin.Header = "Logout ("+userName+")";
            menuLogin.Click -= login_Click;
            menuLogin.Click += logout_Click;
            if(LoginWindow.adminRole == true)
            {
                menuAlbum.IsEnabled = true;
            }
            else
            {
                menuAlbum.IsEnabled = false;
            }
            
        }
        public void logoutSuccessful()
        {
            menuLogin.Header = "Login";
            menuLogin.Click -= logout_Click;
            menuLogin.Click += login_Click;
            menuAlbum.IsEnabled = false;
        }

        public void clearUserData()
        {
            UserSession.UserName = null;
            UserSession.Password= null;

        }
        private void album_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
