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
            if (UserSession.UserName != null)
            {
                MigrateCart();
                Settings.Default["CartId"] = UserSession.UserName;
                Settings.Default.Save();
            }          
        }







        public void EmptyCart()
        {
            var cartItems = storeDB.Carts
                .Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId)
                .Include(cart => cart.Album)
                .ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();

            // Set the order total of the shopping cart
            foreach (var item in cartItems)
                orderTotal += (item.Count * item.Album.Price);
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            // Save the order
            try
            {
                storeDB.Orders.Add(order);
                storeDB.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
            int orderID = storeDB.Orders.Select(o => o.OrderId).Max();
            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = orderID,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                try
                {
                    storeDB.OrderDetails.Add(orderDetail);
                    storeDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
            }
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return orderID;
        }




        //--------------------------------------- 
        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void lvAlbumId_Loaded(object sender, RoutedEventArgs e)
        {
            string cartId;
            if (UserSession.UserName == null)
            {
                btnCheckout.IsEnabled = false;
                cartId = Settings.Default["CartId"].ToString();
            }
            else
            { 
                btnCheckout.IsEnabled = true; 
                cartId = UserSession.UserName;
            }
            
            storeDB.Albums.ToList();
            lvAlbum.ItemsSource = storeDB.Carts.Where(x => x.CartId.Equals(cartId)).ToList();
            txtTotal.Text = GetTotal().ToString();
            
        }
        public int RemoveFromCart(int id)
        {
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

        public void MigrateCart()
        {
            var shoppingCart = storeDB.Carts.Where(c => c.CartId == Settings.Default["CartId"].ToString());
            foreach (Cart item in shoppingCart)
            {
                item.CartId = UserSession.UserName;
            }
            storeDB.SaveChanges();
            Settings.Default["CartId"] = string.Empty;
            Settings.Default.Save();
        }
    }
}
