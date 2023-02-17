using Microsoft.EntityFrameworkCore;
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
        MusicStoreContext dbContext;
        public MainWindow()
        {
            InitializeComponent();
            dbContext = new MusicStoreContext();
            tbAuthor.Text = "SE1611_Group1 - Nguyễn Tiến Nhất | Đoàn Đức Khánh | Nguyễn Hữu Thành | Nguyễn Huy Hoàng";
            BitmapImage bitmap = new BitmapImage(new Uri("Background.png", UriKind.Relative));
            imgBackground.Source = bitmap;

            Settings.Default["CartId"] = string.Empty;
            Settings.Default["UserName"] = string.Empty;
            Settings.Default.Save();
            
            //MessageBox.Show(Settings.Default["CartId"].ToString() + "-" + Settings.Default["UserName"].ToString());
            
        }

        private void shopping_Click(object sender, RoutedEventArgs e)
        {
            ShoppingWindow shoppingWindow = new ShoppingWindow();
            shoppingWindow.ShowDialog();
        }
        private void cart_Click(object sender, RoutedEventArgs e)
        {
            CartWindow cartWindow = new CartWindow();
            cartWindow.Show();
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
            MainWindow main = new MainWindow();
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

            Settings.Default["UserName"] = UserSession.UserName;
            Settings.Default.Save();
            MigrateCart();
            menuCart.Header = "Cart (" + GetCount() + ")";

        }
        public void handleLogout()
        {
            menuLogin.Header = "Login";
            menuLogin.Click -= logout_Click;
            menuLogin.Click += login_Click;
            menuAlbum.IsEnabled = false;

            Settings.Default["UserName"] = string.Empty;
            Settings.Default.Save();
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
            albumWindow.ShowDialog();
        }

        //-----------------------------
        public void MigrateCart()
        {
            var shoppingCart = dbContext.Carts.Where(c => c.CartId == Settings.Default["CartId"].ToString()).ToList();
            foreach (Cart item in shoppingCart)
            {
                Cart userCartItem = dbContext.Carts.First(c => c.CartId == Settings.Default["UserName"].ToString() && c.AlbumId == item.AlbumId);
                if (userCartItem != null)
                {
                    userCartItem.Count += item.Count;
                    dbContext.Carts.Remove(item);
                }
                else
                {
                    item.CartId = Settings.Default["UserName"].ToString();
                }
            }
            dbContext.SaveChanges();
            Settings.Default["CartId"] = string.Empty;
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in dbContext.Carts
                          where cartItems.CartId == Settings.Default["UserName"].ToString()
                          select (int?)cartItems.Count).Count();
            // Return 0 if all entries are null
            return count ?? 0;
            
        }
    }
}
