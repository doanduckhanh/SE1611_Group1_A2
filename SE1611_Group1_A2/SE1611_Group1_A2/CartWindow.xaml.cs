using Microsoft.EntityFrameworkCore;
using SE1611_Group1_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SE1611_Group1_A2
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        MusicStoreContext storeDB = new MusicStoreContext();
        string ShoppingCartId { get; set; }
        public CartWindow()
        {
            InitializeComponent();
            //if (UserSession.UserName != null)
            //{
                
            //    Settings.Default["UserName"] = UserSession.UserName;
            //    Settings.Default.Save();
            //}
            //else
            //{
            //    Settings.Default["UserName"] = string.Empty;
            //    Settings.Default.Save();
            //}          
        }


        //--------------------------------------- 
        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (GetTotal() > 0)
            {
                CheckoutWindow checkoutWindow = new CheckoutWindow(GetTotal().ToString());
                checkoutWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Total must be > 0");
            }
        }

        private void lvAlbumId_Loaded(object sender, RoutedEventArgs e)
        {
            var cart = GetCart();
            storeDB.Albums.ToList();
            lvAlbum.ItemsSource = storeDB.Carts.Where(x => x.CartId.Equals(cart.ShoppingCartId)).ToList();
            txtTotal.Text = GetTotal().ToString();

        }

        public static ShoppingWindow GetCart()
        {
            var cart = new ShoppingWindow();
            cart.ShoppingCartId = cart.GetCartId();
            return cart;
        }
        public int RemoveFromCart(int id)
        {
            //var cart = GetCart();
            // Get the matching cart and album id
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == Settings.Default["CartId"].ToString()
                && c.RecordId == id);

            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                // Save changes
                storeDB.SaveChanges();
            }
            return itemCount;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartId == Settings.Default["CartId"].ToString()
                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();
            return total ?? 0;
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Cart cart = b.CommandParameter as Cart;
            RemoveFromCart(cart.RecordId);
            lvAlbumId_Loaded(sender, e);
        }

        //public static ShoppingWindow GetCart()
        //{
        //    var cart = new ShoppingWindow();
        //    cart.ShoppingCartId = cart.GetCartId();
        //    return cart;
        //}

        public string GetCartId()
        {
            if (Settings.Default["CartId"].ToString().Equals(string.Empty))
            {
                if (!Settings.Default["UserName"].ToString().Equals(string.Empty))
                    Settings.Default["CartId"] = Settings.Default["UserName"].ToString();
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    Settings.Default["CartId"] = tempCartId.ToString();
                }
                Settings.Default.Save();
            }
            return Settings.Default["CartId"].ToString();
        }

        //public void MigrateCart()
        //{
        //    var shoppingCart = storeDB.Carts.Where(c => c.CartId == Settings.Default["CartId"].ToString()).ToList();
        //    MessageBox.Show("Migrate " + Settings.Default["CartId"].ToString());
        //    foreach (Cart item in shoppingCart)
        //    {           
        //            item.CartId = UserSession.UserName;
        //    }
            
        //    storeDB.SaveChanges();
        //    Settings.Default["CartId"] = string.Empty;
        //    Settings.Default.Save();
        //}

    }
}
